using Ksiegarnia.ModelMappers;
using Ksiegarnia.Repositories;
using Ksiegarnia.Types;

namespace Ksiegarnia.Services;

// generyczny serwis definiujący metody serwisów do użytku kontrolerów
public class EfModelService<TModel, TViewModel> : IModelService<TModel, TViewModel> 
    where TModel : class, IHasIntId 
    where TViewModel : class {

    protected readonly IModelRepository<TModel> _repository;
    protected readonly IModelMapper<TModel, TViewModel> _mapper;
    
    protected EfModelService(IModelRepository<TModel> repository, IModelMapper<TModel, TViewModel> mapper) {
        _repository = repository;
        _mapper = mapper;
    }
    
    public virtual Task<List<TModel>> GetAllAsync() {
        return _repository.GetAllAsync();
    }
    
    public virtual Task<TModel?> GetByIdAsync(int id) {
        return _repository.GetByIdAsync(id);
    }

    public virtual async Task<List<TViewModel>> GetAllViewsAsync() {
        var models = await _repository.GetAllAsync();
        return models
            .Select(_mapper.MapToViewModel)
            .ToList();
    }

    public virtual async Task<TViewModel?> GetViewByIdAsync(int id) {
        var model = await _repository.GetByIdAsync(id);

        if (model == null)
            return null;

        return _mapper.MapToViewModel(model);
    }

    public Task AddAsync(TModel model) {
        return _repository.AddAsync(model);
    }
    
    public Task UpdateAsync(TModel model) {
        return _repository.UpdateAsync(model);
    }

    public virtual Task DeleteAsync(int id) {
        return _repository.DeleteAsync(id);
    }

    public virtual async Task<TModel> AddFromViewAsync(TViewModel view)
    {
        var model = _mapper.MapFromViewModel(view);
        await _repository.AddAsync(model);
        return model;
    }

    public virtual async Task UpdateFromViewAsync(TViewModel viewModel, int id)
    {
        var model = await _repository.GetByIdAsync(id);
        if (model == null)
            throw new ArgumentException("No model with this id exists");

        _mapper.ReplacePropertiesFromViewModel(model, viewModel);
        await _repository.UpdateAsync(model);
    }
}