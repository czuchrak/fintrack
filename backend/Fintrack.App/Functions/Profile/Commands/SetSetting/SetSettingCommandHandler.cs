using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Profile.Commands.SetSetting;

public class SetSettingCommandHandler(DatabaseContext context) : IRequestHandler<SetSettingCommand, Unit>
{
    public async Task<Unit> Handle(SetSettingCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;

        var user = await context.Users.SingleAsync(x => x.Id == userId, cancellationToken);

        switch (request.Name)
        {
            case nameof(user.NewMonthEmailEnabled):
                user.NewMonthEmailEnabled = request.Value;
                break;
            case nameof(user.NewsEmailEnabled):
                user.NewsEmailEnabled = request.Value;
                break;
        }

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}