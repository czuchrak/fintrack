using System.ComponentModel.DataAnnotations;

namespace Fintrack.App.Functions.Property.Models;

public class PropertyTransactionModel
{
    public Guid? Id { get; set; }

    public Guid PropertyId { get; set; }

    public Guid CategoryId { get; set; }

    public DateTime Date { get; set; }

    public decimal Value { get; set; }

    [StringLength(50)] public string Details { get; set; }
}