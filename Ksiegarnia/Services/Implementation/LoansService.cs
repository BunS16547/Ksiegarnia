using Ksiegarnia.ModelMappers;
using Ksiegarnia.Models;
using Ksiegarnia.Repositories;
using Ksiegarnia.ViewModels.Loans;

namespace Ksiegarnia.Services.Implementation;

public class LoansService : EfModelService<LoanModel, LoanViewModel>, ILoansService {
    private readonly IBooksRepository _booksRepository;
    private readonly ILoansRepository _loansRepository;
    private readonly ILoansMapper _loansMapper;

    public LoansService(
        ILoansRepository repository,
        IBooksRepository booksRepository, 
        ILoansMapper mapper) : base(repository, mapper) {
        
        _booksRepository = booksRepository;
        _loansMapper = mapper;
        _loansRepository = repository;
    }

    public async Task<bool> CheckIfBookIsAvailable(int bookId) {
        var foundLoan = await _loansRepository.GetActiveLoanByBookIdAsync(bookId);

        return foundLoan == null || foundLoan.ReturnedAt != null;
    }

    public async Task<List<BookModel>> GetAllAvailableBooksAsync() {
        var books = await _booksRepository.GetAllAsync();
        var availableBooks = new List<BookModel>();
        
        foreach (var book in books) {
            if (await CheckIfBookIsAvailable(book.Id)) {
                availableBooks.Add(book);
            }
        }

        return availableBooks;
    }

    public async Task<LoanEditViewModel?> GetEditViewByIdAsync(int id) {
        var loan = await _repository.GetByIdAsync(id);
        if (loan == null) 
            return null;
        
        return _loansMapper.MapToEditView(loan);
    }

    public async Task<LoanDeleteViewModel?> GetDeleteViewByIdAsync(int id) {
        var loan = await _repository.GetByIdAsync(id);
        if (loan == null) 
            return null;

        return _loansMapper.MapToDeleteView(loan);
    }

    public Task AddFromCreateViewAsync(LoanCreateViewModel loanCreateView) {
        var now = DateTime.UtcNow;

        var loan = new LoanModel
        {
            BookId = loanCreateView.BookId,
            UserId = loanCreateView.UserId,
            LoanedAt = now,
            DueAt = now.AddDays(loanCreateView.Days),
            ReturnedAt = null
        };

        return _repository.AddAsync(loan);

    }
    
    public async Task UpdateFromEditViewAsync(LoanEditViewModel loanEditView)
    {
        var loan = await _repository.GetByIdAsync(loanEditView.Id);
        if (loan == null)
            throw new InvalidOperationException("Loan not found.");

        // User: jeśli null albo taki sam -> nie zmieniaj
        if (!string.IsNullOrWhiteSpace(loanEditView.NewUserId) && loanEditView.NewUserId != loan.UserId)
            loan.UserId = loanEditView.NewUserId;

        // Book: jeśli null albo taki sam -> nie zmieniaj
        if (loanEditView.NewBookId.HasValue && loanEditView.NewBookId.Value != loan.BookId)
            loan.BookId = loanEditView.NewBookId.Value;

        // Zwrócenie: tylko jeśli checkbox true i jeszcze nie zwrócone
        if (loanEditView.HasBeenReturned && loan.ReturnedAt == null)
            loan.ReturnedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(loan);
    }

}