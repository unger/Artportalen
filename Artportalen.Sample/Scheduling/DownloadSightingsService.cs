using Artportalen.Sample.Helpers;

namespace Artportalen.Sample.Scheduling
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading;

    using AppHarbor;

    using Artportalen.Helpers;
    using Artportalen.Model;
    using Artportalen.Response;

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

            if (onlyLatest && !lastSightingId.HasValue)
            {
                Console.WriteLine("LastSightingId has no value, exiting.");
                return;
            }
            
            var sendSightingsService = new SendSightingsService();

            HttpMessageHandler handler = this.GetMessageHandler();

            var ap2Client = new Ap2Client(ConfigurationManager.AppSettings["Ap2AccessKey"], handler);
            var authManager = new Ap2AuthManager(ConfigurationManager.AppSettings["Ap2BasicAuthToken"], ap2Client, new CacheAuthTokenRepository());
            var ap2SightingsService = new Ap2SightingsService(ap2Client, authManager);

            try
            {
                var result = this.HandleSightings(null, ap2SightingsService, sendSightingsService, onlyLatest);
                if (result.Data.Length > 0)
                {
                    lastSightingId = result.Data[0].SightingId;
                }

                while (result.Data.Length == result.Pager.PageSize)
                {
                    result = this.HandleSightings(result, ap2SightingsService, sendSightingsService, onlyLatest);
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

        private HttpMessageHandler GetMessageHandler()
        {
            var saveResponse = System.Configuration.ConfigurationManager.AppSettings["SaveResponseToDisk"];
            if (saveResponse == "true")
            {
                return new SaveResponseHttpMessageHandler();
            }

            return null;
        }

        private SightingsResponse HandleSightings(SightingsResponse lastResponse, Ap2SightingsService ap2SightingsService, SendSightingsService sendSightingsService, bool onlyLatest)
        {
            bool useYesterday = DateTime.Now.Hour < 5;
            HttpResponseMessage sendResponse = null;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = lastResponse == null
                ? onlyLatest 
                    ? ap2SightingsService.GetLastThreeDaysSightings(TaxonGroupEnum.Fåglar, lastSightingId)
                    : useYesterday
                        ? ap2SightingsService.GetYesterdaysSightings(TaxonGroupEnum.Fåglar)
                        : ap2SightingsService.GetTodaysSightings(TaxonGroupEnum.Fåglar)
                : ap2SightingsService.GetNextPage(lastResponse);
            stopwatch.Stop();
            if (result.Data.Length > 0)
            {
                Console.WriteLine(
                    "{5} Page {0} items {1}-{2} [{3}] total:{6} ({4}ms)",
                    result.Pager.PageIndex,
                    ((result.Pager.PageIndex - 1) * result.Pager.PageSize) + 1,
                    ((result.Pager.PageIndex - 1) * result.Pager.PageSize) + result.Data.Length,
                    onlyLatest ? lastSightingId.ToString() : string.Empty,
                    stopwatch.ElapsedMilliseconds,
                    onlyLatest ? "Latest" : "Todays",
                    result.Pager.TotalCount);
            }
            else
            {
                Console.WriteLine("{3} Page {0} No items returned [{1}] ({2}ms)", result.Pager.PageIndex, lastSightingId, stopwatch.ElapsedMilliseconds, onlyLatest ? "Latest" : "Todays");
            }

            var uris = this.GetSendSightingsUrls();

            if (uris.Length == 0)
            {
                Console.WriteLine("No external urls defined to send Sightings to");
            }
            else
            {
                foreach (var uri in uris)
                {
                    stopwatch.Restart();
                    var sendStatus = "Success";
                    try
                    {
                        sendResponse = sendSightingsService.Send(result.Data, uri);
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
                            "Send to '{0}' {1} [{2} {3}] ({4}ms)",
                            uri,
                            sendStatus,
                            (int)sendResponse.StatusCode,
                            sendResponse.ReasonPhrase,
                            stopwatch.ElapsedMilliseconds);
                    }
                    else
                    {
                        Console.WriteLine(
                            "Send to '{0}' Failed [response = null] ({1}ms)",
                            uri,
                            stopwatch.ElapsedMilliseconds);
                    }
                }
            }

            return result;
        }

        private Uri[] GetSendSightingsUrls()
        {
            var uris = new List<Uri>();
            var uriStrings = (ConfigurationManager.AppSettings["KustobsarSightingsUrl"] ?? string.Empty).Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var uriString in uriStrings)
            {
                try
                {
                    var uri = new Uri(uriString);
                    uris.Add(uri);
                }
                catch (Exception)
                {
                }
            }

            return uris.ToArray();
        }
    }
}
