using MediatR;

namespace Fintrack.App.Functions.Profile.Commands.MarkNotificationAsRead;

public class MarkNotificationAsReadCommand : RequestBase, IRequest<Unit>
{
    public Guid NotificationId { get; set; }
}