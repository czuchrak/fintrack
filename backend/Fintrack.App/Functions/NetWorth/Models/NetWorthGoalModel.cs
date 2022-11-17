using System.ComponentModel.DataAnnotations;

namespace Fintrack.App.Functions.NetWorth.Models;

public class NetWorthGoalModel
{
    public Guid? Id { get; set; }

    [Required] public IEnumerable<Guid> Parts { get; set; }

    [Required] public DateTime Deadline { get; set; }

    [Required] public decimal Value { get; set; }

    [Required] public decimal ReturnRate { get; set; }

    [Required] public string Name { get; set; }
}