using System;
using Fintrack.Database;
using Fintrack.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.Tests;

public class TestBase
{
    protected const string UserId = "1234";
    private readonly DbContextOptions<DatabaseContext> _contextOptions;

    protected TestBase()
    {
        _contextOptions = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase("DatabaseContextTests")
            .Options;

        using var context = new DatabaseContext(_contextOptions);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.Settings.Add(new Setting { Name = "AdminId", Value = UserId });
        context.Users.Add(new User
        {
            Id = UserId,
            Email = "test@example.com",
            Currency = "PLN",
            CreationDate = DateTime.Now,
            LastActivity = DateTime.Now,
            NewMonthEmailEnabled = true,
            NewsEmailEnabled = true,
            VerificationMailSent = null
        });
        context.Currencies.Add(new Currency { Code = "PLN" });
        context.Currencies.Add(new Currency { Code = "EUR" });
        context.Currencies.Add(new Currency { Code = "GBP" });
        context.Currencies.Add(new Currency { Code = "USD" });
        context.Currencies.Add(new Currency { Code = "CHF" });

        context.SaveChanges();
    }

    protected DatabaseContext CreateContext()
    {
        return new DatabaseContext(_contextOptions);
    }
}