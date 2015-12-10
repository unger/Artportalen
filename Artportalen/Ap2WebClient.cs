using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artportalen
{
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Net;
    using System.Net.Http;

    using AngleSharp.Parser.Html;

    using Artportalen.Helpers;
    using Artportalen.Request.Web;
    using Artportalen.Response.Web;

    public class Ap2WebClient
    {
        private readonly HttpClient httpClient;
        private readonly CookieContainer cookies = new CookieContainer();

        private NetworkCredential credential;

        private string baseAddress = "https://www.artportalen.se/";

        private IJsonConverter jsonConverter;

        public Ap2WebClient(HttpClientHandler handler = null)
        {
            var messageHandler = handler ?? new HttpClientHandler();

            if (messageHandler.CookieContainer == null)
            {
                messageHandler.CookieContainer = this.cookies;
                messageHandler.UseCookies = true;
            }

            this.httpClient = new HttpClient(messageHandler)
                                  {
                                      BaseAddress = new Uri(this.baseAddress)
                                  };
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

        public IReadOnlyCollection<Cookie> Cookies
        {
            get
            {
                return new ReadOnlyCollection<Cookie>(this.cookies.GetCookies(new Uri(this.baseAddress)).Cast<Cookie>().ToList());
            }
        }

        public void SetCredentials(string username, string password)
        {
            this.credential = new NetworkCredential(username, password);
        }

        public void SetCredentials(string basicAuthToken)
        {
            this.credential = BasicAuthHelper.Decode(basicAuthToken);
        }

        public async Task<bool> AuthorizeAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/LogOn");

            var response = await this.httpClient.SendAsync(request);

            await this.AuthenticateResponseAsync(response);

            return this.IsAuthorized();
        }

        public async Task<IList<SiteResponse>> GetSitesWithinRadiusAsync(SiteRequest query)
        {
            var east1 = (query.East - query.Radius).ToString(CultureInfo.InvariantCulture);
            var east2 = (query.East + query.Radius).ToString(CultureInfo.InvariantCulture);
            var north1 = (query.North - query.Radius).ToString(CultureInfo.InvariantCulture);
            var north2 = (query.North + query.Radius).ToString(CultureInfo.InvariantCulture);

            var bbox = string.Join(",", east1, north1, east2, north2);

            var request = new HttpRequestMessage(HttpMethod.Get, "/Site/Site");
            var response = await this.httpClient.SendAsync(request);

            if (response.RequestMessage.RequestUri.PathAndQuery.StartsWith("/LogOn"))
            {
                response = await this.AuthenticateResponseAsync(response);
            }

            if (response.RequestMessage.RequestUri.PathAndQuery.StartsWith("/Site/Site"))
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var userId = RegexHelper.GetFirstGroup(responseString, @"""UserId"":([0-9]+)");
                var newRelicId = RegexHelper.GetFirstGroup(responseString, @"xpid:""([^""]*)""");

                request = new HttpRequestMessage(HttpMethod.Post, "/Map/GetPrivateAndPublicSitesGeoJson");
                request.Headers.Add("X-NewRelic-ID", newRelicId);
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");

                var postData = new Dictionary<string, string>
                                   {
                                       { "zoomLevel", "13" },
                                       { "bbox", bbox },
                                       { "userId", userId },
                                       { "coordSys", "0" },
                                   };

                request.Content = new FormUrlEncodedContent(postData);

                var siteResponse = await this.httpClient.SendAsync(request);

                var responseStream = await siteResponse.Content.ReadAsStreamAsync();
                var jsonSites = this.JsonConverter.Deserialize<SiteGeoJsonResponse>(responseStream);

                return this.ConvertToSites(jsonSites);
            }

            throw new UnauthorizedAccessException("Could not authenticate request");
        }

        private async Task<HttpResponseMessage> AuthenticateResponseAsync(HttpResponseMessage response)
        {
            if (this.credential == null)
            {
                throw new UnauthorizedAccessException("No credentals added to client, use SetCredentials method");
            }

            var responseStream = await response.Content.ReadAsStreamAsync();

            var parser = new HtmlParser();
            var doc = await parser.ParseAsync(responseStream);
            var requestInput = doc.QuerySelector("input[name=__RequestVerificationToken]");
            var returnUrlInput = doc.QuerySelector("input#AuthenticationViewModel_ReturnUrl");

            var requestVerificationToken = requestInput.GetAttribute("value");
            var returnUrl = returnUrlInput.GetAttribute("value");

            var loginData = new Dictionary<string, string>
                                {
                                    { "__RequestVerificationToken", requestVerificationToken },
                                    { "AuthenticationViewModel.UserName", this.credential.UserName },
                                    { "AuthenticationViewModel.ReturnUrl", returnUrl },
                                    { "AuthenticationViewModel.Password", this.credential.Password },
                                    { "Shared_LogOn", "Logga in" }
                                };

            var loginRequest = new HttpRequestMessage(HttpMethod.Post, "/LogOn")
            {
                Content = new FormUrlEncodedContent(loginData)
            };

            return await this.httpClient.SendAsync(loginRequest);
        }

        private IList<SiteResponse> ConvertToSites(SiteGeoJsonResponse jsonSites)
        {
            var sites = new List<SiteResponse>();

            if (jsonSites != null && jsonSites.points != null && jsonSites.points.features != null)
            {
                foreach (var feature in jsonSites.points.features)
                {
                    var site = new SiteResponse { SiteId = feature.id };

                    if (feature.properties != null)
                    {
                        site.Kommun = feature.properties.siteAreaName;
                        site.ParentId = feature.properties.parentId;
                        site.Accuracy = feature.properties.accuracy;
                        site.IsPublic = feature.properties.siteType != 0;
                        site.Accuracy = feature.properties.accuracy;
                        site.SiteName = feature.properties.siteName;
                    }
                    if (feature.geometry != null && feature.geometry.coordinates != null
                        && feature.geometry.coordinates.Length == 2)
                    {
                        site.SiteXCoord = feature.geometry.coordinates[0];
                        site.SiteYCoord = feature.geometry.coordinates[1];
                    }

                    sites.Add(site);
                }   
            }

            return sites;
        }

        private bool IsAuthorized()
        {
            IEnumerable<Cookie> responseCookies = this.cookies.GetCookies(new Uri(this.baseAddress)).Cast<Cookie>();
            return responseCookies.Any(cookie => cookie.Name == ".ASPXAUTH");
        }
    }
}
