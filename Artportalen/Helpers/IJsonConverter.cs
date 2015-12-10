namespace Artportalen.Helpers
{
    using System.IO;

    public interface IJsonConverter
    {
        string Serialize<T>(object obj);

        void Serialize<T>(Stream stream, object obj);

        T Deserialize<T>(Stream stream);

        T Deserialize<T>(string content);
    }
}