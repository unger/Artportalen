namespace Artportalen.Tests.Fakes
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        public HttpRequestMessage Request { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this.Request = request;

            return Task<HttpResponseMessage>.Factory.StartNew(
                () =>
                    {
                        var response = request.CreateResponse();

                        this.CreateContent(response);

                        return response;
                    },
                    cancellationToken);
        }

        private void CreateContent(HttpResponseMessage response)
        {
            if (response.RequestMessage.RequestUri.PathAndQuery == "/api/token")
            {
                response.Content = new StringContent("12345");
            }
            else if (response.RequestMessage.RequestUri.PathAndQuery == "/api/sighting/1")
            {
                response.Content = new StringContent(@"{
                              ""SightingId"": 1,
                              ""ScientificName"": ""sample string 2"",
                              ""Author"": ""sample string 3"",
                              ""CommonName"": ""sample string 4"",
                              ""Lan"": ""sample string 5"",
                              ""Forsamling"": ""sample string 6"",
                              ""Kommun"": ""sample string 7"",
                              ""Socken"": ""sample string 8"",
                              ""Landskap"": ""sample string 9"",
                              ""SiteYCoord"": 10,
                              ""SiteXCoord"": 11,
                              ""Unit"": ""sample string 12"",
                              ""QuantityOfSubstrate"": 1,
                              ""DiscoveryMethod"": ""sample string 13"",
                              ""Length"": 1,
                              ""Weight"": 1,
                              ""PrivateComment"": {
                                ""Comment"": ""sample string 1""
                              },
                              ""SightingObservers"": ""sample string 14"",
                              ""StartDate"": ""2015-03-29T23:38:48.7472384+02:00"",
                              ""StartTime"": ""00:00:00.1234567"",
                              ""EndDate"": ""2015-03-29T23:38:48.7472384+02:00"",
                              ""EndTime"": ""00:00:00.1234567"",
                              ""TaxonId"": 17,
                              ""Quantity"": 18,
                              ""SiteName"": ""sample string 19"",
                              ""Accuracy"": 20,
                              ""UnsureDetermination"": true,
                              ""NotRecovered"": true,
                              ""MinDepth"": 1,
                              ""MaxDepth"": 1,
                              ""MinHeight"": 1,
                              ""MaxHeight"": 1,
                              ""PublicComment"": ""sample string 23""
                            }");
            }
        }
    }
}