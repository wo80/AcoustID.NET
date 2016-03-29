
namespace AcoustID.Web
{
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    static class WebHelper
    {
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

        public static async Task<string> SendPost(string url, string body, bool useCompression)
        {
            var client = WebHelper.CreateHttpClient();

            // The stream to hold the gzipped bytes.
            using (var stream = new MemoryStream())
            {
                byte[] data = Encoding.UTF8.GetBytes(body);

                // For small data size, gzip will increase number of bytes to send.
                useCompression &= body.Length > 1800;

                if (useCompression)
                {
                    // Create gzip stream
                    using (var gzip = new GZipStream(stream, CompressionMode.Compress, true))
                    {
                        gzip.Write(data, 0, data.Length);
                        gzip.Close();
                    }
                }
                else
                {
                    stream.Write(data, 0, data.Length);
                }

                // IMPORTANT: reset stream position.
                stream.Position = 0;

                var content = new StreamContent(stream);

                if (useCompression)
                {
                    content.Headers.Add("Content-Encoding", "gzip");
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                }

                var response = await client.PostAsync(url, content);

                return await response.Content.ReadAsStringAsync();
            }
        }

        public static async Task<string> SendGet(string url, string query)
        {
            var client = WebHelper.CreateHttpClient();

            return await client.GetStringAsync(url + "?" + query);
        }
    }
}
