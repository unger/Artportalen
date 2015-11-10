using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Artportalen.Sample.Scheduling
{
    public class SaveResponseHttpMessageHandler : HttpClientHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith<Task<HttpResponseMessage>>(async t =>
            {
                var response = t.Result;
                var content = await response.Content.ReadAsStringAsync();

                using (var file = File.CreateText(string.Format("{0}.txt", DateTime.Now.ToString("yyyyMMddHHmmss"))))
                {
                    file.Write(content);
                }

                return t.Result;
            }
            , cancellationToken).Unwrap();
        }
    }
}