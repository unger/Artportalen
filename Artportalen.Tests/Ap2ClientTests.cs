namespace Artportalen.Tests
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Artportalen.Model;
    using Artportalen.Request;
    using Artportalen.Response;
    using Artportalen.Tests.Fakes;

    using NUnit.Framework;

    [TestFixture]
    public class Ap2ClientTests
    {
        private readonly FakeHttpMessageHandler httpMessageHandler = new FakeHttpMessageHandler();

        private Ap2Client ap2Client;
        private AuthorizeToken authToken;

        [SetUp]
        public void Setup()
        {
            this.ap2Client = new Ap2Client("12345", this.httpMessageHandler);

            this.authToken = this.ap2Client.Authorize("test", "test");
        }

        [Test]
        public void Authorize_VerifyBasicAuthorizationHeader()
        {
            this.ap2Client.Authorize("test", "test");

            Assert.NotNull(this.httpMessageHandler.Request.Headers.Authorization);
            Assert.AreEqual("Basic", this.httpMessageHandler.Request.Headers.Authorization.Scheme);

            var unencoded = Encoding.UTF8.GetString(Convert.FromBase64String(this.httpMessageHandler.Request.Headers.Authorization.Parameter));
            Assert.AreEqual("test:test", unencoded);
        }

        [Test]
        public void Sighting_ById_ShouldReturnSighting()
        {
            var result = this.ap2Client.Sighting(1, this.authToken);

            Assert.IsInstanceOf<Sighting>(result);
        }

        [Test]
        public void Sightings_VerifySerializeOneParameter()
        {
            var result = this.ap2Client.Sightings(new SightingsQuery { TaxonId = 1 }, this.authToken);

            Assert.AreEqual("?TaxonId=1", this.httpMessageHandler.Request.RequestUri.Query);
        }

        [Test]
        public void Sightings_VerifySerializeTwoParameters()
        {
            var result = this.ap2Client.Sightings(new SightingsQuery { TaxonId = 1, PageNumber = 2 }, this.authToken);

            Assert.AreEqual("?TaxonId=1&PageNumber=2", this.httpMessageHandler.Request.RequestUri.Query);
        }

        [Test]
        public void Sightings_VerifySerializeFourParameters()
        {
            var result = this.ap2Client.Sightings(new SightingsQuery { TaxonId = 1, PageNumber = 2, SortField = "StartDate", SortOrder = "Descending" }, this.authToken);

            Assert.AreEqual("?TaxonId=1&PageNumber=2&SortField=StartDate&SortOrder=Descending", this.httpMessageHandler.Request.RequestUri.Query);
        }

        [Test]
        public void Test_CheckSessionAuthorizationHeaderExists()
        {
            this.ap2Client.Test(this.authToken);

            Assert.NotNull(this.httpMessageHandler.Request.Headers.Authorization);
            Assert.AreEqual("Session", this.httpMessageHandler.Request.Headers.Authorization.Scheme);
        }

        [Test]
        public void Test_VerifyStringArrayContent()
        {
            var result = this.ap2Client.Test(this.authToken);

            Assert.AreEqual(new[] { "User:Magnus Unger" }, result);
        }

        [Test]
        public void TestPublic_VerifyAcceptJson()
        {
            this.ap2Client.TestPublic();

            Assert.True(this.httpMessageHandler.Request.Headers.Accept.Contains(new MediaTypeWithQualityHeaderValue("application/json")));
        }

        [Test]
        public void TestPublic_CheckAccessKeyHeaderExists()
        {
            this.ap2Client.TestPublic();

            Assert.True(this.httpMessageHandler.Request.Headers.Contains("access-key"));
        }

        [Test]
        public void TestPublic_VerifyContentStringArray()
        {
            var result = this.ap2Client.TestPublic();

            Assert.AreEqual(new[] { "value1", "value2" }, result);
        }
    }
}
