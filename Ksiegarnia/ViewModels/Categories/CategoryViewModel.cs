using System.ComponentModel.DataAnnotations;

namespace Ksiegarnia.ViewModels.Categories;

public class CategoryViewModel {
    public int Id { get; init; }

    [Required,
     StringLength(64, ErrorMessage = "Category name can not be longer than 64 characters")]
    public required string Name { get; set; } = string.Empty;
    
    public List<string>? BookTitles { get; set; }
}