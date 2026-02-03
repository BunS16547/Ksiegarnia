using Ksiegarnia.Helpers;
using Ksiegarnia.Models;
using Ksiegarnia.ViewModels;
using Ksiegarnia.ViewModels.Categories;

namespace Ksiegarnia.ModelMappers.Implementation;

public class CategoriesMapper : IModelMapper<CategoryModel, CategoryViewModel> {
    
    public CategoryViewModel MapToViewModel(CategoryModel category) {
        return new CategoryViewModel {
            Id = category.Id,
            Name = category.Name,
            BookTitles = category.Books?.Select(book => book.Title).ToList()
        };
    }

    public CategoryModel MapFromViewModel(CategoryViewModel loanView) {
        return ModelsHelper.BuildNewCategoryWithValues(loanView.Name);
    }

    public void ReplacePropertiesFromViewModel(CategoryModel category, CategoryViewModel loanView) {
        category.Name = loanView.Name;
    }
}