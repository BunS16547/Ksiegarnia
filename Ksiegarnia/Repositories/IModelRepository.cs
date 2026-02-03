namespace Ksiegarnia.Repositories;

// generyczny szkielet dla repozytorium definiujący standardowe metody CRUD do operacji na tabelach bazy danych
public interface IModelRepository<TModel> where TModel : class {
    Task<List<TModel>> GetAllAsync();
    Task<TModel?> GetByIdAsync(int id);
    Task AddAsync(TModel model);
    Task UpdateAsync(TModel model);
    Task DeleteAsync(int id);
}