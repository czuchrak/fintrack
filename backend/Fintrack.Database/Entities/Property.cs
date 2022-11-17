using System.ComponentModel.DataAnnotations;

namespace Fintrack.Database.Entities;

public class Property : BaseEntity
{
    [Required] [MaxLength(50)] public string UserId { get; set; }

    [Required] [MaxLength(50)] public string Name { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<PropertyTransaction> Transactions { get; set; }

    public virtual User User { get; set; }
}