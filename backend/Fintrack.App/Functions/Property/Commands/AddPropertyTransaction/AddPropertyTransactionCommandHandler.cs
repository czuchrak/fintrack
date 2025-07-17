using Fintrack.Database;
using Fintrack.Database.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Property.Commands.AddPropertyTransaction;

public class AddPropertyTransactionCommandHandler(DatabaseContext context)
    : IRequestHandler<AddPropertyTransactionCommand, Unit>
{
    public async Task<Unit> Handle(AddPropertyTransactionCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        var model = request.Model;

        var property = await context.Properties
            .SingleAsync(x => x.Id == model.PropertyId && x.UserId == userId, cancellationToken);

        var propertyTransaction = new PropertyTransaction
        {
            CategoryId = model.CategoryId,
            PropertyId = property.Id,
            Date = model.Date,
            Value = model.Value,
            Details = model.Details
        };

        context.Add(propertyTransaction);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}