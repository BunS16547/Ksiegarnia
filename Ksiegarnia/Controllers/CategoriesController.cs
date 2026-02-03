using Ksiegarnia.Services;
using Ksiegarnia.ViewModels;
using Ksiegarnia.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ksiegarnia.Controllers;

[Authorize(Policy="AdminOrEditor")]
public class CategoriesController : Controller
{
    private readonly ICategoriesService _categoriesService;

    public CategoriesController(ICategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }

    // INDEX GET
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var categoryViews = await _categoriesService.GetAllViewsAsync();
        return View(categoryViews);
    }

    // CREATE GET
    public IActionResult Create()
    {
        return View();
    }

    // CREATE POST
    [HttpPost]
    public async Task<IActionResult> Create(CategoryViewModel categoryView)
    {
        if (!ModelState.IsValid)
            return View(categoryView);

        categoryView.Name = categoryView.Name.Trim();
        
        bool hasUniqueName = await _categoriesService.HasUniqueNameAsync(categoryView.Name, null);

        if (!hasUniqueName) {
            ModelState.AddModelError(
                nameof(categoryView.Name),
                "Name of the category has to be unique");
            return View(categoryView);
        }

        await _categoriesService.AddFromViewAsync(categoryView);

        return RedirectToAction(nameof(Index));
    }

    // EDIT GET
    public async Task<IActionResult> Edit(int id)
    {
        var categoryView = await _categoriesService.GetViewByIdAsync(id);

        if (categoryView == null)
            return NotFound();

        return View(categoryView);
    }

    // EDIT POST
    [HttpPost]
    public async Task<IActionResult> Edit(int id, CategoryViewModel categoryView)
    {
        if (id != categoryView.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(categoryView);

        categoryView.Name = categoryView.Name.Trim();
        
        bool hasUniqueName = await _categoriesService.HasUniqueNameAsync(categoryView.Name, categoryView.Id);

        if (!hasUniqueName) {
            ModelState.AddModelError(
                nameof(categoryView.Name),
                "Name of the category has to be unique");
            return View(categoryView);
        }

        await _categoriesService.UpdateFromViewAsync(categoryView, id);
        TempData["Success"] = "Category succesfully updated";

        return RedirectToAction(nameof(Index));
    }

    // DELETE GET
    public async Task<IActionResult> Delete(int id)
    {
        var categoryView = await _categoriesService.GetViewByIdAsync(id);

        if (categoryView == null)
            return NotFound();

        return View(categoryView);
    }

    // DELETE POST
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await _categoriesService.DeleteAsync(id);
        TempData["Success"] = "Category succesfully deleted";
        
        return RedirectToAction(nameof(Index));
    }
}
