using System.ComponentModel.DataAnnotations;

namespace Ksiegarnia.ViewModels.UsersManage;

public class UserEditViewModel : UserFormViewModel {
    [Required]
    public string Id { get; set; } = "";
    
    [Range(1, 43200)] // do 30 dni
    public int? LockoutMinutes { get; set; }

    public bool ResetLockOut { get; set; } = false;
    
    // tylko do przeglądu
    public DateTimeOffset? LockoutEnd { get; set; }
}