using System;
using System.Text;

namespace Artportalen.Helpers
{
    using System.Net;

    public class BasicAuthHelper
    {
        public static string Encode(string username, string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, password)));
        }

        public static string Encode(NetworkCredential credential)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", credential.UserName, credential.Password)));
        }

        public static NetworkCredential Decode(string basicAuthToken)
        {
            var bytes = Convert.FromBase64String(basicAuthToken);
            var unencoded = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            var values = unencoded.Split(':');
            if (values.Length == 2)
            {
                return new NetworkCredential(values[0], values[1]);
            }

            throw new ArgumentException("Not a valid basicAuthToken");
        }
    }
}
