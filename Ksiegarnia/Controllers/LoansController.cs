using Ksiegarnia.Data;
using Ksiegarnia.Services;
using Ksiegarnia.ViewModels.Loans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ksiegarnia.Controllers;

[Authorize(Policy = "AdminOrEditor")]
public class LoansController : Controller
{
    private readonly ILoansService _loansService;
    private readonly ApplicationDbContext _db;

    public LoansController(
        ILoansService loansService,
        ApplicationDbContext db)
    {
        _loansService = loansService;
        _db = db;
    }
    
    // dwie helper metody do budowania list książek i użytkowników do pokazania przy tworzeniu / edytowania wypożyczenia (loan)
    private async Task<List<SelectListItem>> BuildAvailableBooksSelectAsync(int? selectedBookId)
    {
        var availableBooks = await _loansService.GetAllAvailableBooksAsync();

        return availableBooks
            .Select(book => new SelectListItem
            {
                Value = book.Id.ToString(),
                Text = book.Title,
                Selected = selectedBookId.HasValue && book.Id == selectedBookId.Value
            })
            .ToList();
    }

    private async Task<List<SelectListItem>> BuildUsersSelectAsync(string? selectedUserId)
    {
        var users = await _db.Users
            .OrderBy(user => user.Email)
            .Select(user => new { user.Id, user.Email })
            .ToListAsync();

        return users
            .Select(user => new SelectListItem
            {
                Value = user.Id,
                Text = user.Email,
                Selected = !string.IsNullOrWhiteSpace(selectedUserId) && user.Id == selectedUserId
            })
            .ToList();
    }


    // INDEX (GET)
    public async Task<IActionResult> Index()
    {
        var loanViews = await _loansService.GetAllViewsAsync();
        return View(loanViews);
    }

    // CREATE (GET)
    public async Task<IActionResult> Create()
    {
        var loanCreateView = new LoanCreateViewModel {
            AvailableBooks = await BuildAvailableBooksSelectAsync(null),
            AvailableUsers = await BuildUsersSelectAsync(null),
            Days = 14
        };

        return View(loanCreateView);
    }

    // CREATE (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LoanCreateViewModel loanCreateView)
    {
        if (!ModelState.IsValid) {
            // odtwórz listy selectów
            loanCreateView.AvailableBooks = await BuildAvailableBooksSelectAsync(loanCreateView.BookId);
            loanCreateView.AvailableUsers = await BuildUsersSelectAsync(loanCreateView.UserId);
            return View(loanCreateView);
        }

        await _loansService.AddFromCreateViewAsync(loanCreateView);

        TempData["Success"] = "Loan created.";
        return RedirectToAction(nameof(Index));
    }

    // EDIT (GET)
    public async Task<IActionResult> Edit(int id)
    {
        var loanEditView = await _loansService.GetEditViewByIdAsync(id);
        if (loanEditView == null) 
            return NotFound();
        

        loanEditView.AvailableBooks = await BuildAvailableBooksSelectAsync(loanEditView.NewBookId);
        loanEditView.AvailableUsers = await BuildUsersSelectAsync(loanEditView.NewUserId);

        return View(loanEditView);
    }

    // EDIT (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(LoanEditViewModel loanEditView)
    {
        if (!ModelState.IsValid)
        {
            loanEditView.AvailableBooks = await BuildAvailableBooksSelectAsync(loanEditView.NewBookId);
            loanEditView.AvailableUsers = await BuildUsersSelectAsync(loanEditView.NewUserId);
            return View(loanEditView);
        }

        await _loansService.UpdateFromEditViewAsync(loanEditView);
        
        TempData["Success"] = "Loan updated.";
        return RedirectToAction(nameof(Index));
    }

    // DELETE (GET)
    public async Task<IActionResult> Delete(int id)
    {
        var loanDeleteView = await _loansService.GetDeleteViewByIdAsync(id);
        if (loanDeleteView == null)
            return NotFound();

        return View(loanDeleteView);
    }

    // DELETE (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(LoanDeleteViewModel loanDeleteView)
    {
        await _loansService.DeleteAsync(loanDeleteView.Id);
        
        TempData["Success"] = "Loan deleted.";
        return RedirectToAction(nameof(Index));
    }
}
