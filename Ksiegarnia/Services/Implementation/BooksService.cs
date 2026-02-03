using System.Data;
using Ksiegarnia.Enums;
using Ksiegarnia.Helpers;
using Ksiegarnia.ModelMappers;
using Ksiegarnia.Models;
using Ksiegarnia.Repositories;
using Ksiegarnia.Types;
using Ksiegarnia.ViewModels.Books;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Ksiegarnia.Services.Implementation;

public class BooksService : EfModelService<BookModel, BookViewModel>, IBooksService {

    private readonly IIsbnsRepository _isbnsRepository;
    private readonly IAuthorsRepository _authorsRepository;
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly ILoansRepository _loansRepository;
    private readonly IBooksRepository _booksRepository;

    public BooksService(
        IModelMapper<BookModel, BookViewModel> mapper,
        IBooksRepository repository,
        IIsbnsRepository isbnsRepository,
        IAuthorsRepository authorsRepository,
        ICategoriesRepository categoriesRepository, 
        ILoansRepository loansRepository) : base(repository, mapper) {
        
        _isbnsRepository = isbnsRepository;
        _authorsRepository = authorsRepository;
        _categoriesRepository = categoriesRepository;
        _loansRepository = loansRepository;
        _booksRepository = repository;
    }

    public override async Task<List<BookViewModel>> GetAllViewsAsync() {
        var books = await _repository.GetAllAsync();
        var bookViews = books.Select(book => _mapper.MapToViewModel(book)).ToList();

        foreach (var bookView in bookViews) {
            var unavailableUntil = await GetUnavailableUntilAsync(bookView.Id);
            bookView.IsAvailableForRent = unavailableUntil == null;
            bookView.UnavailableUntil = unavailableUntil;
        }

        return bookViews;
    }
    
    public override async Task<BookViewModel?> GetViewByIdAsync(int id) {
        var book = await _repository.GetByIdAsync(id);
        if (book == null)
            return null;

        var bookView = _mapper.MapToViewModel(book);
        var unavailableUntil = await GetUnavailableUntilAsync(bookView.Id);
        bookView.IsAvailableForRent = unavailableUntil == null;
        bookView.UnavailableUntil = unavailableUntil;
        
        return bookView;
    }

    public override async Task DeleteAsync(int id) {
        var bookToBeDeleted = await _repository.GetByIdAsync(id);
        
        if (bookToBeDeleted == null)
            return;

        if (bookToBeDeleted.Isbn != null) {
            await _isbnsRepository.DeleteAsync(bookToBeDeleted.Isbn.Id);
        }

        await _repository.DeleteAsync(id);
    }
    
    public async Task<bool> HasUniqueTitleAsync(string bookTitle, int? bookId) {
        var foundBook = await _booksRepository.GetByTitleAsync(bookTitle);
        
        return foundBook == null || foundBook.Id == bookId;
    }

