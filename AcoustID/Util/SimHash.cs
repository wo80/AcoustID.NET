// -----------------------------------------------------------------------
// <copyright file="SimHash.cs" company="">
// Original C++ implementation by Lukas Lalinsky, http://acoustid.org/chromaprint
// </copyright>
// -----------------------------------------------------------------------

namespace AcoustID.Util
{
    /// <summary>
    /// SimHash implementation.
    /// </summary>
    public static class SimHash
    {
        /// <summary>
        /// Generate a single 32-bit hash for an array of integers.
        /// </summary>
        /// <param name="data">Array of 32-bit integers representing the data to be hashed.</param>
        /// <returns>The hash.</returns>
        public static int Compute(int[] data)
        {
            return Compute(data, 0, data.Length);
        }

        /// <summary>
        /// Generate a single 32-bit hash for an array of integers.
        /// </summary>
        /// <param name="data">Array of 32-bit integers representing the data to be hashed.</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>The hash.</returns>
        public static int Compute(int[] data, int start, int end)
        {
            int[] v = new int[32];

            for (int i = 0; i < 32; i++)
            {
                v[i] = 0;
            }

            for (int i = start; i < end; i++)
            {
                uint local_hash = (uint)data[i];
                for (int j = 0; j < 32; j++)
                {
                    v[j] += (local_hash & (1 << j)) == 0 ? -1 : 1;
                }
            }

            uint hash = 0;
            for (int i = 0; i < 32; i++)
            {
                if (v[i] > 0)
                {
                    hash |= (uint)(1 << i);
                }
            }

            return (int)(hash);
        }
    }
}
