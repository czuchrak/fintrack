using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Property.Commands.UpdateProperty;

public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, Unit>
{
    private readonly DatabaseContext _context;

    public UpdatePropertyCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
    {
        var model = request.Model;
        var userId = request.UserId;

        var property = await _context.Properties
            .SingleAsync(x => x.Id == model.Id && x.UserId == userId, cancellationToken);

        property.Name = model.Name;
        property.IsActive = model.IsActive;

        _context.Update(property);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}