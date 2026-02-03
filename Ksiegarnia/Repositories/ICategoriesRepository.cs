using Ksiegarnia.Models;

namespace Ksiegarnia.Repositories;

public interface ICategoriesRepository : IModelRepository<CategoryModel> {
    Task<CategoryModel?> GetByNameAsync(string name);
}