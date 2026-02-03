namespace Ksiegarnia.Types;

public record NavLink(
    string Controller,
    string Action,
    string Text,
    string? SpecialClass = null);

public static class NavLinks {
    public static readonly List<NavLink> NavLinksList = new List<NavLink> {
        new("Home", "Index", "Home"),
        new("Home", "About", "About"),
        
        new("Books", "Index", "Books", "margin-left-small"),
        new("Authors", "Index", "Authors"),
        new("Categories", "Index", "Categories"),
        new("Isbns", "Index", "Isbns"),
    };
}