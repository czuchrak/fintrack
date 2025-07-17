using Fintrack.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.Database;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Setting> Settings { get; set; }
    public DbSet<Log> Logs { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<UserNotification> UserNotifications { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<ExchangeRate> ExchangeRates { get; set; }

    public DbSet<NetWorthPart> NetWorthParts { get; set; }
    public DbSet<NetWorthEntry> NetWorthEntries { get; set; }
    public DbSet<NetWorthEntryPart> NetWorthEntryParts { get; set; }
    public DbSet<NetWorthGoal> NetWorthGoals { get; set; }
    public DbSet<NetWorthGoalPart> NetWorthGoalParts { get; set; }

    public DbSet<Property> Properties { get; set; }
    public DbSet<PropertyCategory> PropertyCategories { get; set; }
    public DbSet<PropertyTransaction> PropertyTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("dbo");

        modelBuilder.Entity<Log>().HasKey(x => x.Id);
        modelBuilder.Entity<Setting>().HasKey(x => x.Name);
        modelBuilder.Entity<User>().HasKey(x => x.Id);
        modelBuilder.Entity<Currency>().HasKey(x => x.Code);
        modelBuilder.Entity<ExchangeRate>().HasKey(x => new { x.Date, x.Currency });
        modelBuilder.Entity<Notification>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
        modelBuilder.Entity<UserNotification>().HasKey(x => new { x.NotificationId, x.UserId });

        modelBuilder.Entity<NetWorthPart>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
        modelBuilder.Entity<NetWorthPart>()
            .HasOne(e => e.User)
            .WithMany(e => e.NetWorthParts)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<NetWorthGoal>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
        modelBuilder.Entity<NetWorthGoalPart>().HasKey(x => new { x.NetWorthPartId, x.NetWorthGoalId });
        modelBuilder.Entity<NetWorthEntry>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
        modelBuilder.Entity<NetWorthEntryPart>().HasKey(x => new { x.NetWorthPartId, x.NetWorthEntryId });

        modelBuilder.Entity<Property>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
        modelBuilder.Entity<PropertyCategory>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
        modelBuilder.Entity<PropertyTransaction>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
    }
}