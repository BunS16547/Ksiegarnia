namespace Ksiegarnia.ViewModels.Home;

public class IndexViewModel
{
    public bool IsAuthenticated { get; set; }
    public string? UserEmail { get; set; }

    public bool IsAdmin { get; set; }
    public bool IsEditor { get; set; }

    public int BooksCount { get; set; }
    public int AuthorsCount { get; set; }
    public int CategoriesCount { get; set; }
    public int IsbnsCount { get; set; }
    public int ActiveLoansCount { get; set; }

    public bool IsAdminOrEditor => IsAdmin || IsEditor;
}