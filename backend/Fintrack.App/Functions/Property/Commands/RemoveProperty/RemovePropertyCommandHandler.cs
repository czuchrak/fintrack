using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Property.Commands.RemoveProperty;

public class RemovePropertyCommandHandler : IRequestHandler<RemovePropertyCommand, Unit>
{
    private readonly DatabaseContext _context;

    public RemovePropertyCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RemovePropertyCommand request, CancellationToken cancellationToken)
    {
        var propertyId = request.PropertyId;
        var userId = request.UserId;

        var property =
            await _context.Properties.SingleAsync(x => x.Id == propertyId && x.UserId == userId, cancellationToken);

        _context.Remove(property);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}