    public async Task<ServiceResult> ProcessFormSubmitAsync(BookViewModel bookView, FormActionsEnum action) {
        bookView.Title = bookView.Title.Trim();

        int? idForTitleCheck = action == FormActionsEnum.Create ? null : bookView.Id;
        bool hasUniqueTitle = await HasUniqueTitleAsync(bookView.Title, idForTitleCheck);

        if (!hasUniqueTitle) {
            return ServiceResult.Fail(
                nameof(bookView.Title),
                "Title of the book has to be unique");
        }
        
        IsbnModel? foundIsbn = null;
        AuthorModel? foundAuthor = null;
        CategoryModel? foundCategory = null;
        
        // rozpatrzenie podanego isbn
        if (!string.IsNullOrWhiteSpace(bookView.IsbnValue)) {
            foundIsbn = await _isbnsRepository.GetByValueAsync(bookView.IsbnValue.Trim());

            if (foundIsbn?.BookId != null) {
                
                if (action == FormActionsEnum.Create)
                    return ServiceResult.Fail(
                        nameof(bookView.IsbnValue),
                        "This Isbn is already assigned to an existing book");

                if (action == FormActionsEnum.Edit && foundIsbn.BookId != bookView.Id)
                    return ServiceResult.Fail(
                        nameof(bookView.IsbnValue),
                        "This Isbn is already assigned to a different book");
            }
        }
        
        // rozpatrzenie podanej kategorii
        if (!string.IsNullOrWhiteSpace(bookView.CategoryName)) {
            foundCategory = await _categoriesRepository.GetByNameAsync(bookView.CategoryName.Trim());
            
            if (foundCategory == null) {
                return ServiceResult.Fail(
                    nameof(bookView.CategoryName),
                    "This Category does not exist");
            }
        }
        
        // rozpatrzenie podanego autora
        if (!string.IsNullOrWhiteSpace(bookView.AuthorName)) {
            foundAuthor = await _authorsRepository.GetByNameAsync(bookView.AuthorName.Trim());
            
            if (foundAuthor == null) {
                return ServiceResult.Fail(
                    nameof(bookView.AuthorName),
                    "This Author does not exist");
            }
        }

        var processedBook = action == FormActionsEnum.Create
            ? await AddFromViewAsync(bookView)
            : await _repository.GetByIdAsync(bookView.Id);
        
        if (processedBook == null) {
            throw new DataException("There was a problem with creating a new book");
        }
        
        var bookId = processedBook.Id;
        
        if (action == FormActionsEnum.Edit)
            _mapper.ReplacePropertiesFromViewModel(processedBook, bookView);
        
        // aktualizacje pól zależnie od tego czy znaleziono pasujące modelu autora, kategorii i isbn
        processedBook.CategoryId = foundCategory?.Id;
        processedBook.AuthorId = foundAuthor?.Id;
        
        // stwierdziłem że jeśli zmieniony zostanie ISBN w edit to po prostu usuwam stary i tworzę nowy
        if (!string.IsNullOrWhiteSpace(bookView.IsbnValue)) {
            if (action == FormActionsEnum.Edit && (foundIsbn == null || foundIsbn.BookId != bookId)) {
                var oldIsbn = await _isbnsRepository.GetByBookIdAsync(bookId);
                if (oldIsbn != null) {
                    await _isbnsRepository.DeleteAsync(oldIsbn.Id);
                }
            }
            
            if (foundIsbn == null) {
                var newIsbn = ModelsHelper.BuildNewIsbnWithValues(bookView.IsbnValue.Trim(), bookId);
                await _isbnsRepository.AddAsync(newIsbn);
            } else if (foundIsbn.BookId != bookId) {
                foundIsbn.BookId = bookId;
                await _isbnsRepository.UpdateAsync(foundIsbn);
            }
        }
        else if (action == FormActionsEnum.Edit) {
            var oldIsbn = await _isbnsRepository.GetByBookIdAsync(bookId);
            if (oldIsbn != null) {
                await _isbnsRepository.DeleteAsync(oldIsbn.Id);
            }
        }
        
        
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> RentBookAsync(int bookId, string userId, int days) {
        if (days <= 0 || days > 60)
            return ServiceResult.Fail("", "Invalid rental period found");

        var foundBook = await _booksRepository.GetByIdAsync(bookId);
        if (foundBook == null) 
            return ServiceResult.Fail("", "Book not found.");

        var foundLoan = await _loansRepository.GetActiveLoanByBookIdAsync(bookId);

        if (foundLoan != null)
            return ServiceResult.Fail("", "This book is currently unavailable.");
        
        var dateTimeNow = DateTime.UtcNow;

        var newLoan = new LoanModel
        {
            BookId = bookId,
            UserId = userId,
            LoanedAt = dateTimeNow,
            DueAt = dateTimeNow.AddDays(days),
            ReturnedAt = null
        };

        await _loansRepository.AddAsync(newLoan);

        return ServiceResult.Success();
    }
    
    public async Task<ServiceResult> ReturnBookAsync(int bookId, string userId, bool isAdmin = false) {
        LoanModel? foundLoan;

        if (isAdmin) {
            foundLoan = await _loansRepository.GetActiveLoanByBookIdAsync(bookId);
        }
        else {
            foundLoan = await _loansRepository.GetActiveLoanByBookIdAndUserIdAsync(bookId, userId);
        }
        
        if (foundLoan == null)
            return ServiceResult.Fail("", "Active loan not found");

        foundLoan.ReturnedAt = DateTime.UtcNow;
        return ServiceResult.Success();
    }
    
    public async Task<DateTime?> GetUnavailableUntilAsync(int bookId) {
        var connectedLoan = await _loansRepository.GetActiveLoanByBookIdAsync(bookId);

        return connectedLoan?.DueAt;
    }

    public async Task<RentBookViewModel?> GetRentViewByIdAsync(int bookId) {
        var bookView = await GetViewByIdAsync(bookId);

        if (bookView == null)
            return null;
        
        return new RentBookViewModel {
            Title = bookView.Title,
            IsAvailableForRent = bookView.IsAvailableForRent,
            UnavailableUntil = bookView.UnavailableUntil
        };
        
    }
}