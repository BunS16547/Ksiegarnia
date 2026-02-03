using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ksiegarnia.ViewModels.Loans;

public class LoanCreateViewModel {
    [Required]
    public int BookId { get; set; }

    [Required]
    public string UserId { get; set; } = "";

    [Range(1, 60)]
    public int Days { get; set; } = 14;

    public List<SelectListItem> AvailableBooks { get; set; } = new();
    public List<SelectListItem> AvailableUsers { get; set; } = new();
}