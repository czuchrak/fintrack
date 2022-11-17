namespace Fintrack.App.Models;

public class ExchangeRateModel
{
    public string Currency { get; set; }

    public DateTime Date { get; set; }

    public decimal Rate { get; set; }
}