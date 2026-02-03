using Ksiegarnia.Helpers;
using Ksiegarnia.Models;
using Ksiegarnia.ViewModels.Loans;

namespace Ksiegarnia.ModelMappers.Implementation;

public class LoansMapper : ILoansMapper {
    public LoanViewModel MapToViewModel(LoanModel loan) {
        return new LoanViewModel {
            Id = loan.Id,
            BookId = loan.BookId,
            BookTitle = loan.Book?.Title ?? "",
            UserId = loan.UserId,
            UserEmail = loan.User?.Email ?? "",
            LoanedAt = loan.LoanedAt,
            DueAt = loan.DueAt,
            ReturnedAt = loan.ReturnedAt
        };
    }
    
    public LoanModel MapFromViewModel(LoanViewModel loanView) {
        return ModelsHelper.BuildNewLoanWithValues(loanView.LoanedAt, loanView.DueAt, loanView.ReturnedAt, loanView.BookId, loanView.UserId);
    }
    
    public void ReplacePropertiesFromViewModel(LoanModel loan, LoanViewModel loanView) {
        loan.LoanedAt = loanView.LoanedAt;
        loan.DueAt = loanView.DueAt;
        loan.ReturnedAt = loanView.ReturnedAt;
        loan.BookId = loanView.BookId;
        loan.UserId = loanView.UserId;
    }

    public LoanEditViewModel MapToEditView(LoanModel loan)
    {
        return new LoanEditViewModel
        {
            Id = loan.Id,

            CurrentBookId = loan.BookId,
            CurrentUserId = loan.UserId,

            CurrentBookTitle = loan.Book?.Title ?? "",
            CurrentUserEmail = loan.User?.Email ?? "",

            NewBookId = loan.BookId,
            NewUserId = loan.UserId,

            LoanedAt = loan.LoanedAt,
            DueAt = loan.DueAt,
            ReturnedAt = loan.ReturnedAt,

            HasBeenReturned = false
        };
    }

    public LoanDeleteViewModel MapToDeleteView(LoanModel loan) {
        return new LoanDeleteViewModel {
            Id = loan.Id,
  
            BookTitle = loan.Book?.Title ?? "",
            UserEmail = loan.User?.Email ?? "",

            LoanedAt = loan.LoanedAt,
            DueAt = loan.DueAt,
            ReturnedAt = loan.ReturnedAt,
        };
    }
}