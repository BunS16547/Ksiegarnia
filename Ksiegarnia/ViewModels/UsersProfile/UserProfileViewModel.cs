namespace Ksiegarnia.ViewModels.UsersProfile;

public class UserProfileViewModel {
    public string Email { get; set; } = string.Empty;
    public string Initials { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; }
    public bool EnableNotifications { get; set; }
    public required string UserRole { get; set; } = "User";
}