
namespace AcoustID.Web
{
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    static class WebHelper
    {
        // For small data sizes, gzip will increase number of bytes to send. We use
        // a threshold to check, if compression should be done.
        private const int COMPRESSION_THRESHOLD = 1800;

        public static HttpClient CreateHttpClient(bool automaticDecompression = true)
        {
            var proxy = Configuration.Proxy;

            var handler = new HttpClientHandler();

            if (proxy != null)
            {
                handler.Proxy = proxy;
                handler.UseProxy = true;
            }

            if (automaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            var client = new HttpClient(handler);

            client.DefaultRequestHeaders.Add("User-Agent", Configuration.UserAgent);

            return client;
        }

        public static async Task<string> SendPost(string url, string query, bool useCompression)
        {
            using (var body = new MemoryStream(Encoding.Default.GetBytes(query)))
            {
                return await WebHelper.SendPost(url, body, useCompression);
            }
        }

        public static async Task<string> SendPost(string url, Stream body, bool useCompression)
        {
            var client = WebHelper.CreateHttpClient();

            // The stream to hold the content bytes (gzipped or not).
            Stream stream;

            int size = (int)body.Length;

            useCompression &= (size > COMPRESSION_THRESHOLD);

            if (useCompression)
            {
                stream = new MemoryStream();

                // Create gzip stream.
                using (var gzip = new GZipStream(stream, CompressionMode.Compress, true))
                {
                    body.CopyTo(gzip);
                }

                // Reset stream position.
                stream.Seek(0L, SeekOrigin.Begin);
            }
            else
            {
                stream = body;
            }

            var content = new StreamContent(stream);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            content.Headers.ContentLength = stream.Length;

            if (useCompression)
            {
                content.Headers.ContentEncoding.Add("gzip");
            }

            var response = await client.PostAsync(url, content);

            if (useCompression)
            {
                // Don't forget to dispose of the memory stream.
                stream.Dispose();
            }

            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> SendGet(string url, string query)
        {
            var client = WebHelper.CreateHttpClient();

            return await client.GetStringAsync(url + "?" + query);
        }
    }
}
