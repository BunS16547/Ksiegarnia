using Ksiegarnia.Data;
using Ksiegarnia.Enums;
using Ksiegarnia.Helpers;
using Ksiegarnia.ModelMappers;
using Ksiegarnia.ModelMappers.Implementation;
using Ksiegarnia.Models;
using Ksiegarnia.Repositories;
using Ksiegarnia.Repositories.Implementation;
using Ksiegarnia.Services;
using Ksiegarnia.Services.Implementation;
using Ksiegarnia.ViewModels.Authors;
using Ksiegarnia.ViewModels.Books;
using Ksiegarnia.ViewModels.Categories;
using Ksiegarnia.ViewModels.Isbns;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// dodanie dependency injection wszystkich:

// --- REPOSITORIES ---
builder.Services.AddScoped<IBooksRepository, BooksRepository>();
builder.Services.AddScoped<IAuthorsRepository, AuthorsRepository>();
builder.Services.AddScoped<IIsbnsRepository, IsbnsRepository>();
builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<ILoansRepository, LoansRepository>();

// --- MAPPERS ---
builder.Services.AddScoped<IModelMapper<BookModel, BookViewModel>, BooksMapper>();
builder.Services.AddScoped<IModelMapper<AuthorModel, AuthorViewModel>, AuthorsMapper>();
builder.Services.AddScoped<IModelMapper<IsbnModel, IsbnViewModel>, IsbnsMapper>();
builder.Services.AddScoped<IModelMapper<CategoryModel, CategoryViewModel>, CategoriesMapper>();
builder.Services.AddScoped<ILoansMapper, LoansMapper>();

// --- SERVICES ---
builder.Services.AddScoped<IBooksService, BooksService>();
builder.Services.AddScoped<IAuthorsService, AuthorsService>();
builder.Services.AddScoped<IIsbnsService, IsbnsService>();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<ILoansService, LoansService>();

// EMAIL SENDER
builder.Services.AddTransient<IEmailSender, ConsoleEmailSenderHelper>();

// w developmencie dołączenie user secrets
if (builder.Environment.IsDevelopment()) {
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddControllersWithViews();

// dodanie UI ze scafolded identity
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
));

// dodanie google
builder.Services.AddAuthentication().AddGoogle(options => {
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    
    options.CorrelationCookie.SameSite = SameSiteMode.Lax;
    options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.None;
});

// Dodanie Identity Usera
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        
        // proste opcje co do hasła:
        // wymaga co najmniej 6 znaków, małej litery i cyfry
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// skonfigurowanie 401 i 403 statusów
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Identity/Account/Login";          // gdy 401
    options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // gdy 403
});

// zamiana ról na policies aby łatwiej dawać authorize:
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"))
    .AddPolicy("AdminOrEditor", policy =>
        policy.RequireRole("Admin", "Editor"))
    .AddPolicy("All", policy =>
        policy.RequireRole("Admin", "Editor", "User"));

var app = builder.Build();

// zaseedowanie pustej tabeli kategorii tymi z CategoriesEnum
using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();

    if (!db.Categories.Any()) {
        var categories = Enum.GetValues<CategoriesEnum>()
            .Select(categoryEnum => new CategoryModel { Name = categoryEnum.ToString() })
            .ToList();

        db.Categories.AddRange(categories);
        await db.SaveChangesAsync();
    }
}

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapStaticAssets();

app.UseAuthentication();
app.UseAuthorization();

// domyślny wzór wykonywania requestów poprzez URL
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}").WithStaticAssets();

app.MapRazorPages();

await using (var scope = app.Services.CreateAsyncScope()) {
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
    
    // seedowanie identity
    await IdentitySeed.SeedRolesAsync(scope.ServiceProvider);
    await IdentitySeed.SeedAdminAsync(app.Services);
}

app.Run();