using Ksiegarnia.Models;

namespace Ksiegarnia.Repositories;

public interface ILoansRepository : IModelRepository<LoanModel> {
    Task<LoanModel?> GetActiveLoanByBookIdAsync(int bookId);
    Task<LoanModel?> GetActiveLoanByBookIdAndUserIdAsync(int bookId, string userId);
    Task<List<LoanModel>> GetLoansByUserIdAsync(string userId);
}