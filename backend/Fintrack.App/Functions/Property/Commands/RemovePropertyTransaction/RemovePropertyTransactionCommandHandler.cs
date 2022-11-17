using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Property.Commands.RemovePropertyTransaction;

public class RemovePropertyTransactionCommandHandler : IRequestHandler<RemovePropertyTransactionCommand, Unit>
{
    private readonly DatabaseContext _context;

    public RemovePropertyTransactionCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RemovePropertyTransactionCommand request, CancellationToken cancellationToken)
    {
        var propertyId = request.PropertyId;
        var transactionId = request.TransactionId;
        var userId = request.UserId;

        var property =
            await _context.Properties
                .SingleAsync(x => x.Id == propertyId && x.UserId == userId, cancellationToken);

        var propertyTransaction = await _context.PropertyTransactions
            .SingleAsync(x => x.Id == transactionId && x.PropertyId == property.Id, cancellationToken);

        _context.PropertyTransactions.Remove(propertyTransaction);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}