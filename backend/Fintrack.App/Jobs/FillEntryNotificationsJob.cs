using Fintrack.App.Functions.Worker.Commands.FillEntryNotifications;
using MediatR;
using Quartz;

namespace Fintrack.App.Jobs;

public class FillEntryNotificationsJob : IJob
{
    private readonly IMediator _mediator;

    public FillEntryNotificationsJob(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _mediator.Send(new FillEntryNotificationsCommand());
    }
}