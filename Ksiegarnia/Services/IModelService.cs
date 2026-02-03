namespace Ksiegarnia.Services;

// generyczny szkielet dla seriwsu definiujący metody serwisów do użytku kontrolerów
public interface IModelService<TModel, TViewModel> 
    where TModel : class
    where TViewModel : class {
    
    Task<List<TViewModel>> GetAllViewsAsync();
    Task<List<TModel>> GetAllAsync();
    Task<TViewModel?> GetViewByIdAsync(int id);
    Task<TModel?> GetByIdAsync(int id);
    Task AddAsync(TModel model);
    Task UpdateAsync(TModel model);
    Task DeleteAsync(int id);
    
    Task UpdateFromViewAsync(TViewModel bookView, int id);
    Task<TModel> AddFromViewAsync(TViewModel bookView);
}