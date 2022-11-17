using Fintrack.Database;
using MediatR;

namespace Fintrack.App.Functions.Profile.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly DatabaseContext _context;

    public DeleteUserCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;

        _context.RemoveRange(_context.Users.Where(x => x.Id == userId));
        _context.RemoveRange(_context.NetWorthParts.Where(x => x.UserId == userId));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}