namespace Ksiegarnia.ViewModels.Loans;

public class LoanViewModel {
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;

    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;

    public DateTime LoanedAt { get; set; }
    public DateTime DueAt { get; set; }
    public DateTime? ReturnedAt { get; set; }
    
    public bool IsActive => ReturnedAt == null;
    public bool IsOverdue => IsActive && DueAt <= DateTime.Now;
    
}