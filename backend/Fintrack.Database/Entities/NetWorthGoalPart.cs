using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fintrack.Database.Entities;

public class NetWorthGoalPart
{
    [Key] [Column(Order = 0)] public Guid NetWorthGoalId { get; set; }

    [Key] [Column(Order = 1)] public Guid NetWorthPartId { get; set; }

    public virtual NetWorthGoal NetWorthGoal { get; set; }

    public virtual NetWorthPart NetWorthPart { get; set; }
}