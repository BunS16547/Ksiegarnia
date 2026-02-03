using Ksiegarnia.Models;

namespace Ksiegarnia.Repositories;

public interface IIsbnsRepository : IModelRepository<IsbnModel> {
    Task<IsbnModel?> GetByValueAsync(string value);
    Task<IsbnModel?> GetByBookIdAsync(int bookId);
}