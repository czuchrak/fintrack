using Fintrack.App.Functions.Worker.Queries.KeepApp;
using MediatR;
using Quartz;

namespace Fintrack.App.Jobs;

public class KeepAppJob : IJob
{
    private readonly IMediator _mediator;

    public KeepAppJob(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _mediator.Send(new KeepAppQuery());
    }
}