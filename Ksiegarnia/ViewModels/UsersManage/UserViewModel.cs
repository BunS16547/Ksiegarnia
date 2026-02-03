namespace Ksiegarnia.ViewModels.UsersManage;

public class UserViewModel {
    public string Id { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "User";
    public string? Initials { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
}
