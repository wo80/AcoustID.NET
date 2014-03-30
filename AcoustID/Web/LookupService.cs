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
        static string CLIENT = "8XaBELgH";
        static string URL = "http://api.acoustid.org/v2/lookup";

        IResponseParser parser;

        /// <summary>
        /// Gets or sets if gzip compression is used to compress request data.
        /// </summary>
        public bool CompressData { get; set; }

        /// <summary>
        /// Gets the last error message (check this if parse methods return empty lists).
        /// </summary>
        public string Error
        {
            get { return parser.Error; }
        }

        public LookupService()
            : this(new XmlResponseParser())
        {
        }

        public LookupService(IResponseParser parser)
        {
            this.parser = parser;

            CompressData = true;
        }

        /// <summary>
        /// Calls the webservice on a background thread.
        /// </summary>
        /// <param name="callback">Callback function.</param>
        /// <param name="fingerprint">The audio fingerprint.</param>
        /// <param name="duration">The total duration of the audio.</param>
        /// <remarks>
        /// WARNING: Don't use this in a console app. An exception concerning the
        /// SynchronizationContext will be thrown.
        /// </remarks>
        public void GetAsync(Action<List<LookupResult>, Exception> callback, string fingerprint, int duration)
        {
            GetAsync(callback, fingerprint, duration, null);
        }

        /// <summary>
        /// Calls the webservice on a background thread.
        /// </summary>
        /// <param name="callback">Callback function.</param>
        /// <param name="fingerprint">The audio fingerprint.</param>
        /// <param name="duration">The total duration of the audio.</param>
        /// <param name="meta">Request meta information.</param>
        /// <remarks>
        /// WARNING: Don't use this in a console app. An exception concerning the
        /// SynchronizationContext will be thrown.
        /// </remarks>
        public void GetAsync(Action<List<LookupResult>, Exception> callback, string fingerprint, int duration, string[] meta)
        {
            TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();

            Task.Factory.StartNew(() =>
            {
                List<LookupResult> result = null;
                Exception exception = null;

                try
                {
                    result = Get(fingerprint, duration, meta);
                }
                catch (Exception e)
                {
                    exception = e;
                }

                var progress = Task.Factory.StartNew(() =>
                {
                    callback(result, exception);
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                scheduler);

                // TODO: pass exceptions to callback

                // Report progress on UI thread
                progress.Wait();
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

                return parser.ParseLookupResponse(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string BuildRequestString(string fingerprint, int duration, string[] meta)
        {
            StringBuilder request = new StringBuilder();

            request.Append("client=" + CLIENT);

            if (meta != null)
            {
                request.Append("&meta=" + String.Join("+", meta));
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
            if (this.CompressData && request.Length > 1800)
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
