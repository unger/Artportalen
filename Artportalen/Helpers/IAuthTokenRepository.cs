namespace Artportalen.Helpers
{
    using Artportalen.Response;

    public interface IAuthTokenRepository
    {
        AuthorizeToken Get(string userName);

        void Save(string userName, AuthorizeToken token);
    }
}