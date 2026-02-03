using System.ComponentModel.DataAnnotations;

namespace Ksiegarnia.ViewModels.UsersManage;

public class UserFormViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, 
     MinLength(2, ErrorMessage = "Initials have to be at least 2 characters long"), 
     MaxLength(3, ErrorMessage = "Initials can not be longer than 3 characters")]
    public string Initials { get; set; } = string.Empty;

    public bool EmailConfirmed { get; set; }

    public bool EnableNotifications { get; set; }

    // UI only
    public List<string> AvailableRoles { get; set; } = new();
    
    [Required]
    public required string SelectedRole { get; set; } = "User";
}
