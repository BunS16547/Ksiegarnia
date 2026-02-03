using Ksiegarnia.Helpers;
using Ksiegarnia.ViewModels;
using Ksiegarnia.Models;
using Ksiegarnia.ViewModels.Books;

namespace Ksiegarnia.ModelMappers.Implementation;

public class BooksMapper : IModelMapper<BookModel, BookViewModel> {
    
    public BookViewModel MapToViewModel(BookModel book) {
        return new BookViewModel {
            Id = book.Id,
            Title = book.Title,
            IsbnValue = book.Isbn?.Value ?? "",
            CategoryName = book.Category?.Name,
            AuthorName = book.Author?.Name
        };
    }

    public BookModel MapFromViewModel(BookViewModel loanView) {
        return ModelsHelper.BuildNewBookWithValues(loanView.Title);
}

    public void ReplacePropertiesFromViewModel( BookModel book, BookViewModel loanView) {
        book.Title = loanView.Title;
    }
}