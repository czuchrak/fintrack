using MediatR;

namespace Fintrack.App.Functions.Worker.Commands.FillExchangeRates;

public class FillExchangeRatesCommand : IRequest<Unit>
{
    public bool FillAll { get; set; } = false;
}