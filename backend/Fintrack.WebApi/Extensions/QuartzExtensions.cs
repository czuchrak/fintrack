using Fintrack.App.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Fintrack.Extensions;

public static class QuartzExtensions
{
    public static void AddQuartzJobs(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection("Quartz").Get<QuartzSettings>();

        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();

            q.AddJob<FillExchangeRatesJob>(settings.FillExchangeRatesJob1Cron, "-1");
            q.AddJob<FillExchangeRatesJob>(settings.FillExchangeRatesJob2Cron, "-2");
            q.AddJob<RemoveUnnecessaryRatesJob>(settings.RemoveUnnecessaryRatesJobCron);
            q.AddJob<FillEntryNotificationsJob>(settings.FillEntryNotificationsJobCron);
            q.AddJob<SendStatusMailJob>(settings.SendStatusMailJobCron);
            q.AddJob<KeepAppJob>(settings.KeepAppJobCron);
        });

        services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; });
    }

    private static void AddJob<T>(this IServiceCollectionQuartzConfigurator q, string cron, string suffix = null)
        where T : IJob
    {
        var name = typeof(T).Name + suffix;
        var jobKey = new JobKey(name);
        q.AddJob<T>(opts => opts.WithIdentity(jobKey));

        q.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity($"{name}-trigger")
            .WithCronSchedule(cron));
    }
}

public class QuartzSettings
{
    public string FillExchangeRatesJob1Cron { get; set; }
    public string FillExchangeRatesJob2Cron { get; set; }
    public string RemoveUnnecessaryRatesJobCron { get; set; }
    public string FillEntryNotificationsJobCron { get; set; }
    public string SendStatusMailJobCron { get; set; }
    public string KeepAppJobCron { get; set; }
}