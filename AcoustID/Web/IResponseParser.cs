// -----------------------------------------------------------------------
// <copyright file="IResponseParser.cs" company="">
// Christian Woltering, https://github.com/wo80
// </copyright>
// -----------------------------------------------------------------------

namespace AcoustID.Web
{
    using System.Collections.Generic;

    /// <summary>
    /// Parse the response of a AcoustId lookup or submit request.
    /// </summary>
    public interface IResponseParser
    {
        /// <summary>
        /// Gets the format of the response parser (must be "xml" or "json").
        /// </summary>
        string Format { get; }

        /// <summary>
        /// Gets an error message (will be null for successful requests).
        /// </summary>
        string Error { get; }

        /// <summary>
        /// Parse the content of a lookup response.
        /// </summary>
        /// <param name="response">The webservice response.</param>
        /// <returns>A list of <see cref="LookupResult"/>.</returns>
        List<LookupResult> ParseLookupResponse(string response);

        /// <summary>
        /// Parse the content of a submit response.
        /// </summary>
        /// <param name="response">The webservice response.</param>
        /// <returns>A list of <see cref="SubmitResult"/>.</returns>
        List<SubmitResult> ParseSubmitResponse(string response);
    }
}
