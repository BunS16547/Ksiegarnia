using System.ComponentModel.DataAnnotations;

namespace Ksiegarnia.ViewModels.UsersManage;

public class UserResetPasswordViewModel
{
    [Required]
    public string Id { get; set; } = string.Empty;

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