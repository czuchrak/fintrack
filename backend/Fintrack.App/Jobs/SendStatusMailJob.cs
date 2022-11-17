using Fintrack.App.Functions.Worker.Commands.SendStatusMail;
using MediatR;
using Quartz;

namespace Fintrack.App.Jobs;

public class SendStatusMailJob : IJob
{
    private readonly IMediator _mediator;

    public SendStatusMailJob(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _mediator.Send(new SendStatusMailCommand());
    }
}