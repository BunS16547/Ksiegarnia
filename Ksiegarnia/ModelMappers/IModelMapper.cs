namespace Ksiegarnia.ModelMappers;

public interface IModelMapper<TModel, TViewModel> {
    public TViewModel MapToViewModel(TModel model);
    public TModel MapFromViewModel(TViewModel loanView);
    public void ReplacePropertiesFromViewModel(TModel model, TViewModel loanView);
}