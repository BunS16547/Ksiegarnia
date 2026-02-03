using System.ComponentModel.DataAnnotations;
using Ksiegarnia.Types;

namespace Ksiegarnia.Models;

public class AuthorModel : IHasIntId {
    public int Id { get; set; }
    
    [Required,
    MinLength(4),
    MaxLength(64)]
    public required string Name { get; set; }
    
    [Required,
     Range(1, 150)]
    public int Age { get; set; }
    
    public List<BookModel>? Books { get; init; }
}