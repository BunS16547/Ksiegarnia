using Ksiegarnia.Data;
using Ksiegarnia.Enums;
using Ksiegarnia.Services;
using Ksiegarnia.ViewModels;
using Ksiegarnia.ViewModels.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ksiegarnia.Controllers;

[Authorize(Policy = "All")]
public class BooksController : Controller {
    private readonly  IBooksService _booksService;
    private readonly ICategoriesService _categoriesService;
    private readonly IAuthorsService _authorsService;
    private readonly UserManager<ApplicationUser> _userManager;

    public BooksController(IBooksService booksService, ICategoriesService categoriesService, UserManager<ApplicationUser> userManager, IAuthorsService authorsService) {
        _booksService = booksService;
        _categoriesService = categoriesService;
        _userManager = userManager;
        _authorsService = authorsService;
    }

    // metoda która wczytuje wszystkie nazwy authorów
    // i wszystkie nazwy kategorii aby móc je wyświetlić na stronie jako dropdown listę
    private async Task LoadCategoriesAndAuthorsAsync() {
        var categoryViews = await _categoriesService.GetAllViewsAsync();
        ViewBag.Categories = categoryViews
            .Select(categoryView => new SelectListItem { Value = categoryView.Name, Text = categoryView.Name })
            .ToList();

        var authorViews = await _authorsService.GetAllViewsAsync();
        ViewBag.Authors = authorViews
            .Select(authorView => new SelectListItem { Value = authorView.Name, Text = authorView.Name })
            .ToList();
    }
    
    // INDEX GET
    [AllowAnonymous]
    public async Task<IActionResult> Index() {
        var bookViews = await _booksService.GetAllViewsAsync();
        
        return View(bookViews);
    }
    
    // DETAILS GET
    [AllowAnonymous]
    public async Task<IActionResult> Details(int id) {
        var bookView = await _booksService.GetViewByIdAsync(id);

        if (bookView == null)
            return NotFound();
        
        var unavailableUntil = await _booksService.GetUnavailableUntilAsync(id);
        bookView.IsAvailableForRent = unavailableUntil == null;
        bookView.UnavailableUntil = unavailableUntil;
        
        return View(bookView);
    }
    
    // RENT (GET)
    public async Task<IActionResult> Rent(int id)
    {
        var rentBookView = await _booksService.GetRentViewByIdAsync(id);
        if (rentBookView == null) 
            return NotFound();

        var unavailableUntil = await _booksService.GetUnavailableUntilAsync(id);
        if (unavailableUntil != null)
        {
            TempData["Error"] = $"This book is unavailable until {rentBookView.UnavailableUntil:yyyy-MM-dd HH:mm}.";
            return RedirectToAction(nameof(Details), new { id });
        }

        return View(rentBookView);
    }

    // RENT (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Rent(RentBookViewModel rentBookView, int days)
    {
        if (!ModelState.IsValid)
            return View(rentBookView);

        var userId = _userManager.GetUserId(User);
        // zwracam 401 jeśli nie zalogowany
        if (string.IsNullOrWhiteSpace(userId))
            return Challenge();

        var result = await _booksService.RentBookAsync(rentBookView.Id, userId, days);

        if (!result.Status)
        {
            TempData["Error"] = result.ErrorMessage ?? "Could not rent the book.";
            return RedirectToAction(nameof(Details), new { id = rentBookView.Id });
        }

        TempData["Success"] = "Book rented successfully.";
        return RedirectToAction(nameof(Details), new { id = rentBookView.Id });
    }

    // RETURN (POST) – zwrot swojej książki
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Return(int id)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId))
            return Challenge();

        var result = await _booksService.ReturnBookAsync(id, userId);

        if (!result.Status)
            TempData["Error"] = result.ErrorMessage ?? "Could not return the book.";
        else
            TempData["Success"] = "Book returned.";

        return RedirectToAction(nameof(Details), new { id });
    }
    
    // CREATE GET
    [Authorize(Policy = "AdminOrEditor")]
    public async Task<IActionResult> Create() {
        await LoadCategoriesAndAuthorsAsync();
        
        return View();
    }
    
    // CREATE POST
    [Authorize(Policy = "AdminOrEditor")]
    [HttpPost]
    public async Task<IActionResult> Create(BookViewModel bookView) {
        await LoadCategoriesAndAuthorsAsync();
        
        if (!ModelState.IsValid)
            return View(bookView);

        var serviceResult = await _booksService.ProcessFormSubmitAsync(bookView, FormActionsEnum.Create);

        if (!serviceResult.Status) {
            ModelState.AddModelError(
                serviceResult.Field ?? "", 
                serviceResult.ErrorMessage ?? "");
            
            return View(bookView);
        }
        
        return RedirectToAction("index");
    }
    
    // EDIT GET
    [Authorize(Policy = "AdminOrEditor")]
    public async Task<IActionResult> Edit(int id) {
        var bookView = await _booksService.GetViewByIdAsync(id);
        await LoadCategoriesAndAuthorsAsync();

        return View(bookView);
    }
    
    // EDIT POST
    [Authorize(Policy = "AdminOrEditor")]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, BookViewModel bookView) {
        await LoadCategoriesAndAuthorsAsync();

        if (id != bookView.Id)
            return BadRequest("Id Mismatch In Book Edit Post");
        
        if (!ModelState.IsValid)
            return View(bookView);

        var serviceResult = await _booksService.ProcessFormSubmitAsync(bookView, FormActionsEnum.Edit);

        if (!serviceResult.Status) {
            ModelState.AddModelError(
                serviceResult.Field ?? "", 
                serviceResult.ErrorMessage ?? "");
            
            return View(bookView);
        }
        
        return RedirectToAction("index");
    }

    // DELETE GET
    [Authorize(Policy = "AdminOrEditor")]
    public async Task<IActionResult> Delete(int id) {
        var bookView = await _booksService.GetViewByIdAsync(id);

        return View(bookView);
    }

    // DELETE POST
    [Authorize(Policy = "AdminOrEditor")]
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteBook(int id) {
        await _booksService.DeleteAsync(id);
        
        return RedirectToAction("Index");
    }
}