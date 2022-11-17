using Fintrack.Database;
using MediatR;

namespace Fintrack.App.Functions.Property.Commands.AddProperty;

public class AddPropertyCommandHandler : IRequestHandler<AddPropertyCommand, Unit>
{
    private readonly DatabaseContext _context;

    public AddPropertyCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddPropertyCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        var model = request.Model;

        var property = new Database.Entities.Property
        {
            Name = model.Name,
            UserId = userId,
            IsActive = model.IsActive
        };

        _context.Add(property);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}