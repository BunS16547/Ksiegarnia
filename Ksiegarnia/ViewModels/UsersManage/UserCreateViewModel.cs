using System.ComponentModel.DataAnnotations;

namespace Ksiegarnia.ViewModels.UsersManage;

public class UserCreateViewModel : UserFormViewModel {
    [Required]
    [DataType(DataType.Password)]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
}