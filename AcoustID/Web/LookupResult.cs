// -----------------------------------------------------------------------
// <copyright file="LookupResult.cs" company="">
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
    /// Result of a lookup request.
    /// </summary>
    public class LookupResult
    {
        public string Id { get; private set; }
        public double Score { get; private set; }

        public List<Recording> Recordings { get; private set; }

        public LookupResult(string id, double score)
        {
            this.Id = id;
            this.Score = score;

            this.Recordings = new List<Recording>();
        }
    }
}
