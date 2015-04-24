namespace Artportalen.Sample.Scheduling
{
    using System;
    using System.Diagnostics;
    using System.Net.Http;

    using AppHarbor;

    using Artportalen.Helpers;
    using Artportalen.Model;
    using Artportalen.Response;
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

            var schedEnabled = System.Configuration.ConfigurationManager.AppSettings["SchedulingEnabled"];
            if (schedEnabled != "true")
            {
                Console.WriteLine("Scheduling disabled");
                return;
            }

            var ap2Client = new Ap2Client(System.Configuration.ConfigurationManager.AppSettings["Ap2AccessKey"]);
            var authManager = new Ap2AuthManager(System.Configuration.ConfigurationManager.AppSettings["Ap2BasicAuthToken"], ap2Client, new CacheAuthTokenRepository());
            var ap2SightingsService = new Ap2SightingsService(ap2Client, authManager);

            try
            {
                var result = this.HandleSightings(null, ap2SightingsService, sightingsService, sendSightingsService);
                if (result.Data.Length > 0)
                {
                    lastSightingId = result.Data[0].SightingId;
                }

                while (result.Data.Length == result.Pager.PageSize)
                {
                    result = this.HandleSightings(result, ap2SightingsService, sightingsService, sendSightingsService);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                if (ap2Client.LastResponseMessage != null)
                {
                    var content = ap2Client.LastResponseMessage.Content != null
                        ? ap2Client.LastResponseMessage.Content.ReadAsStringAsync().Result
                        : string.Empty;

                    Console.WriteLine(
                            "{0} {1}", 
                            (int)ap2Client.LastResponseMessage.StatusCode, 
                            ap2Client.LastResponseMessage.ReasonPhrase);
                }

                logger.Error(ConsoleMirror.Captured);
            }
        }

        private SightingsResponse HandleSightings(SightingsResponse lastResponse, Ap2SightingsService ap2SightingsService, SightingsService sightingsService, SendSightingsService sendSightingsService)
        {
            HttpResponseMessage sendResponse = null;
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            var result = lastResponse == null 
                ? ap2SightingsService.GetLastThreeDaysSightings(SpeciesGroupEnum.Fåglar, lastSightingId) 
                : ap2SightingsService.GetNextPage(lastResponse);
            stopwatch.Stop();
            Console.WriteLine(
                "Page {0} items {1}-{2} [{3}] ({4}ms)", 
                result.Pager.PageIndex,
                ((result.Pager.PageIndex - 1) * result.Pager.PageSize) + 1,
                ((result.Pager.PageIndex - 1) * result.Pager.PageSize) + result.Data.Length,
                lastSightingId, 
                stopwatch.ElapsedMilliseconds);

            stopwatch.Restart();
            var saveStatus = "Success";
            try
            {
                sightingsService.StoreSightings(result.Data);
            }
            catch (Exception saveException)
            {
                saveStatus = string.Format("Failed ({0})", saveException.Message);
            }
            finally
            {
                stopwatch.Stop();
            }

            Console.WriteLine("Store sightings {0} ({1}ms)", saveStatus, stopwatch.ElapsedMilliseconds);

            stopwatch.Restart();
            var sendStatus = "Success";
            try
            {
                sendResponse = sendSightingsService.SendToKustobsar(result.Data);
            }
            catch (Exception sendException)
            {
                sendStatus = string.Format("Failed ({0})", sendException.Message);
            }
            finally
            {
                stopwatch.Stop();
            }

            if (sendResponse != null)
            {
                Console.WriteLine(
                    "Send to Kustobsar {0} [{1} {2}] ({3}ms)",
                    sendStatus,
                    (int)sendResponse.StatusCode,
                    sendResponse.ReasonPhrase,
                    stopwatch.ElapsedMilliseconds);
            }
            else
            {
                Console.WriteLine(
                    "Send to Kustobsar Failed [response = null] ({0}ms)",
                    stopwatch.ElapsedMilliseconds);
            }

            return result;
        }
    }
}
