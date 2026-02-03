using Ksiegarnia.Helpers;
using Ksiegarnia.Models;
using Ksiegarnia.ViewModels;
using Ksiegarnia.ViewModels.Authors;

namespace Ksiegarnia.ModelMappers.Implementation;

public class AuthorsMapper : IModelMapper<AuthorModel, AuthorViewModel> {
    
    public AuthorViewModel MapToViewModel(AuthorModel author) {
        return new AuthorViewModel {
            Id = author.Id,
            Name = author.Name,
            Age = author.Age,
            BookTitles = author.Books?.Select(book => book.Title).ToList()
        };
    }

    public AuthorModel MapFromViewModel(AuthorViewModel loanView) {
        return ModelsHelper.BuildNewAuthorWithValues(loanView.Name, loanView.Age);
    }
    
    public void ReplacePropertiesFromViewModel(AuthorModel author, AuthorViewModel loanView) {
        author.Name = loanView.Name;
        author.Age = loanView.Age;
    }
}