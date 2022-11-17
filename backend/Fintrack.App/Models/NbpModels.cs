using Newtonsoft.Json;

namespace Fintrack.App.Models;

public class RateTable
{
    [JsonProperty(PropertyName = "effectiveDate")]
    public DateTime EffectiveDate { get; set; }

    [JsonProperty(PropertyName = "rates")] public IEnumerable<Rate> Rates { get; set; }
}

public class Rate
{
    [JsonProperty(PropertyName = "code")] public string Code { get; set; }

    [JsonProperty(PropertyName = "mid")] public decimal Mid { get; set; }
}