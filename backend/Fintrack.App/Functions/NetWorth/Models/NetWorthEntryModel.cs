using System.ComponentModel.DataAnnotations;

namespace Fintrack.App.Functions.NetWorth.Models;

public class NetWorthEntryModel
{
    public Guid? Id { get; set; }

    [Required] public string Date { get; set; }

    public IDictionary<string, decimal> PartValues { get; set; }

    public DateTime ExchangeRateDate { get; set; }
}