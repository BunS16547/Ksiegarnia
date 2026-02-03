using Ksiegarnia.Models;
using Ksiegarnia.ViewModels;
using Ksiegarnia.ViewModels.Isbns;

namespace Ksiegarnia.Services;

public interface IIsbnsService : IModelService<IsbnModel, IsbnViewModel> {
    Task<bool> HasUniqueValueAsync(string isbnValue, int? currentId);
}