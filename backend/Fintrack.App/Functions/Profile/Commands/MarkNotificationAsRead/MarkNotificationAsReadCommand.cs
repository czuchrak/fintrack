using MediatR;

namespace Fintrack.App.Functions.Profile.Commands.MarkNotificationAsRead;

public class MarkNotificationAsReadCommand : RequestBase, IRequest
{
    public Guid NotificationId { get; set; }
}