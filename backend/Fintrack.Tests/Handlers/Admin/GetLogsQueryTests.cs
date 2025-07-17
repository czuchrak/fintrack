using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions.Admin.Queries.GetLogs;
using Fintrack.Database.Entities;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace Fintrack.Tests.Handlers.Admin;

public class GetLogsQueryTests : TestBase
{
    private async Task InitializeAsync()
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

    [Fact]
    public async Task GetLogsQueryHandler_ReturnsAllLogs()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new GetLogsQueryHandler(context);

        var logs = (await handler.Handle(new GetLogsQuery { UserId = UserId }, CancellationToken.None)).ToList();

        logs.Should().HaveCount(1);
        logs[0].Id.Should().Be(1);
        logs[0].Level.Should().Be("Information");
        logs[0].Message.Should().NotBeEmpty();
        logs[0].Exception.Should().NotBeEmpty();
        logs[0].Timestamp.Should().NotBe(default);
    }

    [Fact]
    public async Task GetLogsQueryHandler_ThrowsException_WhenUserIsNotAdmin()
    {
        await InitializeAsync();
        await using var context = CreateContext();
        var handler = new GetLogsQueryHandler(context);

        var act = async () => await handler.Handle(new GetLogsQuery { UserId = "Wrong_id" }, new CancellationToken());

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetLogsQueryValidator_ValidatesUserId()
    {
        var validator = new GetLogsQueryValidator();
        var result = await validator.TestValidateAsync(new GetLogsQuery { UserId = UserId });
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public async Task GetLogsQueryValidator_ThrowsException_WhenUserIdIsEmpty()
    {
        var validator = new GetLogsQueryValidator();
        var result = await validator.TestValidateAsync(new GetLogsQuery { UserId = null });
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }
}