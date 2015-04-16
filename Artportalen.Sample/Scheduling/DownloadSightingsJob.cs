namespace Artportalen.Sample.Scheduling
{
    using System;
    using System.Diagnostics;

    using AppHarbor;

    using Artportalen.Helpers;
    using Artportalen.Model;
    using Artportalen.Sample.Data.Services;

    using NLog;

    using Quartz;

    public class DownloadSightingsJob : IJob
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static long? lastSightingId;

        public void Execute(IJobExecutionContext jobContext)
        {
            var sightingsService = new SightingsService();
            var sendSightingsService = new SendSightingsService();

            ConsoleMirror.Initialize();

            Console.WriteLine(DateTimeOffset.Now);

            var ap2Client = new Ap2Client(System.Configuration.ConfigurationManager.AppSettings["Ap2AccessKey"]);
            var authManager = new Ap2AuthManager(System.Configuration.ConfigurationManager.AppSettings["Ap2BasicAuthToken"], ap2Client, new CacheAuthTokenRepository());
            var ap2SightingsService = new Ap2SightingsService(ap2Client, authManager);

            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var result = ap2SightingsService.GetLastThreeDaysSightings(SpeciesGroupEnum.Fåglar, lastSightingId);
                stopwatch.Stop();
                Console.WriteLine("Page {0} count {1} [{2}] ({3}ms)", result.Pager.PageIndex, result.Data.Length, lastSightingId, stopwatch.ElapsedMilliseconds);

                stopwatch.Restart();
                sightingsService.StoreSightings(result.Data);
                stopwatch.Stop();
                Console.WriteLine("Store sightings ({0}ms)", stopwatch.ElapsedMilliseconds);

                stopwatch.Restart();
                var response = sendSightingsService.SendToKustobsar(result.Data);
                stopwatch.Stop();
                Console.WriteLine("Send to Kustobsar [{0} {1}] ({2}ms)", response.StatusCode, response.ReasonPhrase, stopwatch.ElapsedMilliseconds);
                
                if (result.Data.Length > 0)
                {
                    lastSightingId = result.Data[0].SightingId;
                }

                while (result.Pager.HasNextPage)
                {
                    stopwatch.Restart();
                    result = ap2SightingsService.GetNextPage(result);
                    stopwatch.Stop();
                    Console.WriteLine("Page {0} count {1} [{2}] ({3}ms)", result.Pager.PageIndex, result.Data.Length, result.Query.LastSightingId, stopwatch.ElapsedMilliseconds);

                    stopwatch.Restart();
                    sightingsService.StoreSightings(result.Data);
                    stopwatch.Stop();
                    Console.WriteLine("Store sightings ({0}ms)", stopwatch.ElapsedMilliseconds);

                    stopwatch.Restart();
                    response = sendSightingsService.SendToKustobsar(result.Data);
                    stopwatch.Stop();

                    Console.WriteLine("Send to Kustobsar [{0} {1}] ({2}ms)", response.StatusCode, response.ReasonPhrase, stopwatch.ElapsedMilliseconds);
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
