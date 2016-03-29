// -----------------------------------------------------------------------
// <copyright file="SubmitService.cs" company="">
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
    /// Calls the AcoustId webservice to submit a new fingerprint.
    /// </summary>
    public class SubmitService
    {
        private const string URL = "http://api.acoustid.org/v2/submit";

        private IResponseParser parser;

        private string userKey;

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="userKey">The user API key.</param>
        /// <param name="parser"></param>
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

        // TODO: add a method to submit multiple fingerprints at once.

        /// <summary>
        /// Calls the webservice.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SubmitResponse> SubmitAsync(SubmitRequest request)
        {
            try
            {
                string query = BuildRequestString(request);

                // If the request contains invalid parameters, the server will return "400 Bad Request" and
                // we'll end up in the first catch block.
                string response = await WebHelper.SendPost(URL, query, UseCompression);

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

        private string BuildRequestString(SubmitRequest request)
        {
            StringBuilder query = new StringBuilder();

            query.Append("client=" + Configuration.ClientKey);
            query.Append("&user=" + userKey);
            query.Append("&" + request.ToQueryString());
            query.Append("&format=" + parser.Format);

            return query.ToString();
        }
    }
}
