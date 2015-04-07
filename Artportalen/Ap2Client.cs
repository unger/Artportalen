namespace Artportalen
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Authentication;
    using System.Text;
    using System.Web;

    using Artportalen.Request;
    using Artportalen.Response;

    public class Ap2Client
    {
        private const string BaseAddress = "https://test.artportalen.se/";

        private readonly HttpClient httpClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey">All requests need an Access Key</param>
        /// <param name="httpMessageHandler">Used in Unit Tests to check request and response values, otherwise use null</param>
        public Ap2Client(string accessKey, HttpMessageHandler httpMessageHandler = null)
        {
            this.AccessKey = accessKey;
            this.httpClient = this.CreateClient(httpMessageHandler);
        }

        public string AccessKey { get; private set; }

        public Accuracy[] Accuracies()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/accuracies");

            var response = this.Execute<Accuracy[]>(request).Value;

            return response;
        }

        public Activity[] Activities(int? speciesGroupId = null)
        {
            var url = speciesGroupId.HasValue
                          ? string.Format("/api/speciesgroup/{0}/activities", speciesGroupId)
                          : "/api/activities";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = this.Execute<BaseCollection<Activity>>(request).Value;

            return response != null ? response.Data : new Activity[0];
        }

        public AuthorizeToken Authorize(string user, string password)
        {
            var basicAuthToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", user, password)));
            return this.Authorize(basicAuthToken);
        }

        public AuthorizeToken Authorize(string basicAuthToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/token");

            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuthToken);

            return this.Execute<AuthorizeToken>(request).Value;
        }

        public CoordinateSystem[] CoordinateSystems()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/coordinatesystems");

            var response = this.Execute<BaseCollection<CoordinateSystem>>(request).Value;

            return response != null ? response.Data : new CoordinateSystem[0];
        }

        public DeterminationMethod[] DeterminationMethods(int? speciesGroupId = null)
        {
            var url = speciesGroupId.HasValue
                          ? string.Format("/api/speciesgroup/{0}/determinationmethods", speciesGroupId)
                          : "/api/determinationmethods";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = this.Execute<BaseCollection<DeterminationMethod>>(request).Value;

            return response != null ? response.Data : new DeterminationMethod[0];
        }

        public DiscoveryMethod[] DiscoveryMethods(int? speciesGroupId = null)
        {
            var url = speciesGroupId.HasValue
                          ? string.Format("/api/speciesgroup/{0}/discoverymethods", speciesGroupId)
                          : "/api/discoverymethods";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = this.Execute<BaseCollection<DiscoveryMethod>>(request).Value;

            return response != null ? response.Data : new DiscoveryMethod[0];
        }

        public Gender[] Genders(int? speciesGroupId = null)
        {
            var url = speciesGroupId.HasValue
                          ? string.Format("/api/speciesgroup/{0}/genders", speciesGroupId)
                          : "/api/genders";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = this.Execute<BaseCollection<Gender>>(request).Value;

            return response != null ? response.Data : new Gender[0];
        }

        public Sighting Sighting(long id, AuthorizeToken authToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format("/api/sightings/{0}", id));
            this.AddSessionAuthorizationHeader(request, authToken);

            return this.Execute<Sighting>(request).Value;
        }

        public SightingsResponse Sightings(SightingsQuery query, AuthorizeToken authToken)
        {
            string queryString = this.GetQueryString(query);
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format("/api/sightings/search?{0}", queryString));
            this.AddSessionAuthorizationHeader(request, authToken);

            var response = this.Execute<SightingsResponse>(request).Value;
            response.Query = query;

            return response;
        }

        public Site[] SitesWithinRadius(SitesQuery query, AuthorizeToken authToken)
        {
            if (query.CoordSysId == 0)
            {
                throw new ArgumentException("Coordinate system needs to be set in query");
            }

            if (string.IsNullOrEmpty(query.East))
            {
                throw new ArgumentException("East coordinate needs to be set in query");
            }

            if (string.IsNullOrEmpty(query.North))
            {
                throw new ArgumentException("North coordinate needs to be set in query");
            }

            if (query.Radius == 0)
            {
                throw new ArgumentException("Radius needs to be set in query");
            }

            string queryString = this.GetQueryString(new
                                                         {
                                                             speciesGroupId = query.SpeciesGroupId,
                                                             count = query.Count
                                                         });
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format("/api/sites/withinradius/coordsystem/{0}/east/{1}/north/{2}/radius/{3}?{4}", query.CoordSysId, query.East, query.North, query.Radius, queryString));
            this.AddSessionAuthorizationHeader(request, authToken);

            var response = this.Execute<BaseCollection<Site>>(request).Value;

            return response != null ? response.Data : new Site[0];
        }

        public Site[] SitesContainingPoint(SitesQuery query, AuthorizeToken authToken)
        {
            if (query.CoordSysId == 0)
            {
                throw new ArgumentException("Coordinate system needs to be set in query");
            }

            if (string.IsNullOrEmpty(query.East))
            {
                throw new ArgumentException("East coordinate needs to be set in query");
            }

            if (string.IsNullOrEmpty(query.North))
            {
                throw new ArgumentException("North coordinate needs to be set in query");
            }

            string queryString = this.GetQueryString(new
                                                         {
                                                             speciesGroupId = query.SpeciesGroupId,
                                                             count = query.Count
                                                         });
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format("/api/sites/containingpoint/coordsystem/{0}/east/{1}/north/{2}/?{3}", query.CoordSysId, query.East, query.North, queryString));
            this.AddSessionAuthorizationHeader(request, authToken);

            var response = this.Execute<BaseCollection<Site>>(request).Value;

            return response != null ? response.Data : new Site[0];
        }

        public SpeciesGroup[] SpeciesGroups()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/speciesgroups");

            var response = this.Execute<BaseCollection<SpeciesGroup>>(request).Value;

            return response != null ? response.Data : new SpeciesGroup[0];
        }

        public Stage[] Stages(int? speciesGroupId = null)
        {
            var url = speciesGroupId.HasValue
                          ? string.Format("/api/speciesgroup/{0}/stages", speciesGroupId)
                          : "/api/stages";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = this.Execute<BaseCollection<Stage>>(request).Value;

            return response != null ? response.Data : new Stage[0];
        }

        public Taxon Taxon(int taxonId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format("/api/taxon/{0}", taxonId));

            return this.Execute<Taxon>(request).Value;
        }

        public string[] Test(AuthorizeToken authToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/test");
            this.AddSessionAuthorizationHeader(request, authToken);

            return this.Execute<string[]>(request).Value;
        }

        public string[] TestPublic()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/testvalues");

            return this.Execute<string[]>(request).Value;
        }

        public Unit[] Units(int? speciesGroupId = null)
        {
            var url = speciesGroupId.HasValue
                          ? string.Format("/api/speciesgroup/{0}/units", speciesGroupId)
                          : "/api/units";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = this.Execute<BaseCollection<Unit>>(request).Value;

            return response != null ? response.Data : new Unit[0];
        }

        private ResponseWrapper<T> Execute<T>(HttpRequestMessage request) where T : class
        {
            return new ResponseWrapper<T>(this.httpClient.SendAsync(request).Result);
        }

        private void AddSessionAuthorizationHeader(HttpRequestMessage request, AuthorizeToken authToken)
        {
            if (!authToken.IsValid)
            {
                throw new AuthenticationException("AuthorizeToken not valid call Authorize method to authenticate");
            }

            if (string.IsNullOrEmpty(authToken.access_token))
            {
                throw new ArgumentException("Authorization needed AuthToken need to be set, call Authorize method to authenticate");
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Session", authToken.access_token);
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
