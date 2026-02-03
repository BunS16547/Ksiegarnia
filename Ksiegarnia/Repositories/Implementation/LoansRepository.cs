using Ksiegarnia.Data;
using Ksiegarnia.Models;
using Microsoft.EntityFrameworkCore;

namespace Ksiegarnia.Repositories.Implementation;

public class LoansRepository : EfModelRepository<LoanModel>, ILoansRepository {
    public LoansRepository(ApplicationDbContext context) : base(context) { }
    
    public override Task<List<LoanModel>> GetAllAsync() {
        return _set
            .Include(loan => loan.Book)
            .Include(loan => loan.User)
            .AsNoTracking()
            .ToListAsync();
    }
    
    public override Task<LoanModel?> GetByIdAsync(int id) {
        return _set
            .Include(loan => loan.Book)
            .Include(loan => loan.User)
            .SingleOrDefaultAsync(loan => loan.Id == id);
    }
    
    public Task<LoanModel?> GetActiveLoanByBookIdAsync(int bookId) {
        return _set
            .Include(loan => loan.Book)
            .Include(loan => loan.User)
            .FirstOrDefaultAsync(loan => loan.BookId == bookId && loan.ReturnedAt == null);
    }

    public Task<LoanModel?> GetActiveLoanByBookIdAndUserIdAsync(int bookId, string userId) {
        return _set
            .Include(loan => loan.Book)
            .Include(loan => loan.User)
            .FirstOrDefaultAsync(loan => loan.BookId == bookId && loan.UserId == userId && loan.ReturnedAt == null);
    }

    public Task<List<LoanModel>> GetLoansByUserIdAsync(string userId) {
        return _set
            .Include(loan => loan.Book)
            .Include(loan => loan.User)
            .AsNoTracking()
            .Where(loan => loan.UserId == userId)
            .ToListAsync();
    }
}