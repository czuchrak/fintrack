using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.NetWorth.Commands.UpdateNetWorthPart;

public class UpdateNetWorthPartCommandHandler : IRequestHandler<UpdateNetWorthPartCommand, Unit>
{
    private readonly DatabaseContext _context;

    public UpdateNetWorthPartCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateNetWorthPartCommand request, CancellationToken cancellationToken)
    {
        var model = request.Model;
        var userId = request.UserId;

        var netWorthPart = await _context.NetWorthParts
            .SingleAsync(x => x.Id == model.Id && x.UserId == userId, cancellationToken);

        netWorthPart.Name = model.Name;
        netWorthPart.Type = model.Type;
        netWorthPart.IsVisible = model.IsVisible;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}