using System.ComponentModel.DataAnnotations;

namespace Ksiegarnia.ViewModels.UsersProfile;

public class UserProfileEditViewModel {
    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required, 
     MinLength(2, ErrorMessage = "Initials have to be at least 2 characters long"), 
     MaxLength(3, ErrorMessage = "Initials can not be longer than 3 characters")]
    public string Initials { get; set; } = "";

    public bool EnableNotifications { get; set; } = true;
}