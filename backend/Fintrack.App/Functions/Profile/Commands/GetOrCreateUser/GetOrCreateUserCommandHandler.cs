using Fintrack.App.Functions.Profile.Models;
using Fintrack.App.Functions.Property.Models;
using Fintrack.Database;
using Fintrack.Database.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.App.Functions.Profile.Commands.GetOrCreateUser;

public class GetOrCreateUserCommandHandler : IRequestHandler<GetOrCreateUserCommand, UserModel>
{
    private const string NetWorthDataType = "networthdata";

    private readonly DatabaseContext _context;

    public GetOrCreateUserCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<UserModel> Handle(GetOrCreateUserCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId!;

        var user = await GetOrCreateUser(userId, request.Email, cancellationToken);

        return new UserModel
        {
            IsAdmin = await IsAdmin(userId, cancellationToken),
            Currencies = await GetCurrencies(cancellationToken),
            Notifications = await GetNotifications(userId, cancellationToken),
            Properties = GetProperties(user),
            MailVerificationSent = user.VerificationMailSent?.AddMinutes(5) > DateTime.Now,
            Currency = user.Currency,
            UserSettings = new UserSettingsModel
            {
                NewMonthEmailEnabled = user.NewMonthEmailEnabled,
                NewsEmailEnabled = user.NewsEmailEnabled
            }
        };
    }

    private async Task<User> GetOrCreateUser(string userId, string email,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(x => x.Properties)
            .SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (user == null)
        {
            await AddNewUser(userId, email);
            user = await _context.Users
                .Include(x => x.Properties)
                .SingleAsync(x => x.Id == userId, cancellationToken);
        }
        else
        {
            user.LastActivity = DateTime.Now;
            await _context.SaveChangesAsync(cancellationToken);
        }

        return user;
    }

    private async Task AddNewUser(string userId, string email)
    {
        try
        {
            _context.Users.Add(new User
            {
                Id = userId,
                Email = email,
                LastActivity = DateTime.Now,
                CreationDate = DateTime.Now,
                NewMonthEmailEnabled = true,
                NewsEmailEnabled = true,
                Currency = "PLN"
            });

            await _context.SaveChangesAsync();
        }
        catch
        {
            // ignored
        }
    }

    private static IEnumerable<PropertyModel> GetProperties(User user)
    {
        return user.Properties
            .Select(x => new PropertyModel
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive
            })
            .OrderBy(x => x.Name);
    }

    private async Task<bool> IsAdmin(string userId, CancellationToken cancellationToken)
    {
        return (await _context.Settings
                .SingleAsync(x => x.Name == "AdminId", cancellationToken))
            .Value == userId;
    }

    private async Task<IEnumerable<string>> GetCurrencies(CancellationToken cancellationToken)
    {
        return await _context.Currencies.Select(x => x.Code).ToListAsync(cancellationToken);
    }

    private async Task<IEnumerable<UserNotificationModel>> GetNotifications(string userId,
        CancellationToken cancellationToken)
    {
        var readNotifications =
            await _context.UserNotifications
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken);

        var notifications = await _context.Notifications
            .Where(x => x.ValidFrom < DateTime.Now &&
                        x.ValidUntil > DateTime.Now &&
                        x.IsActive)
            .ToListAsync(cancellationToken);

        await HandleNetWorthEntryNotification(notifications, userId, cancellationToken);

        return notifications
            .Select(x => new UserNotificationModel
            {
                Id = x.Id,
                Type = x.Type,
                Message = x.Message,
                Url = x.Url,
                IsRead = readNotifications.Any(y => y.NotificationId == x.Id),
                Date = x.ValidFrom
            })
            .OrderByDescending(x => x.Date);
    }

    private async Task HandleNetWorthEntryNotification(List<Notification> notifications, string userId,
        CancellationToken cancellationToken)
    {
        if (notifications.All(x => x.Type != NetWorthDataType)) return;

        var now = DateTime.Now;
        var hasEntries = await _context.NetWorthEntries
            .AnyAsync(x => x.UserId == userId && x.Date.Month == now.Month && x.Date.Year == now.Year,
                cancellationToken);

        if (hasEntries) notifications.RemoveAll(x => x.Type == NetWorthDataType);
    }
}