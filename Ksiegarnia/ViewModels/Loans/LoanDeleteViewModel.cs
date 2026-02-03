namespace Ksiegarnia.ViewModels.Loans;

public class LoanDeleteViewModel
{
    public int Id { get; set; }

    public string UserEmail { get; set; } = "";
    public string BookTitle { get; set; } = "";

    public DateTime LoanedAt { get; set; }
    public DateTime DueAt { get; set; }
    public DateTime? ReturnedAt { get; set; }

    public bool IsActive => ReturnedAt == null;
}