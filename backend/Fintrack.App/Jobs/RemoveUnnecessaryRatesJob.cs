using Fintrack.App.Functions.Worker.Commands.RemoveUnnecessaryRates;
using MediatR;
using Quartz;

namespace Fintrack.App.Jobs;

public class RemoveUnnecessaryRatesJob : IJob
{
    private readonly IMediator _mediator;

    public RemoveUnnecessaryRatesJob(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _mediator.Send(new RemoveUnnecessaryRatesCommand());
    }
}