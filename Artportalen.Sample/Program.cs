namespace Artportalen.Sample
{
    using System;
    using System.Configuration;

    using Artportalen.Sample.Scheduling;

    using Quartz;
    using Quartz.Impl;

    public class Program
    {
        public static void Main(string[] args)
        {
            int checkInterval;
            if (!int.TryParse(ConfigurationManager.AppSettings["ArtportalenCheckInterval"], out checkInterval))
            {
                checkInterval = 1800;
            }

            Console.WriteLine(checkInterval);

            // construct a scheduler
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler();
            scheduler.Start();

            var latestjob = JobBuilder.Create<DownloadLatestSightingsJob>().Build();
            var todaysjob = JobBuilder.Create<DownloadTodaysSightingsJob>().Build();
            var latestTrigger = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInSeconds(checkInterval).RepeatForever()).Build();
            var todaysTrigger = TriggerBuilder.Create().StartAt(new DateTimeOffset(DateTime.Now.AddSeconds(120))).WithSimpleSchedule(x => x.WithIntervalInSeconds(checkInterval * 3).RepeatForever()).Build();

            scheduler.ScheduleJob(latestjob, latestTrigger);
            scheduler.ScheduleJob(todaysjob, todaysTrigger);
        }
    }
}
