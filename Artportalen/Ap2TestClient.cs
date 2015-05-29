namespace Artportalen
{
    using System.Net.Http;

    public class Ap2TestClient : Ap2Client
    {
        public Ap2TestClient(string accessKey, HttpMessageHandler httpMessageHandler = null)
            : base(accessKey, httpMessageHandler)
        {
            this.BaseAddress = "https://test.artportalen.se/";
        }
    }
}
