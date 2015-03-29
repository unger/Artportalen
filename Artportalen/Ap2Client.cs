namespace Artportalen
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;

    public class Ap2Client
    {
        private const string BaseAddress = "https://test.artportalen.se/";

        public string AuthToken { get; set; }

        public string AccessKey { get; set; }

        public HttpMessageHandler HttpMessageHandler { get; set; }

        public void Authorize(string user, string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/token");

            this.AddBasicAuthorizationHeader(request, user, password);

            var response = this.Execute(request);
            this.AuthToken = response.Content.ReadAsStringAsync().Result;
        }

        public string Test()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/test");
            this.AddSessionAuthorizationHeader(request);

            var response = this.Execute(request);
            return response.Content != null ? response.Content.ReadAsStringAsync().Result : string.Empty;
        }

        public string TestPublic()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/testvalues");
            
            var response = this.Execute(request);
            return response.Content != null ? response.Content.ReadAsStringAsync().Result : string.Empty;
        }

        private HttpResponseMessage Execute(HttpRequestMessage request)
        {
            this.AddAccessKeyHeader(request);

            using (var httpClient = this.CreateClient())
            {
                return httpClient.SendAsync(request).Result;
            }
        }

        private void AddAccessKeyHeader(HttpRequestMessage request)
        {
            if (string.IsNullOrEmpty(this.AccessKey))
            {
                throw new ArgumentException("Access key is not set");
            }

            request.Headers.Add("access-key", this.AccessKey);
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

        private HttpClient CreateClient()
        {
            if (this.HttpMessageHandler != null)
            {
                return new HttpClient(this.HttpMessageHandler) { BaseAddress = new Uri(BaseAddress) };
            }

            return new HttpClient { BaseAddress = new Uri(BaseAddress) };
        }
    }
}
