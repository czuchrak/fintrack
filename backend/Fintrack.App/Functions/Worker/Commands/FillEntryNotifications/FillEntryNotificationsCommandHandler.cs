using Fintrack.Database;
using Fintrack.Database.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fintrack.App.Functions.Worker.Commands.FillEntryNotifications;

public class FillEntryNotificationsCommandHandler : IRequestHandler<FillEntryNotificationsCommand, Unit>
{
    private const string NotificationType = "networthdata";
    private const string Message = "Dodaj aktualne wartości Twojego majątku";
    private const string Url = "/networth/data";

    private readonly DatabaseContext _context;
    private readonly ILogger<FillEntryNotificationsCommandHandler> _logger;

    public FillEntryNotificationsCommandHandler(DatabaseContext context,
        ILogger<FillEntryNotificationsCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(FillEntryNotificationsCommand request, CancellationToken cancellationToken)
    {
        var notifications = await _context.Notifications
            .Where(x => x.Type == NotificationType)
            .ToListAsync(cancellationToken);

        RemoveObsoleteNotifications(notifications);
        AddCurrentNotification(notifications);
        AddFutureNotification(notifications);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private void RemoveObsoleteNotifications(IEnumerable<Notification> notifications)
    {
        var obsoleteNotifications = notifications
            .Where(x => x.ValidUntil < DateTime.Now)
            .ToList();

        if (!obsoleteNotifications.Any()) return;

        _context.RemoveRange(obsoleteNotifications);
        _logger.LogInformation($"{obsoleteNotifications.Count} obsolete entry notifications have been removed");
    }

    private void AddCurrentNotification(IEnumerable<Notification> notifications)
    {
        var currentNotification = notifications
            .SingleOrDefault(x => x.ValidFrom < DateTime.Now && x.ValidUntil > DateTime.Now);

        if (currentNotification != null) return;

        var now = GetDate(DateTime.Now);
        AddNotification(now, now.AddMonths(1));
        _logger.LogInformation("Current new entry notification has been added");
    }

    private void AddFutureNotification(IEnumerable<Notification> notifications)
    {
        var nextNot = notifications.SingleOrDefault(x => x.ValidFrom > DateTime.Now);

        if (nextNot != null) return;

        AddNotification(GetDate(DateTime.Now).AddMonths(1), GetDate(DateTime.Now).AddMonths(2));
        _logger.LogInformation("Future new entry notification has been added");
    }

    private void AddNotification(DateTime from, DateTime until)
    {
        _context.Notifications.Add(new Notification
        {
            Type = NotificationType,
            Message = Message,
            Url = Url,
            ValidFrom = GetDate(from),
            ValidUntil = GetDate(until),
            IsActive = true
        });
    }

    private static DateTime GetDate(DateTime date)
    {
        return DateTime.Parse($"{date.Year}-{date.Month}-1");
    }
}