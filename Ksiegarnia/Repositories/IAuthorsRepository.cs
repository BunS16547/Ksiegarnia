using Ksiegarnia.Models;

namespace Ksiegarnia.Repositories;

public interface IAuthorsRepository : IModelRepository<AuthorModel> {
    Task<AuthorModel?> GetByNameAsync(string name);
}