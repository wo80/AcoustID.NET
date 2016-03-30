// -----------------------------------------------------------------------
// <copyright file="SubmitService.cs" company="">
// Christian Woltering, https://github.com/wo80
// </copyright>
// -----------------------------------------------------------------------

namespace AcoustID.Web
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Calls the AcoustId webservice to submit a new fingerprint.
    /// </summary>
    public class SubmitService
    {
        private const string URL = "http://api.acoustid.org/v2/submit";

        private const string STATUS_URL = "http://api.acoustid.org/v2/submission_status";

        private IResponseParser parser;

        private string userKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitService" /> class.
        /// </summary>
        /// <param name="userKey">The user API key.</param>
        /// <remarks>
        /// Visit https://acoustid.org/api-key to get a user key.
        /// </remarks>
        public SubmitService(string userKey)
            : this(userKey, new XmlResponseParser())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitService" /> class.
        /// </summary>
        /// <param name="userKey">The user API key.</param>
        /// <param name="parser">The <see cref="IResponseParser"/> instance.</param>
        /// <remarks>
        /// Visit https://acoustid.org/api-key to get a user key.
        /// </remarks>
        public SubmitService(string userKey, IResponseParser parser)
        {
            this.userKey = userKey;
            this.parser = parser;

            UseCompression = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to compress the data before submit.
        /// </summary>
        public bool UseCompression { get; set; }

        /// <summary>
        /// Submit audio data to the AcoustID webservice.
        /// </summary>
        /// <param name="request">The submit request data.</param>
        /// <returns></returns>
        public async Task<SubmitResponse> SubmitAsync(SubmitRequest request)
        {
            return await SubmitAsync(new List<SubmitRequest>(1) { request });
        }

        /// <summary>
        /// Submit audio data to the AcoustID webservice.
        /// </summary>
        /// <param name="requests">The submit request data.</param>
        /// <returns></returns>
        public async Task<SubmitResponse> SubmitAsync(IEnumerable<SubmitRequest> requests)
        {
            try
            {
                using (var body = BuildRequestBody(requests))
                {
                    // If the request contains invalid parameters, the server will return
                    // "400 Bad Request" and we'll end up in the first catch block.
                    string response = await WebHelper.SendPost(URL, body, UseCompression);

                    return parser.ParseSubmitResponse(response);
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

        /// <summary>
        /// Get the status of a pending submission.
        /// </summary>
        /// <param name="submit">The pending submission.</param>
        /// <returns></returns>
        public async Task<SubmitResponse> GetSubmitStatusAsync(SubmitResult submit)
        {
            return await GetSubmitStatusAsync(new List<SubmitResult>(1) { submit });
        }

        /// <summary>
        /// Get the status of a number of pending submissions.
        /// </summary>
        /// <param name="submits">The pending submissions.</param>
        /// <returns></returns>
        public async Task<SubmitResponse> GetSubmitStatusAsync(IEnumerable<SubmitResult> submits)
        {
            try
            {
                string query = BuildQueryString(submits);

                // If the request contains invalid parameters, the server will return
                // "400 Bad Request" and we'll end up in the first catch block.
                string response = await WebHelper.SendGet(STATUS_URL, query);

                return parser.ParseSubmitResponse(response);
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

        private SubmitResponse CreateErrorResponse(HttpWebResponse response)
        {
            if (response == null)
            {
                return new SubmitResponse(HttpStatusCode.BadRequest, "Unknown error.");
            }

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var text = reader.ReadToEnd();

                if (parser.CanParse(text))
                {
                    return parser.ParseSubmitResponse(text);
                }

                // TODO: parse error message (JSON).
                return new SubmitResponse(response.StatusCode, text);
            }
        }

        private Stream BuildRequestBody(IEnumerable<SubmitRequest> requests)
        {
            var stream = new MemoryStream();

            int i = 0;

            using (var writer = new StreamWriter(stream, Encoding.Default, 1024, true))
            {
                writer.Write("client=" + Configuration.ClientKey);
                writer.Write("&user=" + userKey);

                foreach (var request in requests)
                {
                    request.WriteQueryString(writer, true, i++);
                }

                writer.Write("&format=" + parser.Format);
            }

            // Reset stream position.
            stream.Seek(0L, SeekOrigin.Begin);

            return stream;
        }

        private string BuildQueryString(IEnumerable<SubmitResult> submits)
        {
            var builder = new StringBuilder();

            builder.AppendFormat("client={0}", Configuration.ClientKey);
            builder.AppendFormat("&format={0}", parser.Format);

            foreach (var submit in submits)
            {
                builder.AppendFormat("&id={0}", submit.Id);
            }

            return builder.ToString();
        }
    }
}
