using Fintrack.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Admin.Commands.UpdateNotification;

public class UpdateNotificationCommandHandler : AdminBaseHandler, IRequestHandler<UpdateNotificationCommand, Unit>
{
    public UpdateNotificationCommandHandler(DatabaseContext context) : base(context)
    {
    }

    public async Task<Unit> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
    {
        await CheckIsAdmin(request.UserId);
        var model = request.Model;

        var notification = await Context.Notifications
            .SingleAsync(x => x.Id == model.Id, cancellationToken);

        notification.Type = model.Type;
        notification.Message = model.Message;
        notification.Url = model.Url;
        notification.ValidFrom = model.ValidFrom;
        notification.ValidUntil = model.ValidUntil;
        notification.IsActive = model.IsActive;

        await Context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}