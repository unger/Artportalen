namespace Artportalen.Tests
{
    using System;
    using System.Net.Http.Headers;
    using System.Text;

    using Artportalen.Request;
    using Artportalen.Response;
    using Artportalen.Tests.Fakes;

    using NUnit.Framework;

    [TestFixture]
    public class Ap2ClientTests
    {
        private readonly FakeHttpMessageHandler httpMessageHandler = new FakeHttpMessageHandler();

        private Ap2Client ap2Client;

        [SetUp]
        public void Setup()
        {
            this.ap2Client = new Ap2Client("12345", this.httpMessageHandler);
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
            var result = this.ap2Client.Sighting(1);

            Assert.IsInstanceOf<Sighting>(result);
        }

        [Test]
        public void Sightings_VerifySerializeOneParameter()
        {
            var result = this.ap2Client.Sightings(new SightingsQuery { TaxonId = 1 });

            Assert.AreEqual("?TaxonId=1", this.httpMessageHandler.Request.RequestUri.Query);
        }

        [Test]
        public void Sightings_VerifySerializeTwoParameters()
        {
            var result = this.ap2Client.Sightings(new SightingsQuery { TaxonId = 1, PageNumber = 2 });

            Assert.AreEqual("?TaxonId=1&PageNumber=2", this.httpMessageHandler.Request.RequestUri.Query);
        }

        [Test]
        public void Sightings_VerifySerializeFourParameters()
        {
            var result = this.ap2Client.Sightings(new SightingsQuery { TaxonId = 1, PageNumber = 2, SortField = "StartDate", SortOrder = "Descending" });

            Assert.AreEqual("?TaxonId=1&PageNumber=2&SortField=StartDate&SortOrder=Descending", this.httpMessageHandler.Request.RequestUri.Query);
        }

        [Test]
        public void Test_CheckSessionAuthorizationHeaderExists()
        {
            this.ap2Client.Authorize("test", "test");

            this.ap2Client.Test();

            Assert.NotNull(this.httpMessageHandler.Request.Headers.Authorization);
            Assert.AreEqual("Session", this.httpMessageHandler.Request.Headers.Authorization.Scheme);
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
    }
}
