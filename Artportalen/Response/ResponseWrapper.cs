namespace Artportalen.Response
{
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Script.Serialization;

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
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpException(string.Format("Remote server returned Internal Server Error: {0}", response.ReasonPhrase));
            }

            var content = response.Content != null ? response.Content.ReadAsStringAsync().Result : null;
            if (content != null)
            {
                if (typeof(T) == typeof(string))
                {
                    return content as T;
                }

                return new JavaScriptSerializer().Deserialize<T>(content);
             }

            return default(T);
        }
    }
}
