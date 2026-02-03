using Ksiegarnia.Data;
using Ksiegarnia.Models;
using Microsoft.EntityFrameworkCore;

namespace Ksiegarnia.Repositories.Implementation;

public class AuthorsRepository : EfModelRepository<AuthorModel>, IAuthorsRepository {
    
    public AuthorsRepository(ApplicationDbContext context) : base(context) { }
    
    public override Task<List<AuthorModel>> GetAllAsync() {
        return _set
            .Include(author => author.Books)
            .AsNoTracking()
            .ToListAsync();
    }

    public override Task<AuthorModel?> GetByIdAsync(int id) {
        return _set
            .Include(author => author.Books)
            .AsNoTracking()
            .SingleOrDefaultAsync(author => author.Id == id);
    }
    
    public Task<AuthorModel?> GetByNameAsync(string name) {
        return _set
            .AsNoTracking()
            .FirstOrDefaultAsync(author => author.Name == name);
    }
}