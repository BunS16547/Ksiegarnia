using Ksiegarnia.Enums;

namespace Ksiegarnia.Data;

using Microsoft.AspNetCore.Identity;


// klasa seedująca identity w projekcie
public static class IdentitySeed
{
    // zaseedowanie ról
    public static async Task SeedRolesAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roles = Enum.GetNames<RolesEnum>().ToList();
        
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
    
    // zaseedowanie admina
    public static async Task SeedAdminAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        // odniesienie do RoleManager i UserManager z Identity
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // z user-secrets pobranie danych
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var adminEmail = config["SeedAdmin:Email"];
        var adminPassword = config["SeedAdmin:Password"];
        var adminRole = config["SeedAdmin:Role"] ?? "Admin";

        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
            return;
        
        // rola
        if (!await roleManager.RoleExistsAsync(adminRole))
        {
            await roleManager.CreateAsync(new IdentityRole(adminRole));
        }

        // user
        var user = await userManager.FindByEmailAsync(adminEmail);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                Initials = "AD"
            };

            var createRes = await userManager.CreateAsync(user, adminPassword);
            if (!createRes.Succeeded)
            {
                var msg = string.Join("; ", createRes.Errors.Select(e => e.Description));
                throw new Exception("Admin seed failed: " + msg);
            }
        }

        // przypisz rolę
        if (!await userManager.IsInRoleAsync(user, adminRole))
        {
            await userManager.AddToRoleAsync(user, adminRole);
        }
    }
}
