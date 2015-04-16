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

            var job = JobBuilder.Create<DownloadSightingsJob>().Build();

            var trigger = TriggerBuilder.Create().WithSimpleSchedule(x => x.WithIntervalInSeconds(checkInterval).RepeatForever()).Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
