// -----------------------------------------------------------------------
// <copyright file="IAudioConsumer.cs" company="">
// Original C++ implementation by Lukas Lalinsky, http://acoustid.org/chromaprint
// </copyright>
// -----------------------------------------------------------------------

namespace AcoustID.Audio
{
    /// <summary>
    /// Consumer for 16bit audio data buffer.
    /// </summary>
    public interface IAudioConsumer
    {
        /// <summary>
        /// Consume audio data.
        /// </summary>
        /// <param name="input">The audio data.</param>
        /// <param name="length">The number of samples to consume.</param>
        void Consume(short[] input, int length);
    }
}
