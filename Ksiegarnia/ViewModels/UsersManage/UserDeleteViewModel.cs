namespace Ksiegarnia.ViewModels.UsersManage;

public class UserDeleteViewModel {
    public string Id { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Initials { get; set; }

    public bool EmailConfirmed { get; set; }
    
    public required string UserRole { get; set; }
}