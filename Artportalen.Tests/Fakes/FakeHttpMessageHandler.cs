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
        }
    }
}