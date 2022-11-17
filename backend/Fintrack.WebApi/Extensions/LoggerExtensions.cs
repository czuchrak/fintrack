using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace Fintrack.Extensions;

public static class LoggerExtensions
{
    public static LoggerConfiguration AddEnvironmentConfiguration(
        this LoggerConfiguration logger,
        string environmentName,
        IConfiguration configuration)
    {
        var isDevelopment = environmentName is "Development";
        var dbLog = configuration.GetConnectionString("Database");
        var sinkOpts = new MSSqlServerSinkOptions { TableName = "Logs" };
        var columnOpts = new ColumnOptions();
        columnOpts.Store.Remove(StandardColumn.Properties);
        columnOpts.Store.Remove(StandardColumn.MessageTemplate);

        if (isDevelopment)
            logger = logger.WriteTo.Console();
        else
            logger = logger
                .MinimumLevel.Warning()
                .MinimumLevel.Override("Fintrack", LogEventLevel.Information);

        return logger
            .WriteTo.MSSqlServer(
                dbLog,
                sinkOpts,
                columnOptions: columnOpts,
                restrictedToMinimumLevel: isDevelopment ? LogEventLevel.Warning : LogEventLevel.Information);
    }
}