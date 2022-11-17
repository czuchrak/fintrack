using System;
using System.Threading.Tasks;
using Fintrack.Database;
using Fintrack.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.Extensions;

public static class DatabaseExtensions
{
    public static async Task CreateDevelopmentDatabase(this DatabaseContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.Settings.AnyAsync())
            context.Settings.Add(new Setting { Name = "AdminId", Value = "12345" });

        if (!await context.Currencies.AnyAsync())
            context.Currencies.AddRange(
                new Currency { Code = "PLN" },
                new Currency { Code = "USD" },
                new Currency { Code = "GBP" },
                new Currency { Code = "EUR" },
                new Currency { Code = "CHF" });

        if (!await context.PropertyCategories.AnyAsync())
            context.PropertyCategories.AddRange(
                new PropertyCategory { Name = "Czynsz", IsCost = true, Type = "rent" },
                new PropertyCategory { Name = "Prąd", IsCost = true, Type = "electricity" },
                new PropertyCategory { Name = "Zakup nieruch.", IsCost = true, Type = "buy" },
                new PropertyCategory { Name = "Przychód z najmu", IsCost = false, Type = "rentalIncome" }
            );

        if (!await context.ExchangeRates.AnyAsync())
            context.ExchangeRates.AddRange(
                new ExchangeRate { Currency = "EUR", Date = DateTime.Now, Rate = 4.7M },
                new ExchangeRate { Currency = "USD", Date = DateTime.Now, Rate = 4.5M },
                new ExchangeRate { Currency = "GBP", Date = DateTime.Now, Rate = 5.2M },
                new ExchangeRate { Currency = "CHF", Date = DateTime.Now, Rate = 4.6M }
            );

        if (!await context.Notifications.AnyAsync())
            context.Notifications.Add(
                new Notification
                {
                    Message = "Testowe",
                    IsActive = true,
                    Type = "update",
                    ValidFrom = DateTime.Now.AddMonths(-1),
                    ValidUntil = DateTime.Now.AddYears(1)
                }
            );

        await context.SaveChangesAsync();
    }
}