using Ksiegarnia.Models;

namespace Ksiegarnia.Repositories;

public interface IBooksRepository : IModelRepository<BookModel> {
    Task<BookModel?> GetByTitleAsync(string title);
}