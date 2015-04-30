namespace Artportalen.Sample.Scheduling
{
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading;

    using AppHarbor;

    using Artportalen.Helpers;
    using Artportalen.Model;
    using Artportalen.Response;
    using Artportalen.Sample.Data.Services;

    using NLog;

    using Quartz;

    public class DownloadLatestSightingsJob : IJob
    {
        public void Execute(IJobExecutionContext jobContext)
        {
            Console.WriteLine("DownloadLatestSightingsJob started: {0}", DateTimeOffset.Now);

            var schedEnabled = System.Configuration.ConfigurationManager.AppSettings["SchedulingEnabled"];
            if (schedEnabled != "true")
            {
                Console.WriteLine("Scheduling disabled");
                return;
            }

            var downloadSightings = new DownloadSightingsService();
            downloadSightings.DownloadLatestAddedSightings();
        }
    }
}
