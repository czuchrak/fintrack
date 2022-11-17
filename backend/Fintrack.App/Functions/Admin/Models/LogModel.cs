namespace Fintrack.App.Functions.Admin.Models;

public class LogModel
{
    public int Id { get; set; }

    public string? Level { get; set; }

    public string? Message { get; set; }

    public DateTimeOffset Timestamp { get; set; }

    public string? Exception { get; set; }
}