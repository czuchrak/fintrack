using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.HttpClients;
using Fintrack.App.Models;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace Fintrack.Tests.HttpClients;

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

    [Fact]
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

        rates.Should().NotBeNull();
        rates.Should().HaveCount(1);
        rates[0].EffectiveDate.Should().Be(DateTime.Parse("2022-01-02"));
        rates[0].Rates.Should().HaveCount(1);
        rates[0].Rates.First().Code.Should().Be("EUR");
        rates[0].Rates.First().Mid.Should().Be(5.1234M);
    }
}