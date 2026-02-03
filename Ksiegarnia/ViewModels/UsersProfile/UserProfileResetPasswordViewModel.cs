using System.ComponentModel.DataAnnotations;

namespace Ksiegarnia.ViewModels.UsersProfile;

public class UserProfileResetPasswordViewModel
{
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [MinLength(6)]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword))]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}