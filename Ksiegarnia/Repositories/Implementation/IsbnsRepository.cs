using Ksiegarnia.Data;
using Ksiegarnia.Models;
using Microsoft.EntityFrameworkCore;

namespace Ksiegarnia.Repositories.Implementation;

public class IsbnsRepository : EfModelRepository<IsbnModel>, IIsbnsRepository {
    
    public IsbnsRepository(ApplicationDbContext context) : base(context) { }
    
    public override Task<List<IsbnModel>> GetAllAsync() {
        return _set
            .Include(isbn => isbn.Book)
            .AsNoTracking()
            .ToListAsync();
    }

    public override Task<IsbnModel?> GetByIdAsync(int id) {
        return _set
            .Include(isbn => isbn.Book)
            .SingleOrDefaultAsync(isbn => isbn.Id == id);
    }

    public Task<IsbnModel?> GetByBookIdAsync(int bookId) {
        return _set
            .SingleOrDefaultAsync(isbn => isbn.BookId == bookId);
    }
    
    public Task<IsbnModel?> GetByValueAsync(string value) {
        return _set
            .AsNoTracking()
            .FirstOrDefaultAsync(isbn => isbn.Value == value);
    }
    
}