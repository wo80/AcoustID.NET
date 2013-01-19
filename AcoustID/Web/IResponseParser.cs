// -----------------------------------------------------------------------
// <copyright file="IResponseParser.cs" company="">
// Christian Woltering, https://github.com/wo80
// </copyright>
// -----------------------------------------------------------------------

namespace AcoustID.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Parse the response of a AcoustId lookup or submit request.
    /// </summary>
    public interface IResponseParser
    {
        string Format { get; }
        string Error { get; }

        List<LookupResult> ParseLookupResponse(string response);
        List<SubmitResult> ParseSubmitResponse(string response);
    }
}
