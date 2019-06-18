// -----------------------------------------------------------------------
// <copyright file="Artist.cs" company="">
// Christian Woltering, https://github.com/wo80
// </copyright>
// -----------------------------------------------------------------------

namespace AcoustID.Web
{
    /// <summary>
    /// Represents an artist.
    /// </summary>
    public class Artist
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Artist" /> class.
        /// </summary>
        /// <param name="id">The MusicBrainz ID.</param>
        /// <param name="name">The artist name.</param>
        public Artist(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        /// <summary>
        /// Gets the MusicBrainz id of the artist.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the name of the artist.
        /// </summary>
        public string Name { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
