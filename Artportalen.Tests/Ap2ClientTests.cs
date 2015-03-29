namespace Artportalen.Tests
{
    using System;
    using System.Net.Http.Headers;
    using System.Text;

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
