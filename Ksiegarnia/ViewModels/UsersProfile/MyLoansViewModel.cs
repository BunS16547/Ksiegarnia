using Ksiegarnia.ViewModels.Loans;

namespace Ksiegarnia.ViewModels.UsersProfile;

public class MyLoansViewModel {
    public List<LoanViewModel> ActiveLoans { get; set; } = new List<LoanViewModel>();
    public List<LoanViewModel> HistoryLoans { get; set; } = new List<LoanViewModel>();
}