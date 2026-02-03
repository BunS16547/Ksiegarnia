using System.ComponentModel.DataAnnotations;

namespace Ksiegarnia.ViewModels.Books;

public class BookViewModel {
    public int Id { get; init; }
    
    [Required(ErrorMessage = "Title of the book is required"),
    MaxLength(128, ErrorMessage = "Title of the book must be shorter than 128 characters"),] 
    public required string Title { get; set; }
    
    [Required(ErrorMessage = "Book has to be confirmed if it is for rent")]
    public bool IsAvailableForRent { get; set; } = true;
    
    public DateTime? UnavailableUntil { get; set; }


    [Required(ErrorMessage = "Isbn value is required. If it does not exist it will be created"),
     MaxLength(13, ErrorMessage = "Isbn has to be exactly 13 characters"),
     MinLength(13, ErrorMessage = "Isbn has to be exactly 13 characters")]
    public string IsbnValue { get; set; } = string.Empty;
    
    [StringLength(64, ErrorMessage = "Category name can not be longer than 64 characters")]
    public string? CategoryName { get; set; }
    
    [MinLength(4, ErrorMessage = "The author's name must be at least 4 characters long"),
     MaxLength(64, ErrorMessage = "The author's name can't be longer than 64 characters ")]
    public string? AuthorName { get; set; }
}