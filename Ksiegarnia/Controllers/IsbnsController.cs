using Ksiegarnia.Helpers;
using Ksiegarnia.Services;
using Ksiegarnia.ViewModels;
using Ksiegarnia.ViewModels.Isbns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ksiegarnia.Controllers;

[Authorize(Policy = "AdminOrEditor")]
public class IsbnsController : Controller
{
    private readonly IIsbnsService _isbnsService;

    public IsbnsController(IIsbnsService isbnsService)
    {
        _isbnsService = isbnsService;
    }
    
    // GenerateIsbn GET
    [HttpGet]
    public async Task<IActionResult> GenerateIsbn() {
        var randomIsbnValue = IsbnGeneratorHelper.Generate();

        while (!(await _isbnsService.HasUniqueValueAsync(randomIsbnValue, null))) {
            randomIsbnValue = IsbnGeneratorHelper.Generate();
        }

        return Json(randomIsbnValue);
    }

    
    // INDEX GET
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var isbnViews = await _isbnsService.GetAllViewsAsync();
        return View(isbnViews);
    }
    
    // CREATE GET
    public IActionResult Create()
    {
        return View();
    }
    
    // CREATE POST
    [HttpPost]
    public async Task<IActionResult> Create(IsbnViewModel isbnView)
    {
        if (!ModelState.IsValid)
            return View(isbnView);

        isbnView.Value = isbnView.Value.Trim();
        
        bool hasUniqueValue = await _isbnsService.HasUniqueValueAsync(isbnView.Value, null);

        if (!hasUniqueValue) {
            ModelState.AddModelError(
                nameof(isbnView.Value),
                "Value of the isbn has to be unique");
            return View(isbnView);
        }

        await _isbnsService.AddFromViewAsync(isbnView);

        return RedirectToAction(nameof(Index));
    }

    // EDIT GET
    public async Task<IActionResult> Edit(int id)
    {
        var isbnView = await _isbnsService.GetViewByIdAsync(id);

        if (isbnView == null)
            return NotFound();

        return View(isbnView);
    }
    
    // EDIT POST
    [HttpPost]
    public async Task<IActionResult> Edit(int id, IsbnViewModel isbnView)
    {
        if (id != isbnView.Id)
            return BadRequest("Id mismatch for Isbn Edit");

        if (!ModelState.IsValid)
            return View(isbnView);

        isbnView.Value = isbnView.Value.Trim();
        
        bool hasUniqueValue = await _isbnsService.HasUniqueValueAsync(isbnView.Value, id);

        if (!hasUniqueValue) {
            ModelState.AddModelError(
                nameof(isbnView.Value),
                "Value of the isbn has to be unique");
            return View(isbnView);
        }

        await _isbnsService.UpdateFromViewAsync(isbnView, id);

        return RedirectToAction(nameof(Index));
    }

    // DELETE GET
    public async Task<IActionResult> Delete(int id)
    {
        var isbnView = await _isbnsService.GetViewByIdAsync(id);

        if (isbnView == null)
            return NotFound();

        return View(isbnView);
    }

    // DELETE POST
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteIsbn(int id)
    {
        await _isbnsService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}