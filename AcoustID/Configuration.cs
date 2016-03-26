
namespace AcoustID
{
    using System;
    using System.Net;

    public class Configuration
    {
        static Configuration()
        {
            UserAgent = "AcoustId.NET/" + ChromaContext.Version;
            Proxy = null;
        }

        /// <summary>
        /// The API key for using the AcoustID webservice.
        /// </summary>
        /// <remarks>
        /// Visit https://acoustid.org/ to get an API key for your application.
        /// </remarks>
        public static string ApiKey = String.Empty;

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
