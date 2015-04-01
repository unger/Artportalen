namespace Artportalen.Response
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Net.Http;

    using fastJSON;

    public class ResponseWrapper<T> where T : class
    {
        public ResponseWrapper(HttpResponseMessage responseMessage)
        {
            this.ResponseMessage = responseMessage;
            this.Value = this.ParseValue(responseMessage);
        }

        public T Value { get; private set; }

        public HttpResponseMessage ResponseMessage { get; private set; }

        private T ParseValue(HttpResponseMessage response)
        {
            var content = response.Content != null ? response.Content.ReadAsStringAsync().Result : null;
            if (content != null)
            {
                if (typeof(T) == typeof(string))
                {
                    return content as T;
                }
                
                // special handling for string[]
                if (typeof(T).IsArray && typeof(T).GetElementType() == typeof(string))
                {
                    var result = JSON.Parse(content) as List<object>;
                    if (result != null)
                    {
                        return result.ConvertAll(o => o.ToString()).ToArray() as T;
                    }
                }

                return JSON.ToObject<T>(content);
            }

            return default(T);
        }
    }
}
