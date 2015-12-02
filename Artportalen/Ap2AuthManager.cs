namespace Artportalen
{
    using System;
    using System.Text;

    using Artportalen.Helpers;
    using Artportalen.Response;

    public class Ap2AuthManager
    {
        private readonly string userName;

        private readonly string basicAuthToken;

        private readonly Ap2Client ap2Client;

        private readonly IAuthTokenRepository authTokenRepository;

        public Ap2AuthManager(string userName, string password, Ap2Client ap2Client, IAuthTokenRepository authTokenRepository)
            : this(Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", userName, password))), ap2Client, authTokenRepository)
        {
        }

        public Ap2AuthManager(string basicAuthToken, Ap2Client ap2Client, IAuthTokenRepository authTokenRepository)
        {
            this.basicAuthToken = basicAuthToken;
            var bytes = Convert.FromBase64String(basicAuthToken);
            var unencoded = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            this.userName = unencoded.Split(new[] { ':' })[0];
            this.ap2Client = ap2Client;
            this.authTokenRepository = authTokenRepository;
        }

        public AuthorizeToken GetValidToken()
        {
            var authToken = this.authTokenRepository.Get(this.userName);
            if (authToken.IsValid)
            {
                return authToken;
            }

            return this.GetNewToken();
        }

        public AuthorizeToken GetNewToken()
        {
            var authToken = this.ap2Client.Authorize(this.basicAuthToken);
            this.authTokenRepository.Save(this.userName, authToken);

            return authToken;
        }
    }
}
