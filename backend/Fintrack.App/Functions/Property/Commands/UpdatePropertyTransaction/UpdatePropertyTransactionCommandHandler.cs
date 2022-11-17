using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Property.Commands.UpdatePropertyTransaction;

public class UpdatePropertyTransactionCommandHandler : IRequestHandler<UpdatePropertyTransactionCommand, Unit>
{
    private readonly DatabaseContext _context;

    public UpdatePropertyTransactionCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdatePropertyTransactionCommand request, CancellationToken cancellationToken)
    {
        var model = request.Model;
        var userId = request.UserId;

        var property = await _context.Properties
            .SingleAsync(x => x.Id == model.PropertyId && x.UserId == userId, cancellationToken);

        var propertyTransaction = await _context.PropertyTransactions
            .SingleAsync(x => x.Id == model.Id && x.PropertyId == property.Id, cancellationToken);

        propertyTransaction.CategoryId = model.CategoryId;
        propertyTransaction.Date = model.Date.Date;
        propertyTransaction.Value = model.Value;
        propertyTransaction.Details = model.Details;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}