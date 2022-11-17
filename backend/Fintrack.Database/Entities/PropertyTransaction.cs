using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fintrack.Database.Entities;

public class PropertyTransaction : BaseEntity
{
    public Guid PropertyId { get; set; }

    public Guid CategoryId { get; set; }

    public DateTime Date { get; set; }

    [Column(TypeName = "decimal(18,2)")] public decimal Value { get; set; }

    [MaxLength(50)] public string Details { get; set; }

    public virtual Property Property { get; set; }
}