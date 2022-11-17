using System.ComponentModel.DataAnnotations;

namespace Fintrack.Database.Entities;

public class PropertyCategory : BaseEntity
{
    [Required] [MaxLength(50)] public string Name { get; set; }

    [Required] [MaxLength(20)] public string Type { get; set; }

    public bool IsCost { get; set; }
}