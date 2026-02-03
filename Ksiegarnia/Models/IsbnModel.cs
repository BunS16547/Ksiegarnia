using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ksiegarnia.Types;

namespace Ksiegarnia.Models;

public class IsbnModel: IHasIntId {
    public int Id { get; set; }

    [Required,
    MinLength(13),
    MaxLength(13)] 
    public required string Value { get; set; }
    
    public int? BookId { get; set; }
    [ForeignKey("BookId")]
    public BookModel? Book { get; init; }
}