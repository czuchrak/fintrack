using Fintrack.Database;
using Fintrack.Database.Entities;
using MediatR;

namespace Fintrack.App.Functions.NetWorth.Commands.AddNetWorthGoal;

public class AddNetWorthGoalCommandHandler : IRequestHandler<AddNetWorthGoalCommand, Unit>
{
    private readonly DatabaseContext _context;

    public AddNetWorthGoalCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(AddNetWorthGoalCommand request, CancellationToken cancellationToken)
    {
        var model = request.Model;
        var userId = request.UserId;

        _context.Add(new NetWorthGoal
        {
            UserId = userId,
            Name = model.Name,
            Value = model.Value,
            ReturnRate = model.ReturnRate,
            Deadline = model.Deadline.Date,
            GoalParts = model.Parts
                .Select(x => new NetWorthGoalPart
                {
                    NetWorthPartId = x
                }).ToList()
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}