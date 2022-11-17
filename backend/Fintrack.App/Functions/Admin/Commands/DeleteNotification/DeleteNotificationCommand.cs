using MediatR;

namespace Fintrack.App.Functions.Admin.Commands.DeleteNotification;

public class DeleteNotificationCommand : RequestBase, IRequest
{
    public Guid NotificationId { get; set; }
}