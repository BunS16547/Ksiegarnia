using Ksiegarnia.Enums;
using Ksiegarnia.Models;
using Ksiegarnia.Types;
using Ksiegarnia.ViewModels;
using Ksiegarnia.ViewModels.Books;

namespace Ksiegarnia.Services;

public interface IBooksService : IModelService<BookModel, BookViewModel> {
    Task<bool> HasUniqueTitleAsync(string bookTitle, int? bookId);
    Task<ServiceResult> ProcessFormSubmitAsync(BookViewModel bookView, FormActionsEnum action);
    Task<ServiceResult> RentBookAsync(int bookId, string userId, int days);
    Task<ServiceResult> ReturnBookAsync(int bookId, string userId, bool isAdmin = false);
    Task<DateTime?> GetUnavailableUntilAsync(int bookId);
    Task<RentBookViewModel?> GetRentViewByIdAsync(int bookId);
}