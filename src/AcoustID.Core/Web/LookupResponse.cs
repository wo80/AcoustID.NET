// -----------------------------------------------------------------------
// <copyright file="LookupResponse.cs" company="">
// Christian Woltering, https://github.com/wo80
// </copyright>
// -----------------------------------------------------------------------

namespace AcoustID.Web
{
    using System.Collections.Generic;
    using System.Net;

    /// <summary>
    /// The webservice response containing the status code and a list of lookup results.
    /// </summary>
    public class LookupResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LookupResponse" /> class.
        /// </summary>
        public LookupResponse()
            : this(HttpStatusCode.OK, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupResponse" /> class.
        /// </summary>
        /// <param name="status">The HTTP status code.</param>
        /// <param name="error">The error message.</param>
        public LookupResponse(HttpStatusCode status, string error)
        {
            this.StatusCode = status;
            this.ErrorMessage = error;

            Results = new List<LookupResult>();
        }

        /// <summary>
        /// Gets the status code returned by the webservice.
        /// </summary>
        public HttpStatusCode StatusCode { get; internal set; }

        /// <summary>
        /// Gets the error message, in case the status code is not "200 OK".
        /// </summary>
        public string ErrorMessage { get; internal set; }

        /// <summary>
        /// Gets a list of <see cref="LookupResult"/>s.
        /// </summary>
        public List<LookupResult> Results { get; private set; }
    }
}
