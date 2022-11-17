namespace Fintrack.App.Functions.Admin.Models;

public class NotificationModel
{
    public Guid? Id { get; set; }

    public string Type { get; set; }

    public string Message { get; set; }

    public string Url { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidUntil { get; set; }

    public bool IsActive { get; set; }

    public int ReadCount { get; set; }

    public int UsersCount { get; set; }
}