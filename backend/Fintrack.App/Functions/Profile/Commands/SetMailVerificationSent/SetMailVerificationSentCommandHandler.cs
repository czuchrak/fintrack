using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Profile.Commands.SetMailVerificationSent;

public class SetMailVerificationSentCommandHandler : IRequestHandler<SetMailVerificationSentCommand, Unit>
{
    private readonly DatabaseContext _context;

    public SetMailVerificationSentCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(SetMailVerificationSentCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;

        var user = await _context.Users.SingleAsync(x => x.Id == userId, cancellationToken);

        user.VerificationMailSent = DateTime.Now;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}