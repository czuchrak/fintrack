using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Property.Commands.RemovePropertyTransaction;

public class RemovePropertyTransactionCommandHandler(DatabaseContext context)
    : IRequestHandler<RemovePropertyTransactionCommand, Unit>
{
    public async Task<Unit> Handle(RemovePropertyTransactionCommand request, CancellationToken cancellationToken)
    {
        var propertyId = request.PropertyId;
        var transactionId = request.TransactionId;
        var userId = request.UserId;

        var property =
            await context.Properties
                .SingleAsync(x => x.Id == propertyId && x.UserId == userId, cancellationToken);

        var propertyTransaction = await context.PropertyTransactions
            .SingleAsync(x => x.Id == transactionId && x.PropertyId == property.Id, cancellationToken);

        context.PropertyTransactions.Remove(propertyTransaction);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}