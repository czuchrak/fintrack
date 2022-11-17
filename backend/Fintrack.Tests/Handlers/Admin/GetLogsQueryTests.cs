using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Queries.GetLogs;
using Fintrack.Database.Entities;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Fintrack.Tests.Handlers.Admin;

[TestFixture]
public class GetLogsQueryTests : TestBase
{
    [OneTimeSetUp]
    public async Task SetUp()
    {
        await using var context = CreateContext();
        context.Logs.Add(new Log
        {
            TimeStamp = DateTime.Now,
            Level = "Information",
            Message = Guid.NewGuid().ToString(),
            Exception = Guid.NewGuid().ToString()
        });

        await context.SaveChangesAsync();
    }

    [Test]
    public async Task GetLogsQueryHandler_ReturnsAllLogs()
    {
        await using var context = CreateContext();
        var handler = new GetLogsQueryHandler(context);

        var logs = (await handler.Handle(new GetLogsQuery { UserId = UserId }, new CancellationToken())).ToList();

        Assert.AreEqual(1, logs.Count);
        Assert.AreEqual(1, logs[0].Id);
        Assert.AreEqual("Information", logs[0].Level);
        Assert.IsNotEmpty(logs[0].Message);
        Assert.IsNotEmpty(logs[0].Exception);
        Assert.IsNotNull(logs[0].Timestamp);
    }

    [Test]
    public async Task GetLogsQueryHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await using var context = CreateContext();
        var handler = new GetLogsQueryHandler(context);

        Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await handler.Handle(new GetLogsQuery { UserId = "Wrong_id" }, new CancellationToken()));
    }

    [Test]
    public async Task GetLogsQueryValidator_ValidatesUserId()
    {
        var validator = new GetLogsQueryValidator();
        var result = await validator.TestValidateAsync(new GetLogsQuery { UserId = UserId });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Test]
    public async Task GetLogsQueryValidator_ThrowsException_WhenUserIdIsEmpty()
    {
        var validator = new GetLogsQueryValidator();
        var result = await validator.TestValidateAsync(new GetLogsQuery { UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}