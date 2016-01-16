using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprinter.Audio
{
    public class AudioProperties
    {
        /// <summary>
        /// Gets the sample rate of the audio source.
        /// </summary>
        public int SampleRate { get; private set; }

        /// <summary>
        /// Gets the sample rate of the audio source (must be 16 bits per sample).
        /// </summary>
        public int BitDepth { get; private set; }

        /// <summary>
        /// Gets the number of channels.
        /// </summary>
        public int Channels { get; private set; }

        /// <summary>
        /// Gets the duration of the audio source (in seconds).
        /// </summary>
        public int Duration { get; private set; }

        public AudioProperties()
            : this(0, 0, 0, 0)
        {
        }

        public AudioProperties(int sampleRate, int bitDepth, int channels, int duration)
        {
            this.SampleRate = sampleRate;
            this.BitDepth = bitDepth;
            this.Channels = channels;
            this.Duration = duration;
        }
    }
}
