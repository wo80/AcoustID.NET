// -----------------------------------------------------------------------
// <copyright file="XmlResponseParser.cs" company="">
// Christian Woltering, https://github.com/wo80
// </copyright>
// -----------------------------------------------------------------------

namespace AcoustID.Web
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Xml.Linq;

    /// <summary>
    /// Parses lookup and submit responses from the webservice (XML format).
    /// </summary>
    /// <remarks>
    /// The parser will parse lookup responses that were requested using the ["recording"]
    /// or ["recording", "releasegroup"] metadata parameters. If you need other metadata,
    /// you will have to implement your own parser.
    /// </remarks>
    public class XmlResponseParser : IResponseParser
    {
        private static NumberFormatInfo numberFormat = CultureInfo.InvariantCulture.NumberFormat;
        private static string format = "xml";

        /// <inheritdoc />
        public string Format
        {
            get { return format; }
        }

        /// <inheritdoc />
        public bool CanParse(string text)
        {
            return !string.IsNullOrEmpty(text) && text.StartsWith("<?xml");
        }

        /// <inheritdoc />
        public LookupResponse ParseLookupResponse(string text)
        {
            try
            {
                var root = XDocument.Parse(text).Element("response");

                var status = root.Element("status");

                if (status.Value == "ok")
                {
                    var response = new LookupResponse();

                    var list = root.Element("results").Descendants("result");

                    foreach (var item in list)
                    {
                        response.Results.Add(ParseLookupResult(item));
                    }

                    return response;
                }

                if (status.Value == "error")
                {
                    var error = root.Element("error");

                    return new LookupResponse(HttpStatusCode.BadRequest, error.Element("message").Value);
                }

                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Parse the response of a submit request.
        /// </summary>
        /// <param name="text">The response string.</param>
        /// <returns>List of submit results.</returns>
        public SubmitResponse ParseSubmitResponse(string text)
        {
            try
            {
                var root = XDocument.Parse(text).Element("response");

                var status = root.Element("status");

                if (status.Value == "ok")
                {
                    var response = new SubmitResponse();

                    var list = root.Element("submissions").Descendants("submission");

                    foreach (var item in list)
                    {
                        response.Results.Add(ParseSubmitResult(item));
                    }

                    return response;
                }

                if (status.Value == "error")
                {
                    var error = root.Element("error");

                    return new SubmitResponse(HttpStatusCode.BadRequest, error.Element("message").Value);
                }

                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #region Lookup response

        private LookupResult ParseLookupResult(XElement el)
        {
            double score = 0.0;
            string id = string.Empty;

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

        private Recording ParseRecording(XElement node)
        {
            int duration;
            string id, title;

            TryParseChild(node, "id", out id);
            TryParseChild(node, "title", out title);
            TryParseChild(node, "duration", 0, out duration);
            
            var recording = new Recording(duration, id, title);

            var e = node.Element("artists");

            if (e != null)
            {
                var list = e.Elements("artist");

                foreach (var item in list)
                {
                    recording.Artists.Add(ParseArtist(item));
                }
            }

            e = node.Element("releasegroups");

            if (e != null)
            {
                var list = e.Elements("releasegroup");

                foreach (var item in list)
                {
                    recording.ReleaseGroups.Add(ParseReleaseGroup(item));
                }
            }

            e = node.Element("releases");

            if (e != null)
            {
                var list = e.Elements("release");

                foreach (var item in list)
                {
                    recording.Releases.Add(ParseRelease(item));
                }
            }

            // TODO: parse more meta
            return recording;
        }

        private Artist ParseArtist(XElement node)
        {
            string id, name;

            TryParseChild(node, "id", out id);
            TryParseChild(node, "name", out name);
            
            return new Artist(id, name);
        }

        private ReleaseGroup ParseReleaseGroup(XElement node)
        {
            string id, type, title;

            TryParseChild(node, "id", out id);
            TryParseChild(node, "type", out type);
            TryParseChild(node, "title", out title);
            
            var releasegroup = new ReleaseGroup(id, title, type);

            var e = node.Element("artists");

            if (e != null)
            {
                var list = e.Elements("artist");

                foreach (var item in list)
                {
                    releasegroup.Artists.Add(ParseArtist(item));
                }
            }

            return releasegroup;
        }


        private Release ParseRelease(XElement node)
        {
            int tracks;
            string id, title, country;

            TryParseChild(node, "id", out id);
            TryParseChild(node, "title", out title);
            TryParseChild(node, "country", out country);
            TryParseChild(node, "track_count", 0, out tracks);
            
            var e = node.Element("date");

            var date = DateTime.MinValue;

            if (e != null)
            {
                int year, month, day;

                TryParseChild(e, "year", 1, out year);
                TryParseChild(e, "month", 1, out month);
                TryParseChild(e, "day", 1, out day);

                date = new DateTime(year, month, day);
            }

            var release = new Release(id, title, country, date, tracks);

            e = node.Element("artists");

            if (e != null)
            {
                var list = e.Elements("artist");

                foreach (var item in list)
                {
                    release.Artists.Add(ParseArtist(item));
                }
            }

            return release;
        }

        #endregion

        #region Submit response

        private SubmitResult ParseSubmitResult(XElement el)
        {
            int id = 0, index = 0;

            XElement element = el.Element("id");

            if (element != null)
            {
                id = int.Parse(element.Value);
            }

            element = el.Element("index");

            if (element != null)
            {
                index = int.Parse(element.Value);
            }

            string status = null, acoustId = null;

            element = el.Element("status");

            if (element != null)
            {
                status = element.Value;
            }

            element = el.Element("result");

            if (element != null)
            {
                acoustId = element.Element("id").Value;
            }

            return new SubmitResult(id, index, status, acoustId);
        }

        #endregion
        
        #region Helper

        private void TryParseChild(XElement node, string name, out string value)
        {
            value = string.Empty;

            var e = node.Element(name);

            if (e != null)
            {
                value = e.Value;
            }
        }

        private void TryParseChild(XElement node, string name, int defaultValue, out int value)
        {
            value = defaultValue;

            var e = node.Element(name);

            if (e != null)
            {
                int.TryParse(e.Value, out value);
            }
        }

        #endregion
    }
}
