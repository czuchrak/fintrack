using System;
using System.IO;
using Fintrack.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Fintrack;

public static class Program
{
    private static readonly string EnvironmentName =
        Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

    private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile($"appsettings.{EnvironmentName}.json", true)
        .Build();

    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .AddEnvironmentConfiguration(EnvironmentName, Configuration)
            .CreateLogger();

        AppDomain.CurrentDomain.ProcessExit += (_, _) => Log.CloseAndFlush();

        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}