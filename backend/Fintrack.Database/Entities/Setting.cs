using System.ComponentModel.DataAnnotations;

namespace Fintrack.Database.Entities;

public class Setting
{
    [Key] [MaxLength(50)] public string Name { get; set; }

    public string Value { get; set; }
}