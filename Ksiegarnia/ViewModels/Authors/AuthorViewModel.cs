using System.ComponentModel.DataAnnotations;

namespace Ksiegarnia.ViewModels.Authors;

public class AuthorViewModel {
    public int Id { get; init; }
    
    [Required,
     MinLength(4, ErrorMessage = "Author's name has to be at least 4 characters long"),
     MaxLength(64, ErrorMessage = "Author's name can not be longer than 64 characters")]
    public required string Name { get; set; }
    
    [Required,
     Range(1, 150, ErrorMessage = "Age has to be a positive number between 1 and 150")]
    public int Age { get; set; }
    
    public List<string>? BookTitles { get; set; }
}