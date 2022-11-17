using System;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Commands.DeleteLogs;
using Fintrack.Database.Entities;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Fintrack.Tests.Handlers.Admin;

[TestFixture]
public class DeleteLogsCommandTests : TestBase
{
    [OneTimeSetUp]
    public async Task SetUp()
    {
        await using var context = CreateContext();
        context.Logs.Add(new Log
        {
            TimeStamp = DateTime.Now,
            Message = Guid.NewGuid().ToString(),
            Exception = Guid.NewGuid().ToString()
        });

        await context.SaveChangesAsync();
    }

    [Test]
    public async Task DeleteLogsCommandHandler_RemovesAllLogs()
    {
        await using var context = CreateContext();
        var handler = new DeleteLogsCommandHandler(context);

        await handler.Handle(new DeleteLogsCommand { UserId = UserId }, new CancellationToken());

        var logs = await context.Logs.ToListAsync();

        Assert.AreEqual(0, logs.Count);
    }

    [Test]
    public async Task DeleteLogsCommandHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await using var context = CreateContext();
        var handler = new DeleteLogsCommandHandler(context);

        Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            await handler.Handle(new DeleteLogsCommand { UserId = "Wrong_id" }, new CancellationToken()));
    }

    [Test]
    public async Task DeleteLogsCommandValidator_ValidatesUserId()
    {
        var validator = new DeleteLogsCommandValidator();
        var result = await validator.TestValidateAsync(new DeleteLogsCommand { UserId = UserId });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Test]
    public async Task DeleteLogsCommandValidator_ThrowsException_WhenUserIdIsEmpty()
    {
        var validator = new DeleteLogsCommandValidator();
        var result = await validator.TestValidateAsync(new DeleteLogsCommand { UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}