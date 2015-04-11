﻿namespace Artportalen.Sample.Scheduling
{
    using System;
    using System.Net.Mail;

    using AppHarbor;

    using Artportalen.Helpers;
    using Artportalen.Model;

    using NLog;
    using NLog.Internal;

    using Quartz;

    public class DownloadSightingsJob : IJob
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static long? lastSightingId;

        public void Execute(IJobExecutionContext jobContext)
        {
            ConsoleMirror.Initialize();

            Console.WriteLine(DateTimeOffset.Now);

            var ap2Client = new Ap2Client(System.Configuration.ConfigurationManager.AppSettings["Ap2AccessKey"]);
            var authManager = new Ap2AuthManager(System.Configuration.ConfigurationManager.AppSettings["Ap2BasicAuthToken"], ap2Client, new CacheAuthTokenRepository());
            var sightingsService = new Ap2SightingsService(ap2Client, authManager);

            try
            {
                var result = sightingsService.GetLastThreeDaysSightings(SpeciesGroupEnum.Fåglar, lastSightingId);

                Console.WriteLine("Page {0} count {1} [{2}]", result.Pager.PageIndex, result.Data.Length, lastSightingId);

                if (result.Data.Length > 0)
                {
                    lastSightingId = result.Data[0].SightingId;
                }

                while (result.Pager.HasNextPage)
                {
                    result = sightingsService.GetNextPage(result);
                    Console.WriteLine("Page {0} count {1} [{2}]", result.Pager.PageIndex, result.Data.Length, result.Query.LastSightingId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                if (ap2Client.LastResponseMessage != null)
                {
                    Console.WriteLine(
                            "{0} {1}: {2}", 
                            ap2Client.LastResponseMessage.StatusCode, 
                            ap2Client.LastResponseMessage.ReasonPhrase, 
                            ap2Client.LastResponseMessage.Content != null ? ap2Client.LastResponseMessage.Content.ReadAsStringAsync().Result : string.Empty);
                }

                logger.Error(ConsoleMirror.Captured);
            }
        }
    }
}
