using System.Collections.Generic;
using System.Linq;

namespace Artportalen.Helpers
{
    using System.Net;
    using System.Reflection;

    public class QueryStringBuilder
    {
        private List<KeyValuePair<string, string>> queryItems = new List<KeyValuePair<string, string>>();

        public QueryStringBuilder()
        {
        }

        public QueryStringBuilder(object obj, string[] ignoreProperties = null)
        {
            this.Add(obj, ignoreProperties);
        }

        public void Add(object obj, string[] ignoreProperties = null)
        {
            var ignores = ignoreProperties ?? new string[0];

            var properties = from p in obj.GetType().GetRuntimeProperties()
                             where p.GetValue(obj, null) != null && !ignores.Contains(p.Name)
                             select new KeyValuePair<string, string>(p.Name, p.GetValue(obj, null).ToString());

            this.queryItems.AddRange(properties);
        }

        public void Add(string key, string value)
        {
            this.queryItems.Add(new KeyValuePair<string, string>(key, value));
        }

        public override string ToString()
        {
            return string.Join("&", this.queryItems.Select(kvp => WebUtility.UrlEncode(kvp.Key) + "=" + WebUtility.UrlEncode(kvp.Value)));
        }
    }
}
