namespace Artportalen.Response
{
    using System;
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
            var content = response.Content != null ? response.Content.ReadAsStringAsync().Result : null;

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new Exception(string.Format(
                    "Remote server returned {0} {1}\n\n [{2}]\n\n {3}",
                    (int)response.StatusCode,
                    response.ReasonPhrase,
                    response.RequestMessage.RequestUri,
                    content));
            }

            if (content != null)
            {
                if (typeof(T) == typeof(string))
                {
                    return content as T;
                }

                try
                {
                    return new JavaScriptSerializer().Deserialize<T>(content);
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format(
                        "Remote server returned {0} {1}\n\n [{2}]\n\n {3}\n {4}", 
                        (int)response.StatusCode, 
                        response.ReasonPhrase, 
                        response.RequestMessage.RequestUri,
                        e.Message,
                        content));
                }
             }

            return default(T);
        }
    }
}
