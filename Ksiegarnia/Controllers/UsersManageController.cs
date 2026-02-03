using System.Data;
using Ksiegarnia.Data;
using Ksiegarnia.Helpers;
using Ksiegarnia.ViewModels.UsersManage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ksiegarnia.Controllers;

// controller użytkowników tylko dla admina
[Authorize(Policy = "AdminOnly")]
public class UsersManageController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsersManageController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    // sprawdzenie czy to ten sam użytkownik jest zalogowany co ten o którego dane poproszono
    private bool IsCurrentlyLoggedInUser(string id)
    {
        var currentUserId = _userManager.GetUserId(User);
        return string.Equals(currentUserId, id, StringComparison.Ordinal);
    }
    
    // LISTA
    public async Task<IActionResult> Index()
    {
        
        var usersBase = await _userManager.Users
            .OrderBy(user => user.Email)
            .Select(user => 
                new UserViewModel {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    Initials = user.Initials,
                    LockoutEnd = user.LockoutEnd
            })
            .ToListAsync();
        
        var users = new List<UserViewModel>(usersBase.Count);

        foreach (var userView in usersBase)
        {
            var foundUser = await _userManager.FindByIdAsync(userView.Id);
            var roles = foundUser != null
                ? await _userManager.GetRolesAsync(foundUser)
                : Array.Empty<string>();

            users.Add(new UserViewModel
            {
                Id = userView.Id,
                Email = userView.Email,
                Role = roles.SingleOrDefault() ?? "User",
                Initials = userView.Initials,
                LockoutEnd = userView.LockoutEnd
            });
        }
        
        return View(users);
    }

    // CREATE (GET)
    public async Task<IActionResult> Create()
    {
        var roles = await _roleManager.Roles.Select(role => role.Name!).ToListAsync();
        return View(new UserCreateViewModel {
            Password = "",
            Email = "",
            AvailableRoles = roles,
            SelectedRole = "User"
        });
    }

    // CREATE (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserCreateViewModel userCreateView)
    {
        if (!ModelState.IsValid)
        {
            userCreateView.AvailableRoles = await _roleManager.Roles.Select(role => role.Name!).ToListAsync();
            return View(userCreateView);
        }
        

        var user = new ApplicationUser
        {
            UserName = userCreateView.Email,
            Email = userCreateView.Email,
            EmailConfirmed = userCreateView.EmailConfirmed,
            Initials = userCreateView.Initials,                  // jeśli NOT NULL, to wymagaj w VM
            EnableNotifications = userCreateView.EnableNotifications
        };

        var resultCreateUser = await _userManager.CreateAsync(user, userCreateView.Password);
        if (!resultCreateUser.Succeeded)
        {
            foreach (var error in resultCreateUser.Errors) {
                ModelState.AddModelError("", error.Description);
                TempData["Error"] = error.Description;
            }
            userCreateView.AvailableRoles = await _roleManager.Roles.Select(role => role.Name!).ToListAsync();
            return View(userCreateView);
        }

        // role
        var resultAddRole = await _userManager.SetSingleRoleAsync(user, userCreateView.SelectedRole);
        if (!resultAddRole.Succeeded) {
            throw new DataException("Error during adding role to the user");
        }
            

        return RedirectToAction(nameof(Index));
    }

    // EDIT (GET)
    public async Task<IActionResult> Edit(string id)
    {
        if (IsCurrentlyLoggedInUser(id)) {
            TempData["Error"] = "You cannot edit the currently logged in user.";
            return RedirectToAction(nameof(Index));
        }
        
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var rolesAll = await _roleManager.Roles.Select(role => role.Name!).ToListAsync();
        var userRoles = await _userManager.GetRolesAsync(user);

        return View(new UserEditViewModel
        {
            Id = user.Id,
            Email = user.Email!,
            EmailConfirmed = user.EmailConfirmed,
            Initials = user.Initials,
            EnableNotifications = user.EnableNotifications,
            AvailableRoles = rolesAll,
            SelectedRole = userRoles.SingleOrDefault() ?? "User",
            LockoutEnd = user.LockoutEnd
        });
    }

    // EDIT (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserEditViewModel userEditView)
    {
        if (IsCurrentlyLoggedInUser(userEditView.Id)) {
            TempData["Error"] = "You cannot edit the currently logged in user.";
            return RedirectToAction(nameof(Index));
        }
        
        if (!ModelState.IsValid)
        {
            userEditView.AvailableRoles = await _roleManager.Roles.Select(role => role.Name!).ToListAsync();
            return View(userEditView);
        }

        var user = await _userManager.FindByIdAsync(userEditView.Id);
        if (user == null) 
            return NotFound();

        if (userEditView.ResetLockOut) {
            await _userManager.SetLockoutEndDateAsync(user, null);
        }
        // rozpatrzenie LockoutMinutes z edit tylko jeśli nie zresetowano lockout
        else if (userEditView.LockoutMinutes.HasValue && userEditView.LockoutMinutes > 0) {
            var lockoutUntil = DateTimeOffset.UtcNow.AddMinutes(userEditView.LockoutMinutes.Value);

            await _userManager.SetLockoutEndDateAsync(user, lockoutUntil);
        }


        user.Email = userEditView.Email;
        user.UserName = userEditView.Email;
        user.EmailConfirmed = userEditView.EmailConfirmed;
        user.Initials = userEditView.Initials;
        user.EnableNotifications = userEditView.EnableNotifications;

        var update = await _userManager.UpdateAsync(user);
        if (!update.Succeeded)
        {
            foreach (var error in update.Errors) ModelState.AddModelError("", error.Description);
            userEditView.AvailableRoles = await _roleManager.Roles.Select(role => role.Name!).ToListAsync();
            return View(userEditView);
        }

        var resultEditRole = await _userManager.SetSingleRoleAsync(user, userEditView.SelectedRole);
        if (!resultEditRole.Succeeded)
            throw new DataException("Error during editing role of the user");

        return RedirectToAction(nameof(Index));
    }

    // RESET PASSWORD (GET)
    public async Task<IActionResult> ResetPassword(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var userResetPasswordView = new UserResetPasswordViewModel
        {
            Id = user.Id,
            Email = user.Email ?? user.UserName ?? ""
        };

        return View(userResetPasswordView);
    }

    // RESET PASSWORD (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(UserResetPasswordViewModel userResetPasswordView)
    {
        if (!ModelState.IsValid)
            return View(userResetPasswordView);

        var user = await _userManager.FindByIdAsync(userResetPasswordView.Id);
        if (user == null) return NotFound();

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, userResetPasswordView.NewPassword);

        if (!result.Succeeded)
        {
            foreach (var e in result.Errors)
                ModelState.AddModelError(string.Empty, e.Description);

            return View(userResetPasswordView);
        }

        TempData["Success"] = "Password has been reset.";
        return RedirectToAction(nameof(Edit), new { id = userResetPasswordView.Id });
    }
    
    // Delete (GET)
    public async Task<IActionResult> Delete(string id)
    {
        if (IsCurrentlyLoggedInUser(id)) {
            TempData["Error"] = "You cannot delete the currently logged in user.";
            return RedirectToAction(nameof(Index));
        }
        
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();
        
        var userRole = (await _userManager.GetRolesAsync(user)).SingleOrDefault() ?? "User";

        return View(new UserDeleteViewModel
        {
            Id = user.Id,
            Email = user.Email!,
            EmailConfirmed = user.EmailConfirmed,
            Initials = user.Initials,
            UserRole = userRole
        });
    }

    // DELETE (POST) - opcjonalnie (uważaj)[ActionName("Delete")]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string id)
    {
        if (IsCurrentlyLoggedInUser(id)) {
            TempData["Error"] = "You cannot delete the currently logged in user.";
            return RedirectToAction(nameof(Index));
        }
        
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) 
            return NotFound();

        // ochrona przed usunięciem samego siebie
        if (User.Identity?.Name == user.UserName)
        {
            TempData["Error"] = "You can not delete the currently logged in user";
            return RedirectToAction(nameof(Index));
        }

        TempData["Success"] = "Deleting was successful!";
        await _userManager.DeleteAsync(user);
        return RedirectToAction(nameof(Index));
    }
}
