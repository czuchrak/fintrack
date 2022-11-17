using Fintrack.App.Models;

namespace Fintrack.App.Functions.NetWorth.Models;

public class NetWorthModel
{
    public IEnumerable<NetWorthPartModel> Parts { get; set; }
    public IEnumerable<NetWorthEntryModel> Entries { get; set; }
    public IEnumerable<NetWorthGoalModel> Goals { get; set; }
    public IEnumerable<ExchangeRateModel> Rates { get; set; }
}