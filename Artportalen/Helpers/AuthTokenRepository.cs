namespace Artportalen.Helpers
{
    using System.Collections.Concurrent;

    using Artportalen.Response;

    public class AuthTokenRepository : IAuthTokenRepository
    {
        private readonly ConcurrentDictionary<string, AuthorizeToken> dictionary = new ConcurrentDictionary<string, AuthorizeToken>();

        public AuthorizeToken Get(string userName)
        {
            return this.dictionary.GetOrAdd(userName, x => new AuthorizeToken());
        }

        public void Save(string userName, AuthorizeToken token)
        {
            this.dictionary.AddOrUpdate(userName, token, (s, oldToken) => token);
        }
    }
}
