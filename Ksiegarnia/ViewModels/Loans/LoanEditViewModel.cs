using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ksiegarnia.ViewModels.Loans;

public class LoanEditViewModel
{
    [Required]
    public int Id { get; set; }

    // Do wyświetlania (nie jako źródło prawdy)
    public string CurrentBookTitle { get; set; } = "";
    public string CurrentUserEmail { get; set; } = "";
    
    public int CurrentBookId { get; set; }
    public string CurrentUserId { get; set; } = "";
    
    public int? NewBookId { get; set; }         
    public string? NewUserId { get; set; }     

    public bool HasBeenReturned { get; set; }

    public DateTime LoanedAt { get; set; }
    public DateTime DueAt { get; set; }
    public DateTime? ReturnedAt { get; set; }
    
    public List<SelectListItem> AvailableBooks { get; set; } = new();
    public List<SelectListItem> AvailableUsers { get; set; } = new();
}