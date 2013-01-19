// -----------------------------------------------------------------------
// <copyright file="IDecoder.cs" company="">
// Original C++ implementation by Lukas Lalinsky, http://acoustid.org/chromaprint
// </copyright>
// -----------------------------------------------------------------------

namespace AcoustID.Audio
{
    using AcoustID.Chromaprint;

    /// <summary>
    /// Interface for audio decoders.
    /// </summary>
    public interface IDecoder
    {
        int SampleRate { get; }
        int BitsPerSample { get; }
        int Channels { get; }

        bool Decode(IAudioConsumer consumer, int maxLength);
    }
}
