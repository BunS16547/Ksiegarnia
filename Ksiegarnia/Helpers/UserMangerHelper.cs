using Ksiegarnia.Data;
using Microsoft.AspNetCore.Identity;

namespace Ksiegarnia.Helpers;

public static class UserMangerHelper {
    public static async Task<IdentityResult> SetSingleRoleAsync(
        this UserManager<ApplicationUser> userManager,
        ApplicationUser user, string role) {
        
        // pobranie ról użytkownika, usunięcie jeśli są i dodanie tylko jednej podanej
        // zrobiłem to w celu wymuszenia tylko jednej roli u użytkownika
        var currentRoles = await userManager.GetRolesAsync(user);
        if (currentRoles.Any())
            await userManager.RemoveFromRolesAsync(user, currentRoles);

        return await userManager.AddToRoleAsync(user, role);
    }
}