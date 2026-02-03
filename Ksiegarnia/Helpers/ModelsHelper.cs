
using Ksiegarnia.Models;

namespace Ksiegarnia.Helpers;

public static class ModelsHelper {
    
    public static BookModel BuildNewBookWithValues( 
        string title, int? authorId = null, int? categoryId = null) {
        
        return new BookModel {
            Title = title,
            AuthorId = authorId,
            CategoryId = categoryId
        };
    }

    public static IsbnModel BuildNewIsbnWithValues(
        string value, int? bookId = null) {

        return new IsbnModel {
            Value = value,
            BookId = bookId
        };
    }
    
    public static CategoryModel BuildNewCategoryWithValues(
        string name) {
        
        return new CategoryModel {
            Name = name
        };
    }
    
    public static AuthorModel BuildNewAuthorWithValues(
        string name, int age) {
        
        return new AuthorModel {
            Name = name,
            Age = age
        };
    }
    
    public static LoanModel BuildNewLoanWithValues(
        DateTime loanedAt, DateTime dueAt, DateTime? returnedAt, int bookId, string userId) {
        
        return new LoanModel {
            LoanedAt = loanedAt,
            DueAt = dueAt,
            ReturnedAt = returnedAt,
            BookId = bookId,
            UserId = userId
        };
    }
}