using System.ComponentModel.DataAnnotations;

namespace Ksiegarnia.ViewModels.Isbns;

public class IsbnViewModel {
    public int Id { get; init; }

    [Required,
     MinLength(13, ErrorMessage = "Isbn has to be exactly 13 characters long"),
     MaxLength(13, ErrorMessage = "Isbn has to be exactly 13 characters long")] 
    public required string Value { get; set; }
    
    [StringLength(64)]
    public string? BookTitle { get; set; }
}