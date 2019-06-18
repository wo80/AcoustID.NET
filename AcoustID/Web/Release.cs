
namespace AcoustID.Web
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a release.
    /// </summary>
    public class Release
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Release" /> class.
        /// </summary>
        /// <param name="id">The MusicBrainz ID.</param>
        /// <param name="title">The title of the release.</param>
        /// <param name="country">The country of the release.</param>
        /// <param name="date">The date of the release.</param>
        /// <param name="tracks">The track-count of the release.</param>
        public Release(string id, string title, string country, DateTime date, int tracks)
        {
            this.Id = id;
            this.Title = title;
            this.Country = country;
            this.Date = date;
            this.TrackCount = tracks;

            this.Artists = new List<Artist>();
        }

        /// <summary>
        /// Gets the MusicBrainz id of the release.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the title of the release.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the country of the release.
        /// </summary>
        public string Country { get; private set; }

        /// <summary>
        /// Gets the date of the release.
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Gets the track-count of the release.
        /// </summary>
        public int TrackCount { get; private set; }

        /// <summary>
        /// Gets the artists associated with the release.
        /// </summary>
        public List<Artist> Artists { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
