using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ksiegarnia.Types;

namespace Ksiegarnia.Models;

public class BookModel : IHasIntId {
    public int Id { get; set; }
    
    [Required, StringLength(128)]
    public required string Title { get; set; }
    
    // nie wymaga IsbnId gdyż relacja 1 do 1 a Isbn już posiada BookId
    public IsbnModel? Isbn { get; init; }
    
    public int? CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public CategoryModel? Category { get; init; }
    
    public int? AuthorId { get; set; }
    [ForeignKey("AuthorId")]
    public AuthorModel? Author { get; init; }
    
    
    
}