using Ksiegarnia.ModelMappers;
using Ksiegarnia.Models;
using Ksiegarnia.Repositories;
using Ksiegarnia.ViewModels.Categories;

namespace Ksiegarnia.Services.Implementation;

public class CategoriesService : EfModelService<CategoryModel, CategoryViewModel>, ICategoriesService {

    private readonly ICategoriesRepository _categoriesRepository;

    public CategoriesService(
        ICategoriesRepository repository,
        IModelMapper<CategoryModel, CategoryViewModel> mapper) : base(repository, mapper) {
        _categoriesRepository = repository;
    }

    public async Task<bool> HasUniqueNameAsync(string categoryName, int? currentId) {
        var foundCategory = await _categoriesRepository.GetByNameAsync(categoryName);
        return foundCategory == null || foundCategory.Id == currentId;
    }
}