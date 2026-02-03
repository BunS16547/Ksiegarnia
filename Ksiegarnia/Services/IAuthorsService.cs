using Ksiegarnia.Models;
using Ksiegarnia.ViewModels;
using Ksiegarnia.ViewModels.Authors;

namespace Ksiegarnia.Services;

public interface IAuthorsService : IModelService<AuthorModel, AuthorViewModel> {
    Task<bool> HasUniqueNameAsync(string authorName, int? currentId);
}