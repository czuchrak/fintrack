using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fintrack.App.Functions;
using Fintrack.App.Functions.Admin.Commands.DeleteLogs;
using FluentValidation;
using MediatR;
using NUnit.Framework;

namespace Fintrack.Tests.Handlers;

[TestFixture]
public class ValidationBehaviorTests : TestBase
{
    [Test]
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
            () => deleteLogsCommandHandler.Handle(deleteLogsCommand, CancellationToken.None),
            CancellationToken.None);

        Assert.Pass();
    }

    [Test]
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

        Assert.ThrowsAsync<ValidationException>(async () => await
            validationBehavior.Handle(deleteLogsCommand,
                () => deleteLogsCommandHandler.Handle(deleteLogsCommand, CancellationToken.None),
                CancellationToken.None)
        );
    }

    [Test]
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
            () => deleteLogsCommandHandler.Handle(deleteLogsCommand, CancellationToken.None),
            CancellationToken.None);

        Assert.Pass();
    }
}