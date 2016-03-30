// -----------------------------------------------------------------------
// <copyright file="SubmitRequest.cs" company="">
// Christian Woltering, https://github.com/wo80
// </copyright>
// -----------------------------------------------------------------------

namespace AcoustID.Web
{
    using System;
    using System.IO;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitRequest" /> class.
        /// </summary>
        /// <param name="fingerprint">The audio fingerprint.</param>
        /// <param name="duration">The duration (in seconds).</param>
        public SubmitRequest(string fingerprint, int duration)
        {
            this.Fingerprint = fingerprint;
            this.Duration = duration;
        }

        /// <summary>
        /// Write the query string to given stream writer.
        /// </summary>
        /// <param name="writer">The stream writer.</param>
        /// <param name="append">If true, an ampersand will be prepended.</param>
        /// <param name="index">The batch index.</param>
        /// <returns></returns>
        public void WriteQueryString(StreamWriter writer, bool append = true, int index = -1)
        {
            if (Duration <= 0)
            {
                throw new Exception("Missing submit parameter: duration");
            }

            if (string.IsNullOrEmpty(Fingerprint))
            {
                throw new Exception("Missing submit parameter: fingerprint");
            }

            var batch = string.Empty;

            if (index >= 0)
            {
                batch = "." + index;
            }

            if (append)
            {
                writer.Write("&");
            }

            writer.Write("duration{0}={1}", batch, Duration);
            writer.Write("&fingerprint{0}={1}", batch, Fingerprint);

            if (Bitrate > 0)
            {
                writer.Write("&bitrate{0}={1}", batch, Bitrate);
            }

            if (!string.IsNullOrWhiteSpace(FileFormat))
            {
                writer.Write("&fileformat{0}={1}", batch, FileFormat);
            }

            if (!string.IsNullOrWhiteSpace(MBID))
            {
                writer.Write("&mbid{0}={1}", batch, MBID);
            }

            if (!string.IsNullOrWhiteSpace(Title))
            {
                writer.Write("&track{0}={1}", batch, Title);
            }

            if (!string.IsNullOrWhiteSpace(Artist))
            {
                writer.Write("&artist{0}={1}", batch, Uri.EscapeUriString(Artist));
            }

            if (!string.IsNullOrWhiteSpace(Album))
            {
                writer.Write("&album{0}={1}", batch, Uri.EscapeUriString(Album));
            }

            if (!string.IsNullOrWhiteSpace(AlbumArtist))
            {
                writer.Write("&albumartist{0}={1}", batch, Uri.EscapeUriString(AlbumArtist));
            }

            if (Year > 0)
            {
                writer.Write("&year{0}={1}", batch, Year);
            }

            if (TrackNumber > 0)
            {
                writer.Write("&trackno{0}={1}", batch, TrackNumber);
            }

            if (DiscNumber > 0)
            {
                writer.Write("&discno{0}={1}", batch, DiscNumber);
            }
        }
    }
}
