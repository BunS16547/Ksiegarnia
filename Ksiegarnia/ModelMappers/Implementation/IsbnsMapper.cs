using Ksiegarnia.Helpers;
using Ksiegarnia.Models;
using Ksiegarnia.ViewModels;
using Ksiegarnia.ViewModels.Isbns;

namespace Ksiegarnia.ModelMappers.Implementation;

public class IsbnsMapper : IModelMapper<IsbnModel, IsbnViewModel> {
    public IsbnViewModel MapToViewModel(IsbnModel isbn) {
        return new IsbnViewModel {
            Id = isbn.Id,
            Value = isbn.Value,
            BookTitle = isbn.Book?.Title
        };
    }

    public IsbnModel MapFromViewModel(IsbnViewModel loanView) {
        return ModelsHelper.BuildNewIsbnWithValues(loanView.Value);
    }

    public void ReplacePropertiesFromViewModel(IsbnModel isbn, IsbnViewModel loanView) {
        isbn.Value = loanView.Value;
    }
}