using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fintrack.Database.Entities;

public class NetWorthEntryPart
{
    [Key] [Column(Order = 0)] public Guid NetWorthEntryId { get; set; }

    [Key] [Column(Order = 1)] public Guid NetWorthPartId { get; set; }

    public virtual NetWorthEntry NetWorthEntry { get; set; }

    public virtual NetWorthPart NetWorthPart { get; set; }

    [Column(TypeName = "decimal(18,2)")] public decimal Value { get; set; }
}