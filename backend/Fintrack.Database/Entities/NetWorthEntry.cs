using System.ComponentModel.DataAnnotations;

namespace Fintrack.Database.Entities;

public class NetWorthEntry : BaseEntity
{
    [Required] [MaxLength(50)] public string UserId { get; set; }

    public DateTime Date { get; set; }

    public DateTime ExchangeRateDate { get; set; }

    public virtual ICollection<NetWorthEntryPart> EntryParts { get; set; }

    public virtual User User { get; set; }
}