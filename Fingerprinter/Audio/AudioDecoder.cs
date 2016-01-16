// -----------------------------------------------------------------------
// <copyright file="AudioDecoder.cs" company="">
// Christian Woltering, https://github.com/wo80
// </copyright>
// -----------------------------------------------------------------------

namespace Fingerprinter.Audio
{
    using AcoustID.Chromaprint;

    /// <summary>
    /// Abstract base class for audio decoders
    /// </summary>
    public abstract class AudioDecoder : IAudioDecoder
    {
        protected static readonly int BUFFER_SIZE = 2 * 192000;

        protected int sampleRate;
        protected int channels;

        public int SampleRate
        {
            get { return sampleRate; }
        }

        public int Channels
        {
            get { return channels; }
        }

        public AudioProperties Format { get; protected set; }

        public abstract bool Decode(IAudioConsumer consumer, int maxLength);

        public virtual void Dispose()
        {
        }
    }
}
