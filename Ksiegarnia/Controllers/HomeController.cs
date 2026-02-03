using System.Diagnostics;
using Ksiegarnia.Data;
using Ksiegarnia.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ksiegarnia.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var isAuth = User.Identity?.IsAuthenticated == true;
        var user = isAuth ? await _userManager.GetUserAsync(User) : null;
        
        var indexViewModel = new IndexViewModel
        {
            IsAuthenticated = isAuth,
            UserEmail = user?.Email,
            IsAdmin = User.IsInRole("Admin"),
            IsEditor = User.IsInRole("Editor"),

            BooksCount = await _db.Books.CountAsync(),
            AuthorsCount = await _db.Authors.CountAsync(),
            CategoriesCount = await _db.Categories.CountAsync(),
            IsbnsCount = await _db.Isbns.CountAsync(),
            ActiveLoansCount = await _db.Loans.Where(loan => loan.ReturnedAt == null).CountAsync()
        };

        return View(indexViewModel);
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Help()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}