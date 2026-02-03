using Ksiegarnia.Services;
using Ksiegarnia.ViewModels;
using Ksiegarnia.ViewModels.Authors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ksiegarnia.Controllers;

[Authorize(Policy = "AdminOrEditor")]
public class AuthorsController : Controller
{
    private readonly IAuthorsService _authorsService;

    public AuthorsController(IAuthorsService authorsService)
    {
        _authorsService = authorsService;
    }

    // INDEX GET
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var authorViews = await _authorsService.GetAllViewsAsync();
        return View(authorViews);
    }
    
    // CREATE GET
    public IActionResult Create()
    {
        return View();
    }
    
    // CREATE POST
    [HttpPost]
    public async Task<IActionResult> Create(AuthorViewModel authorView)
    {
        if (!ModelState.IsValid)
            return View(authorView);
        
        authorView.Name = authorView.Name.Trim();
        bool hasUniqueName = await _authorsService.HasUniqueNameAsync(authorView.Name, null);
        
        if (!hasUniqueName) {
            ModelState.AddModelError(
                nameof(authorView.Name),
                "Name of the author has to be unique");
            return View(authorView);
        }

        await _authorsService.AddFromViewAsync(authorView);

        return RedirectToAction(nameof(Index));
    }
    
    // EDIT GET
    public async Task<IActionResult> Edit(int id)
    {
        var authorView = await _authorsService.GetViewByIdAsync(id);

        if (authorView == null)
            return NotFound();

        return View(authorView);
    }
    
    // EDIT POST
    [HttpPost]
    public async Task<IActionResult> Edit(int id, AuthorViewModel authorView)
    {
        if (id != authorView.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(authorView);

        authorView.Name = authorView.Name.Trim();
        bool hasUniqueName = await _authorsService.HasUniqueNameAsync(authorView.Name, authorView.Id);
        
        if (!hasUniqueName) {
            ModelState.AddModelError(
                nameof(authorView.Name),
                "Name of the author has to be unique");
            return View(authorView);
        }

        await _authorsService.UpdateFromViewAsync(authorView, id);

        return RedirectToAction(nameof(Index));
    }
    
    // DELETE GET
    public async Task<IActionResult> Delete(int id)
    {
        var authorView = await _authorsService.GetViewByIdAsync(id);

        if (authorView == null)
            return NotFound();

        return View(authorView);
    }
    
    // DELETE POST
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        await _authorsService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
