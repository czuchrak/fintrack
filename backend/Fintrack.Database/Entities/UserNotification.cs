using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fintrack.Database.Entities;

public class UserNotification
{
    [Key]
    [Column(Order = 0)]
    [MaxLength(36)]
    public Guid NotificationId { get; set; }

    [MaxLength(50)]
    [Key]
    [Column(Order = 1)]
    public string UserId { get; set; }

    public virtual Notification Notification { get; set; }

    public virtual User User { get; set; }
}