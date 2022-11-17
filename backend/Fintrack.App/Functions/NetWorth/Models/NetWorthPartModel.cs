using System.ComponentModel.DataAnnotations;

namespace Fintrack.App.Functions.NetWorth.Models;

public class NetWorthPartModel
{
    public Guid? Id { get; set; }

    [Required] public string Name { get; set; }

    [Required] public string Type { get; set; }

    [Required] public bool IsVisible { get; set; }

    [Required] public int Order { get; set; }

    [Required] public string Currency { get; set; }
}