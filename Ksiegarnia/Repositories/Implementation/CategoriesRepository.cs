using Ksiegarnia.Data;
using Ksiegarnia.Models;
using Microsoft.EntityFrameworkCore;

namespace Ksiegarnia.Repositories.Implementation;

public class CategoriesRepository : EfModelRepository<CategoryModel>, ICategoriesRepository {
    
    public CategoriesRepository(ApplicationDbContext context) : base(context) { }
    
    public override Task<List<CategoryModel>> GetAllAsync() {
        return _set
            .Include(category => category.Books)
            .AsNoTracking()
            .ToListAsync();
    }

    public override Task<CategoryModel?> GetByIdAsync(int id) {
        return _set
            .Include(category => category.Books)
            .SingleOrDefaultAsync(category => category.Id == id);
    }

    public Task<CategoryModel?> GetByNameAsync(string name) {
        return _set
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.Name == name);
    }
}