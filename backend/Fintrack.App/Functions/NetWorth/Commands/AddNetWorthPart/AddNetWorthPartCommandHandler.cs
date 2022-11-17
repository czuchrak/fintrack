using Fintrack.Database;
using Fintrack.Database.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.NetWorth.Commands.AddNetWorthPart;

public class AddNetWorthPartCommandHandler : IRequestHandler<AddNetWorthPartCommand, Unit>
{
    private readonly DatabaseContext _context;

    public AddNetWorthPartCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddNetWorthPartCommand request, CancellationToken cancellationToken)
    {
        var model = request.Model;
        var userId = request.UserId;

        var parts = await _context.NetWorthParts
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);

        var order = parts.Any() ? parts.Max(x => x.Order) + 1 : 1;

        _context.Add(new NetWorthPart
        {
            IsVisible = model.IsVisible,
            Name = model.Name,
            Order = order,
            Type = model.Type,
            UserId = userId,
            Currency = model.Currency
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}