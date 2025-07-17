using System;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Commands.DeleteLogs;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fintrack.Tests.Handlers.Admin;

public class DeleteLogsCommandTests : TestBase
{
    private async Task InitializeAsync()
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

    [Fact]
    public async Task DeleteLogsCommandHandler_RemovesAllLogs()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new DeleteLogsCommandHandler(context);

        await handler.Handle(new DeleteLogsCommand { UserId = UserId }, CancellationToken.None);

        var logs = await context.Logs.ToListAsync();

        logs.Should().HaveCount(0);
    }

    [Fact]
    public async Task DeleteLogsCommandHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new DeleteLogsCommandHandler(context);

        var act = async () =>
            await handler.Handle(new DeleteLogsCommand { UserId = "Wrong_id" }, new CancellationToken());

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task DeleteLogsCommandValidator_ValidatesUserId()
    {
        var validator = new DeleteLogsCommandValidator();
        var result = await validator.TestValidateAsync(new DeleteLogsCommand { UserId = UserId });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task DeleteLogsCommandValidator_ThrowsException_WhenUserIdIsEmpty()
    {
        var validator = new DeleteLogsCommandValidator();
        var result = await validator.TestValidateAsync(new DeleteLogsCommand { UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}