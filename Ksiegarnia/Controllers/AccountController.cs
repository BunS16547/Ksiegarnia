using Ksiegarnia.Data;
using Ksiegarnia.ModelMappers;
using Ksiegarnia.Models;
using Ksiegarnia.Repositories;
using Ksiegarnia.ViewModels.Loans;
using Ksiegarnia.ViewModels.UsersProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ksiegarnia.Controllers;


[Authorize(Policy = "All")] // każdy zalogowany
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILoansRepository _loansRepository;
    private readonly ILoansMapper _loanMapper;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        ILoansRepository loansRepository, 
        ILoansMapper loanMapper) {
        _userManager = userManager;
        _loansRepository = loansRepository;
        _loanMapper = loanMapper;
    }
    
    // PROFILE (GET)
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge(); // jeśli sesja nieaktualna

        var role = (await _userManager.GetRolesAsync(user)).SingleOrDefault() ?? "";

        var vm = new UserProfileViewModel
        {
            Email = user.Email ?? user.UserName ?? "",
            Initials = user.Initials,
            EnableNotifications = user.EnableNotifications,
            UserRole = role
        };

        return View(vm);
    }

    // EDIT PERSONAL INFO (GET)
    public async Task<IActionResult> Edit()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var vm = new UserProfileEditViewModel
        {
            Email = user.Email ?? "",
            Initials = user.Initials,
            EnableNotifications = user.EnableNotifications
        };

        return View(vm);
    }

    // EDIT PERSONAL INFO (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserProfileEditViewModel userProfileEditView) {
        if (!ModelState.IsValid)
            return View(userProfileEditView);

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var currentEmail = user.Email ?? "";
        if (!string.Equals(currentEmail, userProfileEditView.Email, StringComparison.OrdinalIgnoreCase))
        {
            var setEmail = await _userManager.SetEmailAsync(user, userProfileEditView.Email);
            if (!setEmail.Succeeded)
            {
                foreach (var e in setEmail.Errors)
                    ModelState.AddModelError(string.Empty, e.Description);
                return View(userProfileEditView);
            }

            var setUserName = await _userManager.SetUserNameAsync(user, userProfileEditView.Email);
            if (!setUserName.Succeeded)
            {
                foreach (var e in setUserName.Errors)
                    ModelState.AddModelError(string.Empty, e.Description);
                return View(userProfileEditView);
            }
        }

        user.Initials = userProfileEditView.Initials;
        user.EnableNotifications = userProfileEditView.EnableNotifications;

        var update = await _userManager.UpdateAsync(user);
        if (!update.Succeeded)
        {
            foreach (var e in update.Errors)
                ModelState.AddModelError(string.Empty, e.Description);
            return View(userProfileEditView);
        }

        TempData["Success"] = "Personal information updated.";
        return RedirectToAction(nameof(Index));
    }
    
    // MY LOANS (GET)
    public async Task<IActionResult> MyLoans()
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId))
            return Challenge();

        var foundLoans = await _loansRepository.GetLoansByUserIdAsync(userId);

        var activeLoans = foundLoans
            .Where(loan => loan.ReturnedAt == null)
            .OrderBy(loan => loan.DueAt)
            .Select(_loanMapper.MapToViewModel)
            .ToList();

        var historyLoans = foundLoans
            .Where(loan => loan.ReturnedAt != null)
            .OrderByDescending(loan => loan.ReturnedAt)
            .Select(_loanMapper.MapToViewModel)
            .ToList();

        var myLoansView = new MyLoansViewModel
        {
            ActiveLoans = activeLoans,
            HistoryLoans = historyLoans
        };

        return View(myLoansView);
    }

    // RESET PASSWORD (GET)
    public IActionResult ResetPassword() {
        
        return View(new UserProfileResetPasswordViewModel());
    }

    // RESET PASSWORD (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(UserProfileResetPasswordViewModel userProfileResetPasswordView) {
        if (!ModelState.IsValid)
            return View(userProfileResetPasswordView);

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, userProfileResetPasswordView.NewPassword);

        if (!result.Succeeded)
        {
            foreach (var e in result.Errors)
                ModelState.AddModelError(string.Empty, e.Description);
            return View(userProfileResetPasswordView);
        }

        TempData["Success"] = "Password has been reset.";
        return RedirectToAction(nameof(Index)); 
    }
}
