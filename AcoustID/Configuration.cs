
namespace AcoustID
{
    using System;
    using System.Net;

    /// <summary>
    /// Static configuration class.
    /// </summary>
    public static class Configuration
    {
        static Configuration()
        {
            ClientKey = String.Empty;

            UserAgent = "AcoustID.NET/" + ChromaContext.GetVersion();
            Proxy = null;
        }

        /// <summary>
        /// The client API key for using the AcoustID webservice.
        /// </summary>
        /// <remarks>
        /// Visit https://acoustid.org/new-application to get an API key for your application.
        /// </remarks>
        public static string ClientKey { get; set; }

        /// <summary>
        /// Gets or sets the user-agent string.
        /// </summary>
        public static string UserAgent { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="System.Net.IWebProxy"/> used to call the webservice.
        /// </summary>
        public static IWebProxy Proxy { get; set; }
    }
}
