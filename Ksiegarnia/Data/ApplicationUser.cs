using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Ksiegarnia.Data;

public class ApplicationUser : IdentityUser {
    [Required, MinLength(2), MaxLength(3)]
    public required string Initials { get; set; } = "UU";
    
    [Required]
    public bool EnableNotifications { get; set; } = true;
};