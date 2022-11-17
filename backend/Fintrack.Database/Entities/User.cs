using System.ComponentModel.DataAnnotations;

namespace Fintrack.Database.Entities;

public class User
{
    [MaxLength(50)] public string Id { get; set; }

    [MaxLength(200)] public string Email { get; set; }

    public DateTime LastActivity { get; set; }

    public DateTime CreationDate { get; set; }

    public bool NewMonthEmailEnabled { get; set; }

    public bool NewsEmailEnabled { get; set; }

    public DateTime? VerificationMailSent { get; set; }

    [Required] [MaxLength(3)] public string Currency { get; set; }

    public virtual ICollection<NetWorthEntry> NetWorthEntries { get; set; }

    public virtual ICollection<NetWorthGoal> NetWorthGoals { get; set; }

    public virtual ICollection<NetWorthPart> NetWorthParts { get; set; }

    public virtual ICollection<Property> Properties { get; set; }

    public virtual ICollection<UserNotification> UserNotifications { get; set; }
}