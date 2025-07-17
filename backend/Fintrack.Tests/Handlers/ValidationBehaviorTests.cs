using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions;
using Fintrack.App.Functions.Admin.Commands.DeleteLogs;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Xunit;

namespace Fintrack.Tests.Handlers;

public class ValidationBehaviorTests : TestBase
{
    [Fact]
    public async Task Handle_FinishesWithoutError()
    {
        await using var context = CreateContext();
        var deleteLogsCommand = new DeleteLogsCommand
        {
            UserId = UserId
        };

        var deleteLogsCommandHandler = new DeleteLogsCommandHandler(context);

        var validationBehavior =
            new ValidationBehavior<DeleteLogsCommand, Unit>(
                new List<IValidator<DeleteLogsCommand>>
                {
                    new DeleteLogsCommandValidator()
                }
            );

        await validationBehavior.Handle(deleteLogsCommand,
            ct => deleteLogsCommandHandler.Handle(deleteLogsCommand, ct),
            CancellationToken.None);
    }

    [Fact]
    public async Task Handle_ThrowsException_WhenUserIdIsNull()
    {
        await using var context = CreateContext();
        var deleteLogsCommand = new DeleteLogsCommand
        {
            UserId = null
        };

        var deleteLogsCommandHandler = new DeleteLogsCommandHandler(context);

        var validationBehavior =
            new ValidationBehavior<DeleteLogsCommand, Unit>(
                new List<IValidator<DeleteLogsCommand>>
                {
                    new DeleteLogsCommandValidator()
                }
            );

        var act = async () => await validationBehavior.Handle(deleteLogsCommand,
            ct => deleteLogsCommandHandler.Handle(deleteLogsCommand, ct),
            CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task Handle_FinishesWithoutError_WhenNoValidators()
    {
        await using var context = CreateContext();
        var deleteLogsCommand = new DeleteLogsCommand
        {
            UserId = UserId
        };

        var deleteLogsCommandHandler = new DeleteLogsCommandHandler(context);

        var validationBehavior =
            new ValidationBehavior<DeleteLogsCommand, Unit>(new List<IValidator<DeleteLogsCommand>>());

        await validationBehavior.Handle(deleteLogsCommand,
            ct => deleteLogsCommandHandler.Handle(deleteLogsCommand, ct),
            CancellationToken.None);
    }
}