using Fintrack.App.Models;
using Newtonsoft.Json;

namespace Fintrack.App.HttpClients;

public interface INbpHttpClient
{
    Task<IEnumerable<RateTable>> GetRates(DateTime from, DateTime to);
}

public class NbpHttpClient : INbpHttpClient
{
    private const string NbpDateFormat = "yyyy-MM-dd";
    private const string RatesUrl = "https://api.nbp.pl/api/exchangerates/tables/A/{0}/{1}?format=json";
    private readonly HttpClient _httpClient;

    public NbpHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<RateTable>> GetRates(DateTime from, DateTime to)
    {
        var uri = string.Format(RatesUrl, from.ToString(NbpDateFormat), to.ToString(NbpDateFormat));
        var table = await _httpClient.GetStringAsync(uri);
        return JsonConvert.DeserializeObject<IEnumerable<RateTable>>(table)!;
    }
}