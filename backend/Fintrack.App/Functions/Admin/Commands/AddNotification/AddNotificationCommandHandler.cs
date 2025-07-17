using Fintrack.Database;
using Fintrack.Database.Entities;
using MediatR;

namespace Fintrack.App.Functions.Admin.Commands.AddNotification;

public class AddNotificationCommandHandler(DatabaseContext context)
    : AdminBaseHandler(context), IRequestHandler<AddNotificationCommand, Unit>
{
    public async Task<Unit> Handle(AddNotificationCommand request, CancellationToken cancellationToken)
    {
        await CheckIsAdmin(request.UserId);
        var model = request.Model;

        Context.Notifications.Add(new Notification
        {
            Type = model.Type,
            Message = model.Message,
            Url = model.Url,
            ValidFrom = model.ValidFrom,
            ValidUntil = model.ValidUntil,
            IsActive = model.IsActive
        });

        await Context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}