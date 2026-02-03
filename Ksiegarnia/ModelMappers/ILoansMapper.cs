using Ksiegarnia.Models;
using Ksiegarnia.ViewModels.Loans;

namespace Ksiegarnia.ModelMappers;

public interface ILoansMapper : IModelMapper<LoanModel, LoanViewModel> {
    LoanEditViewModel MapToEditView(LoanModel loan);
    LoanDeleteViewModel MapToDeleteView(LoanModel loan);
}