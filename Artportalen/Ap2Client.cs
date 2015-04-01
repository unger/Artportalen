﻿namespace Artportalen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Web;

    using Artportalen.Request;
    using Artportalen.Response;

    public class Ap2Client
    {
        private const string BaseAddress = "https://test.artportalen.se/";

        private readonly HttpClient httpClient;

        private string basicAuthorizationParameter;

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
            var basicAuthToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", user, password)));
            this.Authorize(basicAuthToken);
        }

        public void Authorize(string basicAuthToken)
        {
            this.basicAuthorizationParameter = basicAuthToken;
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/token");

            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuthToken);

            var response = this.Execute<AuthorizeTokenResponse>(request);

            if (response.Value != null)
            {
                this.AuthToken = response.Value.access_token;
            }
        }

        public CoordinateSystem[] CoordinateSystems()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/coordinatesystems");

            var response = this.Execute<BaseCollection<CoordinateSystem>>(request).Value;

            return response != null ? response.Data : new CoordinateSystem[0];
        }

        public Sighting Sighting(long id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format("/api/sighting/{0}", id));
            this.AddSessionAuthorizationHeader(request);

            return this.Execute<Sighting>(request).Value;
        }

        public SightingsCollection Sightings(SightingsQuery search)
        {
            string queryString = this.GetQueryString(search);
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format("/api/sightings/search?{0}", queryString));
            this.AddSessionAuthorizationHeader(request);

            return this.Execute<SightingsCollection>(request).Value;
        }

        public Site[] Sites(int coordSysId, string east, string north, int radius, int count, int? speciesGroupId = null)
        {
            string queryString = this.GetQueryString(new
                                                         {
                                                             speciesGroupId,
                                                             count
                                                         });
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format("/api/sites/withinradius/coordsystem/{0}/east/{1}/north/{2}/radius/{3}?{4}", coordSysId, east, north, radius, queryString));
            this.AddSessionAuthorizationHeader(request);

            var response = this.Execute<BaseCollection<Site>>(request).Value;

            return response != null ? response.Data : new Site[0];
        }

        public SpeciesGroup[] SpeciesGroups()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/speciesgroups");

            var response = this.Execute<BaseCollection<SpeciesGroup>>(request).Value;

            return response != null ? response.Data : new SpeciesGroup[0];
        }

        public string[] Test()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/test");
            this.AddSessionAuthorizationHeader(request);

            return this.Execute<string[]>(request).Value;
        }

        public string[] TestPublic()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/testvalues");

            return this.Execute<string[]>(request).Value;
        }

        private ResponseWrapper<T> Execute<T>(HttpRequestMessage request) where T : class
        {
            return new ResponseWrapper<T>(this.httpClient.SendAsync(request).Result);
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

        private string GetQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return string.Join("&", properties.ToArray());
        }
    }
}
