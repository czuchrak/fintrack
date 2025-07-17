using Fintrack.Database;
using MediatR;

namespace Fintrack.App.Functions.Profile.Commands.DeleteUser;

public class DeleteUserCommandHandler(DatabaseContext context) : IRequestHandler<DeleteUserCommand, Unit>
{
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;

        context.RemoveRange(context.Users.Where(x => x.Id == userId));
        context.RemoveRange(context.NetWorthParts.Where(x => x.UserId == userId));
        context.RemoveRange(context.Properties.Where(x => x.UserId == userId));

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}