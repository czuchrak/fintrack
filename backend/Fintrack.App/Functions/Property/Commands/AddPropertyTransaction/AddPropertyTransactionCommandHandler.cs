using Fintrack.Database;
using Fintrack.Database.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Property.Commands.AddPropertyTransaction;

public class AddPropertyTransactionCommandHandler : IRequestHandler<AddPropertyTransactionCommand, Unit>
{
    private readonly DatabaseContext _context;

    public AddPropertyTransactionCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddPropertyTransactionCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        var model = request.Model;

        var property = await _context.Properties
            .SingleAsync(x => x.Id == model.PropertyId && x.UserId == userId, cancellationToken);

        var propertyTransaction = new PropertyTransaction
        {
            CategoryId = model.CategoryId,
            PropertyId = property.Id,
            Date = model.Date,
            Value = model.Value,
            Details = model.Details
        };

        _context.Add(propertyTransaction);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}