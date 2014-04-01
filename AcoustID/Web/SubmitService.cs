// -----------------------------------------------------------------------
// <copyright file="SubmitService.cs" company="">
// Christian Woltering, https://github.com/wo80
// </copyright>
// -----------------------------------------------------------------------

namespace AcoustID.Web
{
    using System;

    /// <summary>
    /// Calls the AcoustId webservice to submit a new fingerprint.
    /// </summary>
    public class SubmitService
    {
        static string URL = "http://api.acoustid.org/v2/submit";

        IResponseParser parser;

        /// <summary>
        /// Gets or sets if gzip compression is used to compress data before submit.
        /// </summary>
        public bool CompressData { get; set; }

        public SubmitService()
            : this(new XmlResponseParser())
        {
        }

        public SubmitService(IResponseParser parser)
        {
            this.parser = parser;

            CompressData = true;
        }

        // TODO: implement submit
    }
}
