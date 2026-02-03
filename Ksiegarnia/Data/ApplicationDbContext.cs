using Ksiegarnia.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ksiegarnia.Data;

// obsługa nie tylko użytkowników ale także ról
public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string> {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
    
    // overwrite metody OnModelCreating
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasDefaultSchema("ksiegarnia_schema");
        
        // sprawdzenie unikalności book title dla każdego book
        modelBuilder.Entity<BookModel>()
            .HasIndex(book => book.Title)
            .IsUnique();
        
        // sprawdzenie unikalności isbn value dla każdego isbn
        modelBuilder.Entity<IsbnModel>()
            .HasIndex(isbn => isbn.Value)
            .IsUnique();
        
        // sprawdzenie unikalności category name dla każdego category
        modelBuilder.Entity<CategoryModel>()
            .HasIndex(category => category.Name)
            .IsUnique();
        
        // sprawdzenie unikalności author name dla każdego author
        modelBuilder.Entity<AuthorModel>()
            .HasIndex(author => author.Name)
            .IsUnique();
        
        modelBuilder.Entity<LoanModel>()
            .HasOne(loan => loan.Book)
            .WithMany() 
            .HasForeignKey(loan => loan.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<LoanModel>()
            .HasOne(loan => loan.User)
            .WithMany() 
            .HasForeignKey(loan => loan.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    // tabele używane w projekcie
    public DbSet<BookModel> Books { get; set; }
    
    public DbSet<IsbnModel> Isbns { get; set; }
    
    public DbSet<AuthorModel> Authors { get; set; }
    
    public DbSet<CategoryModel> Categories { get; set; }

    public DbSet<LoanModel> Loans { get; set; } 


}