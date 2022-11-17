using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fintrack.Database.Entities;

public class NetWorthGoal : BaseEntity
{
    [Required] [MaxLength(50)] public string UserId { get; set; }

    [Required] [MaxLength(50)] public string Name { get; set; }

    public virtual ICollection<NetWorthGoalPart> GoalParts { get; set; }

    [Required] public DateTime Deadline { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Value { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal ReturnRate { get; set; }

    public virtual User User { get; set; }
}