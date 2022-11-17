using MediatR;

namespace Fintrack.App.Functions.Admin.Commands.DuplicateNotification;

public class DuplicateNotificationCommand : RequestBase, IRequest
{
    public Guid NotificationId { get; set; }
}