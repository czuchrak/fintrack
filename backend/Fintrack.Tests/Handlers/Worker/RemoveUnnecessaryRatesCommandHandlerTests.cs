using System;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Worker.Commands.RemoveUnnecessaryRates;
using Fintrack.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Fintrack.Tests.Handlers.Worker;

[TestFixture]
public class RemoveUnnecessaryRatesCommandHandlerTests : TestBase
{
    [OneTimeSetUp]
    public async Task SetUp()
    {
        await using var context = CreateContext();

        context.ExchangeRates.AddRange(new ExchangeRate
            {
                Currency = "EUR", Date = DateTime.Parse("2020-01-01"), Rate = 5.1234M
            },
            new ExchangeRate
            {
                Currency = "EUR", Date = DateTime.Parse("2020-01-15"), Rate = 5.1234M
            },
            new ExchangeRate
            {
                Currency = "EUR", Date = DateTime.Parse("2020-02-01"), Rate = 5.1234M
            });

        await context.SaveChangesAsync();
    }

    [Test]
    public async Task Handle_FillsExchangeRatesInDbFromNbpApi()
    {
        await using var context = CreateContext();
        var loggerMock = new Mock<ILogger<RemoveUnnecessaryRatesCommandHandler>>();
        loggerMock.Setup(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            )
        );
        var handler = new RemoveUnnecessaryRatesCommandHandler(context, loggerMock.Object);

        await handler.Handle(new RemoveUnnecessaryRatesCommand(), new CancellationToken());

        var rates = await context.ExchangeRates.ToListAsync();

        Assert.AreEqual(2, rates.Count);

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