using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Worker.Commands.FillExchangeRates;
using Fintrack.App.HttpClients;
using Fintrack.App.Models;
using Fintrack.Database.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Fintrack.Tests.Handlers.Worker;

public class FillExchangeRatesCommandHandlerTests : TestBase
{
    private static readonly IEnumerable<RateTable> RatesResult = new List<RateTable>
    {
        new()
        {
            EffectiveDate = DateTime.Now.Date,
            Rates = new List<Rate>
            {
                new() { Code = "EUR", Mid = 5.1234M },
                new() { Code = "USD", Mid = 5.4321M },
                new() { Code = "GBP", Mid = 5.2134M },
                new() { Code = "CHF", Mid = 5.3142M }
            }
        },
        new()
        {
            EffectiveDate = DateTime.Now.AddDays(-1).Date,
            Rates = new List<Rate>
            {
                new() { Code = "EUR", Mid = 5.1234M },
                new() { Code = "USD", Mid = 5.4321M },
                new() { Code = "GBP", Mid = 5.2134M },
                new() { Code = "CHF", Mid = 5.3142M }
            }
        }
    };

    private async Task InitializeAsync()
    {
        await using var context = CreateContext();

        context.ExchangeRates.AddRange(new ExchangeRate
            {
                Currency = "EUR", Date = DateTime.Now.AddDays(-1).Date, Rate = 5.1234M
            },
            new ExchangeRate
            {
                Currency = "USD", Date = DateTime.Now.AddDays(-1).Date, Rate = 5.4321M
            },
            new ExchangeRate
            {
                Currency = "GBP", Date = DateTime.Now.AddDays(-1).Date, Rate = 5.2134M
            },
            new ExchangeRate
            {
                Currency = "CHF", Date = DateTime.Now.AddDays(-1).Date, Rate = 5.3142M
            });

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_FillsExchangeRatesInDbFromNbpApi()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var loggerMock = new Mock<ILogger<FillExchangeRatesCommandHandler>>();
        loggerMock.Setup(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            )
        );

        var nbpHttpClient = new Mock<INbpHttpClient>();
        nbpHttpClient
            .Setup(x => x.GetRates(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns(Task.FromResult(RatesResult));

        var handler = new FillExchangeRatesCommandHandler(context, nbpHttpClient.Object, loggerMock.Object);

        await handler.Handle(new FillExchangeRatesCommand(), CancellationToken.None);

        var rates = await context.ExchangeRates.ToListAsync();
        var todayRates = rates.Where(x => x.Date == DateTime.Now.Date).ToList();

        rates.Should().HaveCount(8);
        todayRates.Should().HaveCount(4);
        todayRates.First(x => x.Currency == "EUR").Rate.Should().Be(5.1234M);
        todayRates.First(x => x.Currency == "EUR").Date.Should().Be(DateTime.Now.Date);
        todayRates.First(x => x.Currency == "USD").Rate.Should().Be(5.4321M);
        todayRates.First(x => x.Currency == "USD").Date.Should().Be(DateTime.Now.Date);
        todayRates.First(x => x.Currency == "GBP").Rate.Should().Be(5.2134M);
        todayRates.First(x => x.Currency == "GBP").Date.Should().Be(DateTime.Now.Date);
        todayRates.First(x => x.Currency == "CHF").Rate.Should().Be(5.3142M);
        todayRates.First(x => x.Currency == "CHF").Date.Should().Be(DateTime.Now.Date);

        loggerMock.Verify(
            m => m.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once
        );
    }
}