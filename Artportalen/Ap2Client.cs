namespace Artportalen
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;

    public class Ap2Client
    {
        private const string BaseAddress = "https://test.artportalen.se/";

        private readonly HttpClient httpClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey">All requests need an Access Key</param>
        /// <param name="httpMessageHandler">Used in Unit Tests to check request and response values</param>
        public Ap2Client(string accessKey, HttpMessageHandler httpMessageHandler = null)
        {
            this.AccessKey = accessKey;
            this.httpClient = this.CreateClient(httpMessageHandler);
        }

        public string AuthToken { get; private set; }

        public string AccessKey { get; private set; }

        public void Authorize(string user, string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/token");

            this.AddBasicAuthorizationHeader(request, user, password);

            var response = this.Execute<string>(request);
            this.AuthToken = response.Value;
        }

        public string Test()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/test");
            this.AddSessionAuthorizationHeader(request);

            var response = this.Execute<string>(request);
            return response.Value;
        }

        public string TestPublic()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/testvalues");
            
            var response = this.Execute<string>(request);
            return response.Value;
        }

        private ResponseWrapper<T> Execute<T>(HttpRequestMessage request) where T : class
        {
            return new ResponseWrapper<T>(this.httpClient.SendAsync(request).Result);
        }

        private void AddBasicAuthorizationHeader(HttpRequestMessage request, string user, string password)
        {
            var basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", user, password)));

            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
        }

        private void AddSessionAuthorizationHeader(HttpRequestMessage request)
        {
            if (string.IsNullOrEmpty(this.AuthToken))
            {
                throw new ArgumentException("Authorization needed AuthToken need to be set, call Authorize method to authenticate");
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Session", this.AuthToken);
        }

        private HttpClient CreateClient(HttpMessageHandler httpMessageHandler = null)
        {
            var client = httpMessageHandler != null ? new HttpClient(httpMessageHandler) : new HttpClient();

            client.BaseAddress = new Uri(BaseAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (string.IsNullOrEmpty(this.AccessKey))
            {
                throw new ArgumentException("Access key is not set");
            }
            
            client.DefaultRequestHeaders.Add("access-key", this.AccessKey);

            return client;
        }
    }
}
