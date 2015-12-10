using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artportalen.Helpers
{
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;

    public class DataContractJsonConverter : IJsonConverter
    {
        private DataContractJsonSerializerSettings settings;

        public DataContractJsonConverter(DataContractJsonSerializerSettings settings = null)
        {
            this.settings = settings ?? new DataContractJsonSerializerSettings
                                {
                                    DateTimeFormat = new DateTimeFormat("yyyy-MM-ddTHH:mm:ss")
                                };
        }

        public string Serialize<T>(object obj)
        {
            string content;
            using (var memStream = new MemoryStream())
            {
                this.Serialize<T>(memStream, obj);
                memStream.Seek(0, 0);

                using (var sr = new StreamReader(memStream))
                {
                    content = sr.ReadToEnd();
                }
            }

            return content;
        }

        public void Serialize<T>(Stream stream, object obj)
        {
            var serializer = new DataContractJsonSerializer(typeof(T), this.settings);
            serializer.WriteObject(stream, obj);
        }

        public T Deserialize<T>(Stream stream)
        {
            var serializer = new DataContractJsonSerializer(typeof(T), this.settings);
            return (T)serializer.ReadObject(stream);
        }

        public T Deserialize<T>(string content)
        {
            return this.Deserialize<T>(ConvertToStream(content));
        }

        private static Stream ConvertToStream(string content)
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
