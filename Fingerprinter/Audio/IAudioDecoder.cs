// -----------------------------------------------------------------------
// <copyright file="IAudioDecoder.cs" company="">
// Christian Woltering, https://github.com/wo80
// </copyright>
// -----------------------------------------------------------------------

namespace Fingerprinter.Audio
{
    using System;
    using AcoustID.Audio;

    /// <summary>
    /// Interface for audio decoders.
    /// </summary>
    public interface IAudioDecoder : IDecoder, IDisposable
    {
        int SourceSampleRate { get; }
        int SourceBitDepth { get; }
        int SourceChannels { get; }

        int Duration { get; }
        bool Ready { get; }
    }
}
