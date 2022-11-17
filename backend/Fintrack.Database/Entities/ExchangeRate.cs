using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fintrack.Database.Entities;

public class ExchangeRate
{
    [Required] [Key] [Column(Order = 0)] public DateTime Date { get; set; }

    [Required]
    [MaxLength(3)]
    [Key]
    [Column(Order = 1)]
    public string Currency { get; set; }

    [Required]
    [Column(TypeName = "decimal(8,6)")]
    public decimal Rate { get; set; }
}