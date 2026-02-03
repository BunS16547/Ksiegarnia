using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ksiegarnia.Data;
using Ksiegarnia.Types;

namespace Ksiegarnia.Models;

public class LoanModel : IHasIntId{
    public int Id { get; set; }
    
    [Required]
    public int BookId { get; set; }
    [ForeignKey("BookId")]
    public BookModel? Book { get; set; }

    [Required] 
    public string UserId { get; set; } = string.Empty;
    [ForeignKey("UserId")]
    public ApplicationUser? User { get; set; }

    [Required]
    public DateTime LoanedAt { get; set; }

    [Required]
    public DateTime DueAt { get; set; }

    public DateTime? ReturnedAt { get; set; }
    
    [NotMapped]
    public bool IsActive => ReturnedAt == null;
}