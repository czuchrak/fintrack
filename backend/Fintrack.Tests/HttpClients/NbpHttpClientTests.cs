using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.HttpClients;
using Fintrack.App.Models;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Fintrack.Tests.HttpClients;

[TestFixture]
public class NbpHttpClientTests : TestBase
{
    private static readonly IEnumerable<RateTable> RatesResult = new List<RateTable>
    {
        new()
        {
            EffectiveDate = DateTime.Parse("2022-01-02"),
            Rates = new List<Rate>
            {
                new() { Code = "EUR", Mid = 5.1234M }
            }
        }
    };

    [Test]
    public async Task GetRates_Returns()
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(RatesResult))
            })
            .Verifiable();

        var nbpHttpClient = new NbpHttpClient(new HttpClient(handlerMock.Object));
        var rates = (await nbpHttpClient
                .GetRates(DateTime.Parse("2022-01-01"), DateTime.Parse("2022-01-08")))
            .ToList();

        Assert.NotNull(rates);
        Assert.AreEqual(1, rates.Count);
        Assert.AreEqual(DateTime.Parse("2022-01-02"), rates[0].EffectiveDate);
        Assert.AreEqual(1, rates[0].Rates.Count());
        Assert.AreEqual("EUR", rates[0].Rates.First().Code);
        Assert.AreEqual(5.1234M, rates[0].Rates.First().Mid);
    }
}