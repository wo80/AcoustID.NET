// -----------------------------------------------------------------------
// <copyright file="LookupService.cs" company="">
// Christian Woltering, https://github.com/wo80
// </copyright>
// -----------------------------------------------------------------------

namespace AcoustID.Web
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Calls the AcoustID webservice to lookup audio data for a given fingerprint.
    /// </summary>
    public class LookupService
    {
        private const string URL = "http://api.acoustid.org/v2/lookup";

        private IResponseParser parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupService" /> class.
        /// </summary>
        public LookupService()
            : this(new XmlResponseParser())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupService" /> class.
        /// </summary>
        /// <param name="parser">The <see cref="IResponseParser"/> instance.</param>
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
        /// Calls the webservice.
        /// </summary>
        /// <param name="fingerprint">The audio fingerprint.</param>
        /// <param name="duration">The total duration of the audio.</param>
        /// <returns>A task which returns a <see cref="LookupResponse"/>.</returns>
        public Task<LookupResponse> GetAsync(string fingerprint, int duration)
        {
            return GetAsync(fingerprint, duration, null);
        }

        /// <summary>
        /// Calls the webservice.
        /// </summary>
        /// <param name="fingerprint">The audio fingerprint.</param>
        /// <param name="duration">The total duration of the audio.</param>
        /// <param name="meta">Request meta information.</param>
        /// <returns>A task which returns a <see cref="LookupResponse"/>.</returns>
        public async Task<LookupResponse> GetAsync(string fingerprint, int duration, string[] meta)
        {
            try
            {
                using (var body = BuildRequestBody(fingerprint, duration, meta))
                {
                    // If the request contains invalid parameters, the server will return
                    // "400 Bad Request" and we'll end up in the first catch block.
                    string response = await WebHelper.SendPost(URL, body, UseCompression);

                    return parser.ParseLookupResponse(response);
                }
            }
            catch (WebException e)
            {
                // Handle bad requests gracefully.
                return CreateErrorResponse(e.Response as HttpWebResponse);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private LookupResponse CreateErrorResponse(HttpWebResponse response)
        {
            if (response == null)
            {
                return new LookupResponse(HttpStatusCode.BadRequest, "Unknown error.");
            }

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var text = reader.ReadToEnd();

                if (parser.CanParse(text))
                {
                    return parser.ParseLookupResponse(text);
                }

                // TODO: parse error message (JSON).
                return new LookupResponse(response.StatusCode, text);
            }
        }

        private Stream BuildRequestBody(string fingerprint, int duration, string[] meta)
        {
            var stream = new MemoryStream();

            using (var writer = new StreamWriter(stream, Encoding.Default, 1024, true))
            {
                writer.Write("client=" + Configuration.ClientKey);

                if (meta != null)
                {
                    writer.Write("&meta=" + string.Join("+", meta));
                }

                writer.Write("&format=" + parser.Format);
                writer.Write("&duration=" + duration);
                writer.Write("&fingerprint=" + fingerprint);
            }

            // Reset stream position.
            stream.Seek(0L, SeekOrigin.Begin);

            return stream;
        }
    }
}
