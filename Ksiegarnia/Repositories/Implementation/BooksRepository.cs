using Ksiegarnia.Data;
using Ksiegarnia.Models;
using Microsoft.EntityFrameworkCore;

namespace Ksiegarnia.Repositories.Implementation;

public class BooksRepository : EfModelRepository<BookModel>, IBooksRepository {
    
    public BooksRepository(ApplicationDbContext context) : base(context) { }

    public override Task<List<BookModel>> GetAllAsync() {
        return _set
            .Include(book => book.Author)
            .Include(book => book.Isbn)
            .Include(book => book.Category)
            .AsNoTracking()
            .ToListAsync();
    }

    public override Task<BookModel?> GetByIdAsync(int id) {
        return _set
            .Include(book => book.Author)
            .Include(book => book.Isbn)
            .Include(book => book.Category)
            .SingleOrDefaultAsync(book => book.Id == id);
    }

    public Task<BookModel?> GetByTitleAsync(string title) {
        return _set
            .AsNoTracking()
            .FirstOrDefaultAsync(book => book.Title == title);
    }
}