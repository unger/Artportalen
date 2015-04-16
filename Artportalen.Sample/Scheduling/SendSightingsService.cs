using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artportalen.Sample.Scheduling
{
    using System.Configuration;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Script.Serialization;

    using Artportalen.Response;

    using NLog;

    public class SendSightingsService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void SendToKustobsar(IEnumerable<Sighting> sightings)
        {
            var client = new HttpClient();

            var httpContent = new StringContent(new JavaScriptSerializer().Serialize(new { sightings }));
            httpContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

            HttpResponseMessage response = client.PostAsync(this.GetKustobsarUri(), httpContent).Result;

            Console.WriteLine("SendToKustObsar response: {0} {1}", response.StatusCode, response.ReasonPhrase);
        }

        private Uri GetKustobsarUri()
        {
            return new Uri(ConfigurationManager.AppSettings["KustobsarSightingsUrl"]);
        }
    }
}
