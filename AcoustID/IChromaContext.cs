
namespace AcoustID
{
    using AcoustID.Audio;

    /// <summary>
    /// The basic Chromaprint interface.
    /// </summary>
    public interface IChromaContext : IAudioConsumer
    {
        /// <summary>
        /// Gets the fingerprint algorithm the context is configured to use.
        /// </summary>
        int Algorithm { get; }

        /// <summary>
        /// Return the version number of Chromaprint.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Restart the computation of a fingerprint with a new audio stream
        /// </summary>
        /// <param name="sample_rate">sample rate of the audio stream (in Hz)</param>
        /// <param name="num_channels">numbers of channels in the audio stream (1 or 2)</param>
        /// <returns>False on error, true on success</returns>
        bool Start(int sample_rate, int num_channels);

        /// <summary>
        /// Send audio data to the fingerprint calculator.
        /// </summary>
        /// <param name="data">raw audio data, should point to an array of 16-bit 
        /// signed integers in native byte-order</param>
        /// <param name="size">size of the data buffer (in samples)</param>
        void Feed(short[] data, int size);

        /// <summary>
        /// Process any remaining buffered audio data and calculate the fingerprint.
        /// </summary>
        void Finish();

        /// <summary>
        /// Return the calculated fingerprint as a compressed string.
        /// </summary>
        /// <returns>The fingerprint as a compressed string</returns>
        string GetFingerprint();

        /// <summary>
        /// Return the calculated fingerprint as an array of 32-bit integers.
        /// </summary>
        /// <returns>The raw fingerprint (array of 32-bit integers)</returns>
        int[] GetRawFingerprint();

        /// <summary>
        /// Return 32-bit hash of the calculated fingerprint.
        /// </summary>
        /// <returns>The hash.</returns>
        int GetFingerprintHash();
    }
}
