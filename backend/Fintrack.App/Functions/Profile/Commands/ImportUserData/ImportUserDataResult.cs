namespace Fintrack.App.Functions.Profile.Commands.ImportUserData;

public class ImportUserDataResult
{
    public int PartsAdded { get; set; }
    public int EntriesAdded { get; set; }
    public int PropertiesAdded { get; set; }
    public int PropertyTransactionsAdded { get; set; }
    public List<string> Errors { get; set; } = [];
}