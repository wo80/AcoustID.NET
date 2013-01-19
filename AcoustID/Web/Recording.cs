// -----------------------------------------------------------------------
// <copyright file="Recording.cs" company="">
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
    /// Represents a recording.
    /// </summary>
    public class Recording
    {
        public int Duration { get; private set; }
        public string Id { get; private set; }
        public string Title { get; private set; }

        public List<Artist> Artists { get; private set; }
        public List<ReleaseGroup> ReleaseGroups { get; private set; }

        public Recording(int duration, string id, string title)
        {
            this.Duration = duration;
            this.Id = id;
            this.Title = title;

            this.Artists = new List<Artist>();
            this.ReleaseGroups = new List<ReleaseGroup>();
        }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
