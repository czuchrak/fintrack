using System.ComponentModel.DataAnnotations;

namespace Fintrack.Database.Entities;

public class Currency
{
    [Key] [MaxLength(3)] public string Code { get; set; }
}