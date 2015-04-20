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

    using Newtonsoft.Json;

    using NLog;

    public class SendSightingsService
    {
        public HttpResponseMessage SendToKustobsar(IEnumerable<Sighting> sightings)
        {
            var client = new HttpClient();

            var content = JsonConvert.SerializeObject(new { sightings = sightings });

            var httpContent = new StringContent(content);
            httpContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

            return client.PostAsync(this.GetKustobsarUri(), httpContent).Result;
        }

        private Uri GetKustobsarUri()
        {
            return new Uri(ConfigurationManager.AppSettings["KustobsarSightingsUrl"]);
        }
    }
}
