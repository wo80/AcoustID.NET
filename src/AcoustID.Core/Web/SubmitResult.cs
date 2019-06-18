// -----------------------------------------------------------------------
// <copyright file="SubmitResult.cs" company="">
// Christian Woltering, https://github.com/wo80
// </copyright>
// -----------------------------------------------------------------------

namespace AcoustID.Web
{
    /// <summary>
    /// Result of a submit request.
    /// </summary>
    public class SubmitResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitResult" /> class.
        /// </summary>
        /// <param name="id">The id of the submit</param>
        public SubmitResult(int id)
            : this(id, 0, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitResult" /> class.
        /// </summary>
        /// <param name="id">The id of the submit</param>
        /// <param name="index">The index of the submit (only for batch submits).</param>
        /// <param name="status">The status of the submit (pending or imported).</param>
        /// <param name="acoustId">The AcoustID assigned of the submit.</param>
        public SubmitResult(int id, int index, string status, string acoustId)
        {
            this.Id = id;
            this.Index = index;
            this.Status = status;
            this.AcoustId = acoustId;
        }

        /// <summary>
        /// Gets the id of the submit.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the index of the submit (for batch submits).
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Gets the status of the submit (pending or imported).
        /// </summary>
        public string Status { get; private set; }

        /// <summary>
        /// Gets the assigned AcoustId of the submit (available if status is "imported").
        /// </summary>
        public string AcoustId { get; private set; }
    }
}
