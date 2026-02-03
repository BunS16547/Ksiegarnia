using Ksiegarnia.Models;
using Ksiegarnia.ViewModels;
using Ksiegarnia.ViewModels.Categories;

namespace Ksiegarnia.Services;

public interface ICategoriesService : IModelService<CategoryModel, CategoryViewModel> {
    Task<bool> HasUniqueNameAsync(string categoryName, int? currentId);
}