// -----------------------------------------------------------------------
// <copyright file="LookupService.cs" company="">
// Christian Woltering, https://github.com/wo80
// </copyright>
// -----------------------------------------------------------------------

namespace AcoustID.Web
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Calls the AcoustID webservice to lookup audio data for a given fingerprint.
    /// </summary>
    public class LookupService
    {
        private const string URL = "http://api.acoustid.org/v2/lookup";

        private IResponseParser parser;

        public LookupService()
            : this(new XmlResponseParser())
        {
        }

        public LookupService(IResponseParser parser)
        {
            this.parser = parser;

            UseCompression = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to compress the data before submit.
        /// </summary>
        public bool UseCompression { get; set; }

        /// <summary>
        /// Gets the last error message (check this if parse methods return empty lists).
        /// </summary>
        public string Error
        {
            get { return parser.Error; }
        }

        /// <summary>
        /// Calls the webservice on a worker thread.
        /// </summary>
        /// <param name="fingerprint">The audio fingerprint.</param>
        /// <param name="duration">The total duration of the audio.</param>
        /// <returns>A task which, on success, returns a list of lookup results.</returns>
        public Task<List<LookupResult>> GetAsync(string fingerprint, int duration)
        {
            return GetAsync(fingerprint, duration, null);
        }

        /// <summary>
        /// Calls the webservice on a worker thread.
        /// </summary>
        /// <param name="fingerprint">The audio fingerprint.</param>
        /// <param name="duration">The total duration of the audio.</param>
        /// <param name="meta">Request meta information.</param>
        /// <returns>A task which, on success, returns a list of lookup results.</returns>
        public Task<List<LookupResult>> GetAsync(string fingerprint, int duration, string[] meta)
        {
            return Task.Factory.StartNew<List<LookupResult>>(() =>
            {
                try
                {
                    return Get(fingerprint, duration, meta);
                }
                catch (Exception)
                {
                    throw;
                }
            });
        }

        /// <summary>
        /// Calls the webservice.
        /// </summary>
        /// <param name="fingerprint">The audio fingerprint.</param>
        /// <param name="duration">The total duration of the audio.</param>
        /// <returns>List of lookup results.</returns>
        public List<LookupResult> Get(string fingerprint, int duration)
        {
            return Get(fingerprint, duration, null);
        }

        /// <summary>
        /// Calls the webservice.
        /// </summary>
        /// <param name="fingerprint">The audio fingerprint.</param>
        /// <param name="duration">The total duration of the audio.</param>
        /// <param name="meta">Request meta information.</param>
        /// <returns>List of lookup results.</returns>
        public List<LookupResult> Get(string fingerprint, int duration, string[] meta)
        {
            try
            {
                string request = BuildRequestString(fingerprint, duration, meta);
                string response = RequestService(request);

                // TODO: server might return an error message as json.
                //       Should probably add a json parser anyway ...
                return parser.ParseLookupResponse(response);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string BuildRequestString(string fingerprint, int duration, string[] meta)
        {
            StringBuilder request = new StringBuilder();

            request.Append("client=" + Configuration.ApiKey);

            if (meta != null)
            {
                request.Append("&meta=" + string.Join("+", meta));
            }

            request.Append("&format=" + parser.Format);
            request.Append("&duration=" + duration);
            request.Append("&fingerprint=" + fingerprint);

            return request.ToString();
        }

        private string RequestService(string request)
        {
            WebClient client = new WebClient();
            client.Headers.Add("User-Agent", "AcoustId.Net/" + ChromaContext.Version);
            client.Proxy = null;

            // For small data size, gzip will increase number of bytes to send.
            if (this.UseCompression && request.Length > 1800)
            {
                // The stream to hold the gzipped bytes.
                using (var stream = new MemoryStream())
                {
                    var encoding = Encoding.UTF8;

                    byte[] data = encoding.GetBytes(request);

                    // Create gzip stream
                    using (GZipStream gzip = new GZipStream(stream, CompressionMode.Compress))
                    {
                        gzip.Write(data, 0, data.Length);
                        gzip.Close();
                    }

                    double ratio = 1 / (double)data.Length;

                    data = stream.ToArray();

                    // ratio = (compressed size) / (uncompressed size)
                    ratio *= data.Length;

                    if (ratio > 0.95)
                    {
                        // Use standard get request
                        return client.DownloadString(URL + "?" + request);
                    }

                    client.Headers.Add("Content-Encoding", "gzip");
                    client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    data = client.UploadData(URL, data);
                    return encoding.GetString(data);
                }
            }
            else
            {
                return client.DownloadString(URL + "?" + request);
            }
        }
    }
}
