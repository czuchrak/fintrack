namespace Fintrack.App.Functions.Admin.Models;

public class UserModel
{
    public string Id { get; set; }

    public string Name { get; set; }

    public DateTime LastActivity { get; set; }

    public DateTime CreationDate { get; set; }

    public bool NewMonthEmailEnabled { get; set; }

    public bool NewsEmailEnabled { get; set; }

    public int PartsCount { get; set; }

    public int EntriesCount { get; set; }

    public int PropertiesCount { get; set; }

    public int GoalsCount { get; set; }
}