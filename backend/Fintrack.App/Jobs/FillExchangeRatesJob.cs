using Fintrack.App.Functions.Worker.Commands.FillExchangeRates;
using MediatR;
using Quartz;

namespace Fintrack.App.Jobs;

public class FillExchangeRatesJob(IMediator mediator) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await mediator.Send(new FillExchangeRatesCommand());
    }
}