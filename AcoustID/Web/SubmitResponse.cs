// -----------------------------------------------------------------------
// <copyright file="SubmitResponse.cs" company="">
// Christian Woltering, https://github.com/wo80
// </copyright>
// -----------------------------------------------------------------------

namespace AcoustID.Web
{
    using System.Collections.Generic;
    using System.Net;

    /// <summary>
    /// The webservice response containing the status code and a list of submit results.
    /// </summary>
    public class SubmitResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitResponse" /> class.
        /// </summary>
        public SubmitResponse()
            : this(HttpStatusCode.OK, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitResponse" /> class.
        /// </summary>
        /// <param name="status">The HTTP status code.</param>
        /// <param name="error">The error message.</param>
        public SubmitResponse(HttpStatusCode status, string error)
        {
            this.StatusCode = status;
            this.ErrorMessage = error;

            Results = new List<SubmitResult>();
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
        /// Gets a list of <see cref="SubmitResult"/>s.
        /// </summary>
        public List<SubmitResult> Results { get; private set; }
    }
}
