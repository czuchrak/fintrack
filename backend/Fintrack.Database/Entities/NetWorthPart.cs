using System.ComponentModel.DataAnnotations;

namespace Fintrack.Database.Entities;

public class NetWorthPart : BaseEntity
{
    [Required] [MaxLength(50)] public string UserId { get; set; }

    [Required] [MaxLength(50)] public string Name { get; set; }

    [Required] [MaxLength(20)] public string Type { get; set; }

    [Required] [MaxLength(3)] public string Currency { get; set; }

    public bool IsVisible { get; set; }

    public int Order { get; set; }

    public virtual ICollection<NetWorthEntryPart> EntryParts { get; set; }

    public virtual ICollection<NetWorthGoalPart> GoalParts { get; set; }

    public virtual User User { get; set; }
}