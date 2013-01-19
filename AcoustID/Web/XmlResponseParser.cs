// -----------------------------------------------------------------------
// <copyright file="XmlResponseParser.cs" company="">
// Christian Woltering, https://github.com/wo80
// </copyright>
// -----------------------------------------------------------------------

namespace AcoustID.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using System.Globalization;

    /// <summary>
    /// Parses lookup and submit responses from the webservice (XML format).
    /// </summary>
    public class XmlResponseParser : IResponseParser
    {
        static NumberFormatInfo numberFormat = CultureInfo.InvariantCulture.NumberFormat;
        static string format = "xml";

        public string Format
        {
            get { return format; }
        }

        /// <summary>
        /// Gets the last error message.
        /// </summary>
        public string Error { get; private set; }

        /// <summary>
        /// Parse the response of a lookup request.
        /// </summary>
        /// <param name="response">The response string.</param>
        /// <returns>List of lookup results.</returns>
        public List<LookupResult> ParseLookupResponse(string response)
        {
            try
            {
                this.Error = String.Empty;

                var root = XDocument.Parse(response).Element("response");

                var status = root.Element("status");

                List<LookupResult> results = new List<LookupResult>();

                if (status.Value == "ok")
                {
                    var list = root.Element("results").Descendants("result");

                    foreach (var item in list)
                    {
                        results.Add(ParseLookupResult(item));
                    }
                }
                else if (status.Value == "error")
                {
                    var error = root.Element("error");

                    this.Error = error.Element("message").Value;
                }

                return results;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Parse the response of a submit request.
        /// </summary>
        /// <param name="response">The response string.</param>
        /// <returns>List of submit results.</returns>
        public List<SubmitResult> ParseSubmitResponse(string response)
        {
            // TODO: implement submit response parsing
            throw new NotImplementedException();
        }

        #region Lookup response

        private LookupResult ParseLookupResult(XElement el)
        {
            double score = 0.0;
            string id = String.Empty;

            XElement element = el.Element("score");

            if (element != null)
            {
                score = double.Parse(element.Value, numberFormat);
            }

            element = el.Element("id");

            if (element != null)
            {
                id = element.Value;
            }

            LookupResult result = new LookupResult(id, score);

            element = el.Element("recordings");

            if (element != null)
            {
                var recordings = element.Elements("recording");

                foreach (var recording in recordings)
                {
                    result.Recordings.Add(ParseRecording(recording));
                }
            }

            return result;
        }

        private Recording ParseRecording(XElement el)
        {
            int duration = 0;
            string id = String.Empty;
            string title = String.Empty;

            XElement element = el.Element("duration");

            if (element != null)
            {
                duration = int.Parse(element.Value);
            }

            element = el.Element("id");

            if (element != null)
            {
                id = element.Value;
            }

            element = el.Element("title");

            if (element != null)
            {
                title = element.Value;
            }

            var recording = new Recording(duration, id, title);

            element = el.Element("artists");

            if (element != null)
            {
                var list = element.Elements("artist");

                foreach (var item in list)
                {
                    recording.Artists.Add(ParseArtist(item));
                }
            }

            element = el.Element("releasegroups");

            if (element != null)
            {
                var list = element.Elements("releasegroup");

                foreach (var item in list)
                {
                    recording.ReleaseGroups.Add(ParseReleaseGroup(item));
                }
            }

            // TODO: parse more meta
            return recording;
        }

        private Artist ParseArtist(XElement el)
        {
            string id = String.Empty;
            string name = String.Empty;

            XElement element = el.Element("name");

            if (element != null)
            {
                name = element.Value;
            }

            element = el.Element("id");

            if (element != null)
            {
                id = element.Value;
            }

            return new Artist(id, name);
        }

        private ReleaseGroup ParseReleaseGroup(XElement el)
        {
            string id = String.Empty;
            string title = String.Empty;
            string type = String.Empty;

            XElement element = el.Element("id");

            if (element != null)
            {
                id = element.Value;
            }

            element = el.Element("title");

            if (element != null)
            {
                title = element.Value;
            }

            element = el.Element("type");

            if (element != null)
            {
                type = element.Value;
            }

            var releasegroup = new ReleaseGroup(id, title, type);

            element = el.Element("artists");

            if (element != null)
            {
                var list = element.Elements("artist");

                foreach (var item in list)
                {
                    releasegroup.Artists.Add(ParseArtist(item));
                }
            }

            return releasegroup;
        }

        #endregion
    }
}
