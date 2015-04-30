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

    public class DownloadSightingsService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        private static long? lastSightingId;

        public void DownloadAllTodaysSightings()
        {
            this.DownloadSightings(false);
        }

        public void DownloadLatestAddedSightings()
        {
            this.DownloadSightings(true);
        }

        private void DownloadSightings(bool onlyLatest)
        {
            ConsoleMirror.Initialize();
            
            var sightingsService = new SightingsService();
            var sendSightingsService = new SendSightingsService();

            var ap2Client = new Ap2Client(System.Configuration.ConfigurationManager.AppSettings["Ap2AccessKey"]);
            var authManager = new Ap2AuthManager(System.Configuration.ConfigurationManager.AppSettings["Ap2BasicAuthToken"], ap2Client, new CacheAuthTokenRepository());
            var ap2SightingsService = new Ap2SightingsService(ap2Client, authManager);

            lastSightingId = sightingsService.GetLastSightingId();

            try
            {
                var result = this.HandleSightings(null, ap2SightingsService, sightingsService, sendSightingsService, onlyLatest);
                if (result.Data.Length > 0)
                {
                    lastSightingId = result.Data[0].SightingId;
                }

                while (result.Data.Length == result.Pager.PageSize)
                {
                    result = this.HandleSightings(result, ap2SightingsService, sightingsService, sendSightingsService, onlyLatest);
                    Thread.Sleep(5000);
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

        private SightingsResponse HandleSightings(SightingsResponse lastResponse, Ap2SightingsService ap2SightingsService, SightingsService sightingsService, SendSightingsService sendSightingsService, bool onlyLatest)
        {
            HttpResponseMessage sendResponse = null;
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            var result = lastResponse == null
                ? onlyLatest 
                    ? ap2SightingsService.GetLastThreeDaysSightings(SpeciesGroupEnum.Fåglar, lastSightingId)
                    : ap2SightingsService.GetTodaysSightings(SpeciesGroupEnum.Fåglar)
                : ap2SightingsService.GetNextPage(lastResponse);
            stopwatch.Stop();
            if (result.Data.Length > 0)
            {
                Console.WriteLine(
                    "{5} Page {0} items {1}-{2} [{3}] ({4}ms)",
                    result.Pager.PageIndex,
                    ((result.Pager.PageIndex - 1) * result.Pager.PageSize) + 1,
                    ((result.Pager.PageIndex - 1) * result.Pager.PageSize) + result.Data.Length,
                    onlyLatest ? lastSightingId.ToString() : string.Empty,
                    stopwatch.ElapsedMilliseconds,
                    onlyLatest ? "Latest" : "Todays");
            }
            else
            {
                Console.WriteLine("{3} Page {0} No items returned [{1}] ({2}ms)", result.Pager.PageIndex, lastSightingId, stopwatch.ElapsedMilliseconds, onlyLatest ? "Latest" : "Todays");
            }

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
