using System;
using System.Threading.Tasks;

namespace Artportalen
{
    using System.Net.Http;
    using System.Net.Http.Headers;

    using Artportalen.Helpers;
    using Artportalen.Request;
    using Artportalen.Response;

    public class Ap2ApiClient
    {
        private readonly HttpClient httpClient;

        private string basicAuthToken;

        private string baseAddress = "https://www.artportalen.se/";

        private IJsonConverter jsonConverter;

        public Ap2ApiClient(string accessKey, HttpClientHandler handler = null)
        {
            var messageHandler = handler ?? new HttpClientHandler();

            this.httpClient = new HttpClient(messageHandler)
                                  {
                                      BaseAddress = new Uri(this.baseAddress)
                                  };

            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.httpClient.DefaultRequestHeaders.Add("access-key", accessKey);
        }

        public IJsonConverter JsonConverter
        {
            get
            {
                return this.jsonConverter ?? (this.jsonConverter = new DataContractJsonConverter());
            }
            set
            {
                this.jsonConverter = value;
            }
        }

        public void SetCredentials(string username, string password)
        {
            this.basicAuthToken = BasicAuthHelper.Encode(username, password);
        }

        public void SetCredentials(string basicAuth)
        {
            this.basicAuthToken = basicAuth;
        }

        public async Task<Sighting> GetSightingAsync(long sightingId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format("/api/sightings/{0}", sightingId));
            this.AddAuthorizationHeader(request);

            var response = await this.httpClient.SendAsync(request);

            var sightingResponse = await this.ParseResponse<Sighting>(response);

            return sightingResponse;
        }

        public async Task<SightingsResponse> GetSightingsAsync(SightingsQuery query)
        {
            var qb = new QueryStringBuilder(query, new[] { "LastSightingId" });

            var request = new HttpRequestMessage(HttpMethod.Get, string.Format("/api/sightings/search?{0}", qb));
            this.AddAuthorizationHeader(request);

            var response = await this.httpClient.SendAsync(request);

            var sightingsResponse = await this.ParseResponse<SightingsResponse>(response);
            sightingsResponse.Query = query;

            foreach (var sighting in sightingsResponse.Data)
            {
                sighting.Source = this.httpClient.BaseAddress.Host;
            }

            return sightingsResponse;
        }

        public async Task<AreaDataset[]> GetAreaDatasetsAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/areadatasets");

            var response = await this.httpClient.SendAsync(request);

            var areaDatasetResponse = await this.ParseResponse<BaseCollection<AreaDataset>>(response);

            return areaDatasetResponse != null ? areaDatasetResponse.Data : new AreaDataset[0];
        }

        private void AddAuthorizationHeader(HttpRequestMessage request)
        {
            if (string.IsNullOrEmpty(this.basicAuthToken))
            {
                throw new UnauthorizedAccessException("No credentals added to client, use SetCredentials method");
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", this.basicAuthToken);
        }

        private async Task<T> ParseResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                return this.JsonConverter.Deserialize<T>(responseStream);
            }

            throw new HttpRequestException(string.Format("{0} {1}", response.StatusCode, response.ReasonPhrase));
        }
    }
}
