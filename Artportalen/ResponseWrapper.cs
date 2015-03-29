namespace Artportalen
{
    using System.Net.Http;

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
            return response.Content != null ? response.Content.ReadAsStringAsync().Result as T : default(T);
        }
    }
}
