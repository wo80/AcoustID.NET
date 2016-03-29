// -----------------------------------------------------------------------
// <copyright file="SubmitRequest.cs" company="">
// Christian Woltering, https://github.com/wo80
// </copyright>
// -----------------------------------------------------------------------

namespace AcoustID.Web
{
    using System;
    using System.Text;

    /// <summary>
    /// Submit request data.
    /// </summary>
    public class SubmitRequest
    {
        /// <summary>
        /// Gets or sets the duration of the whole audio file in seconds.
        /// </summary>
        public int Duration { get; private set; }

        /// <summary>
        /// Gets or sets the audio fingerprint data.
        /// </summary>
        public string Fingerprint { get; private set; }

        /// <summary>
        /// Gets or sets the bitrate of the audio file.
        /// </summary>
        public int Bitrate { get; set; }

        /// <summary>
        /// Gets or sets the file format of the audio file.
        /// </summary>
        public string FileFormat { get; set; }

        /// <summary>
        /// Gets or sets the corresponding MusicBrainz recording ID.
        /// </summary>
        public string MBID { get; set; }

        /// <summary>
        /// Gets or sets the track title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the track artist.
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// Gets or sets the album title.
        /// </summary>
        public string Album { get; set; }

        /// <summary>
        /// Gets or sets the album artist.
        /// </summary>
        public string AlbumArtist { get; set; }

        /// <summary>
        /// Gets or sets the album release year.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the track number.
        /// </summary>
        public int TrackNumber { get; set; }

        /// <summary>
        /// Gets or sets the disc number.
        /// </summary>
        public int DiscNumber { get; set; }

        public SubmitRequest(string fingerprint, int duration)
        {
            this.Fingerprint = fingerprint;
            this.Duration = duration;
        }

        /// <summary>
        /// Create a query string for the request.
        /// </summary>
        /// <param name="index">The batch index.</param>
        /// <returns></returns>
        public string ToQueryString(int index = -1)
        {
            if (Duration <= 0)
            {
                throw new Exception("Missing submit parameter: duration");
            }

            if (string.IsNullOrEmpty(Fingerprint))
            {
                throw new Exception("Missing submit parameter: fingerprint");
            }

            var builder = new StringBuilder();

            var batch = string.Empty;

            if (index >= 0)
            {
                batch = "." + index;
            }

            builder.AppendFormat("duration{0}={1}", batch, Duration);
            builder.AppendFormat("&fingerprint{0}={1}", batch, Fingerprint);

            if (Bitrate > 0)
            {
                builder.AppendFormat("&bitrate{0}={1}", batch, Bitrate);
            }

            if (!string.IsNullOrWhiteSpace(FileFormat))
            {
                builder.AppendFormat("&fileformat{0}={1}", batch, FileFormat);
            }

            if (!string.IsNullOrWhiteSpace(MBID))
            {
                builder.AppendFormat("&mbid{0}={1}", batch, MBID);
            }

            if (!string.IsNullOrWhiteSpace(Title))
            {
                builder.AppendFormat("&track{0}={1}", batch, Title);
            }

            if (!string.IsNullOrWhiteSpace(Artist))
            {
                builder.AppendFormat("&artist{0}={1}", batch, Uri.EscapeUriString(Artist));
            }

            if (!string.IsNullOrWhiteSpace(Album))
            {
                builder.AppendFormat("&album{0}={1}", batch, Uri.EscapeUriString(Album));
            }

            if (!string.IsNullOrWhiteSpace(AlbumArtist))
            {
                builder.AppendFormat("&albumartist{0}={1}", batch, Uri.EscapeUriString(AlbumArtist));
            }

            if (Year > 0)
            {
                builder.AppendFormat("&year{0}={1}", batch, Year);
            }

            if (TrackNumber > 0)
            {
                builder.AppendFormat("&trackno{0}={1}", batch, TrackNumber);
            }

            if (DiscNumber > 0)
            {
                builder.AppendFormat("&discno{0}={1}", batch, DiscNumber);
            }

            return builder.ToString();
        }
    }
}
