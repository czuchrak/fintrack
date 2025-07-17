using Fintrack.App.Functions.Worker.Commands.FillEntryNotifications;
using MediatR;
using Quartz;

namespace Fintrack.App.Jobs;

public class FillEntryNotificationsJob(IMediator mediator) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await mediator.Send(new FillEntryNotificationsCommand());
    }
}