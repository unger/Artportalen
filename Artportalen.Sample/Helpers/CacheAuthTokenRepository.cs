using System.Web;
using Artportalen.Helpers;
using Artportalen.Response;

namespace Artportalen.Sample.Helpers
{
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
