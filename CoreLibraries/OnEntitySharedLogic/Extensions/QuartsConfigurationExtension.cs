using Microsoft.Extensions.Configuration;
using Quartz;

namespace OnEntitySharedLogic.Extensions;

public static class QuartsConfigurationExtension
{
    public static void AddJobAndTrigger<TJob>(this IServiceCollectionQuartzConfigurator quartz, IConfiguration configuration) where TJob : IJob
    {
        var jobName = typeof(TJob).Name;

        var jobExecutingPeriod = configuration.GetSection("QuartzJobPeriods")[jobName];

        if (jobExecutingPeriod is null)
        {
            throw new Exception($"No period has been set up for the job {jobName}!");
        }

        quartz.AddJob<TJob>(options =>
        {
            options.WithIdentity(jobName);
        });
        
        var triggerIdentity = jobName + "-trigger";
        quartz.AddTrigger(options =>
        {
            options
                .ForJob(jobName)
                .WithIdentity(triggerIdentity)
                .WithCronSchedule(jobExecutingPeriod);
        });
    }
}