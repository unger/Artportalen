namespace Artportalen.Helpers
{
    using System.Web;

    using Artportalen.Response;

    public class CacheAuthTokenRepository : IAuthTokenRepository
    {
        public AuthorizeToken Get(string userName)
        {
            var token = HttpRuntime.Cache["AuthToken_" + userName] as AuthorizeToken;
            return token ?? new AuthorizeToken();
        }

        public void Save(string userName, AuthorizeToken token)
        {
            HttpRuntime.Cache["AuthToken_" + userName] = token;
        }
    }
}
