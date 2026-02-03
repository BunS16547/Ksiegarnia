using Ksiegarnia.Models;
using Ksiegarnia.ViewModels.Loans;

namespace Ksiegarnia.Services;

public interface ILoansService : IModelService<LoanModel, LoanViewModel> {
    Task<List<BookModel>> GetAllAvailableBooksAsync();
    Task<LoanEditViewModel?> GetEditViewByIdAsync(int id);
    Task<LoanDeleteViewModel?> GetDeleteViewByIdAsync(int id);
    Task AddFromCreateViewAsync(LoanCreateViewModel loanCreateView);
    Task UpdateFromEditViewAsync(LoanEditViewModel loanEditView);
}