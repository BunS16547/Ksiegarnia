using System.ComponentModel.DataAnnotations;

namespace Ksiegarnia.ViewModels.Books;

public class RentBookViewModel
{
    [Required]
    public int Id { get; set; }

    public required string Title { get; set; } = string.Empty;

    public required bool IsAvailableForRent { get; set; }
    public DateTime? UnavailableUntil { get; set; }

    [Range(1, 60)]
    public int Days { get; set; } = 14;
}