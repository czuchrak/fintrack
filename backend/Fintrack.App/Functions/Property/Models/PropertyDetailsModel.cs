using Fintrack.App.Functions.Admin.Models;

namespace Fintrack.App.Functions.Property.Models;

public class PropertyDetailsModel
{
    public Guid? Id { get; set; }

    public string Name { get; set; }

    public IEnumerable<PropertyTransactionModel> Transactions { get; set; }

    public IEnumerable<PropertyCategoryModel> Categories { get; set; }

    public bool IsActive { get; set; }
}