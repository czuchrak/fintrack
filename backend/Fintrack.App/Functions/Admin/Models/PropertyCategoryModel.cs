namespace Fintrack.App.Functions.Admin.Models;

public class PropertyCategoryModel
{
    public Guid? Id { get; set; }

    public string Name { get; set; }

    public string Type { get; set; }

    public bool IsCost { get; set; }

    public int Count { get; set; }
}