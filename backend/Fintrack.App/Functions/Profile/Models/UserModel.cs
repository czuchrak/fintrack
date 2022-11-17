using Fintrack.App.Functions.Property.Models;

namespace Fintrack.App.Functions.Profile.Models;

public class UserModel
{
    public bool IsAdmin { get; set; }

    public bool MailVerificationSent { get; set; }

    public IEnumerable<UserNotificationModel> Notifications { get; set; }

    public UserSettingsModel UserSettings { get; set; }

    public IEnumerable<PropertyModel> Properties { get; set; }

    public IEnumerable<string> Currencies { get; set; }

    public string Currency { get; set; }
}

public class UserNotificationModel
{
    public Guid Id { get; set; }

    public string Type { get; set; }

    public string Message { get; set; }

    public string Url { get; set; }

    public bool IsRead { get; set; }

    public DateTime Date { get; set; }
}

public class UserSettingsModel
{
    public bool NewMonthEmailEnabled { get; set; }

    public bool NewsEmailEnabled { get; set; }
}