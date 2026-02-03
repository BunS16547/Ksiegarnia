using Ksiegarnia.Data;
using Microsoft.EntityFrameworkCore;

namespace Ksiegarnia.Repositories;

// generyczne repozytorium definiujące standardowe metody CRUD do operacji na tabelach bazy danych
public class EfModelRepository<TModel> : IModelRepository<TModel> where TModel : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TModel> _set;

    protected EfModelRepository(ApplicationDbContext context)
    {
        _context = context;
        _set = context.Set<TModel>();
    }

    public virtual Task<List<TModel>> GetAllAsync() {
        return _set.AsNoTracking().ToListAsync();
    }

    public virtual Task<TModel?> GetByIdAsync(int id) {
        return _set.FindAsync(id).AsTask();
    }
    
    public virtual async Task AddAsync(TModel model)
    {
        _set.Add(model);
        await _context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TModel model)
    {
        _set.Update(model);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(int id)
    {
        var model = await GetByIdAsync(id);
        if (model is null) 
            return;

        _set.Remove(model);
        await _context.SaveChangesAsync();
    }
}
