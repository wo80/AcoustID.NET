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
        AudioProperties Format { get; }
    }
}
