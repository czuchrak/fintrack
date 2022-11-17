using System.ComponentModel.DataAnnotations;

namespace Fintrack.Database.Entities;

public class Notification : BaseEntity
{
    [Required] [MaxLength(20)] public string Type { get; set; }

    [Required] [MaxLength(200)] public string Message { get; set; }

    [MaxLength(100)] public string Url { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidUntil { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<UserNotification> UserNotifications { get; set; }
}