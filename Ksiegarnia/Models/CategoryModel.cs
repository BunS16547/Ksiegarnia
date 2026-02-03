using System.ComponentModel.DataAnnotations;
using Ksiegarnia.Types;

namespace Ksiegarnia.Models;

public class CategoryModel : IHasIntId {
    public int Id { get; set; }

    [Required,
    StringLength(64)]
    public required string Name { get; set; } = string.Empty;
    
    public List<BookModel>? Books { get; init; }
}