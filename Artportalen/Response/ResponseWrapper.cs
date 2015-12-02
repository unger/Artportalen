using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Artportalen.Response
{
    using System;
    using System.Net;
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
            var content = response.Content != null ? response.Content.ReadAsStringAsync().Result : null;

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new Exception(string.Format(
                    "Remote server returned {0} {1}\n\n [{2}]\n\n{3}",
                    (int)response.StatusCode,
                    response.ReasonPhrase,
                    response.RequestMessage.RequestUri,
                    content));
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException(string.Format(
                    "Remote server returned {0} {1}\n\n [{2}]\n\n{3}",
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
                    return JsonDeserialize<T>(content);
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format(
                        "Remote server returned {0} {1}\n\n [{2}]\n\n {3}", 
                        (int)response.StatusCode, 
                        response.ReasonPhrase, 
                        response.RequestMessage.RequestUri,
                        e.Message));
                }
             }

            return default(T);
        }

        private TObject JsonDeserialize<TObject>(string content)
        {
            var serializer = new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings { DateTimeFormat = new DateTimeFormat("yyyy-MM-ddTHH:mm:ss") });
            return (TObject)serializer.ReadObject(ConvertToStream(content));
        }

        public static Stream ConvertToStream(string content)
        {
            var memStream = new MemoryStream();
            var streamWriter = new StreamWriter(memStream);

            streamWriter.Write(content);

            streamWriter.Flush();
            memStream.Seek(0, SeekOrigin.Begin);
            return memStream;
        }
    }
}
