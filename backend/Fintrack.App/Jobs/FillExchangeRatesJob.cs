using Fintrack.App.Functions.Worker.Commands.FillExchangeRates;
using MediatR;
using Quartz;

namespace Fintrack.App.Jobs;

public class FillExchangeRatesJob : IJob
{
    private readonly IMediator _mediator;

    public FillExchangeRatesJob(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _mediator.Send(new FillExchangeRatesCommand());
    }
}