// -----------------------------------------------------------------------
// <copyright file="Decoder.cs" company="">
// Christian Woltering, https://github.com/wo80
// </copyright>
// -----------------------------------------------------------------------

namespace Fingerprinter.Audio
{
    /* README

    // You will need Bass.Net.dll, bass.dll and bassmix.dll
    // from http://www.un4seen.com/

    // WARNING:
    // If you use Bass resampling (like the Decode method of this class does),
    // make sure to set the sample rate for the chroma context correctly to fixed
    // value 11025 (and don't use the property values of this class, which represent
    // the audio of the source file).

    using System;
    using AcoustId.Audio;
    using AcoustId.Chromaprint;
    using Un4seen.Bass;
    using Un4seen.Bass.AddOn.Mix;

    /// <summary>
    /// Decode using the Bass.Net library. Uses Bass to resample the audio, which
    /// is faster than the AcoustId resampling.
    /// </summary>
    public class BassDecoder : IDecoder, IDisposable
    {
        private static readonly int BUFFER_SIZE = 4 * 256 * 256;

        int bassStream;
        int bassMixer;

        int sampleRate;
        int bitsPerSample;
        int channels;
        int duration;

        bool ready;

        public int SampleRate
        {
            get { return sampleRate; }
        }

        public int BitsPerSample
        {
            get { return bitsPerSample; }
        }

        public int Channels
        {
            get { return channels; }
        }

        public int Duration
        {
            get { return duration; }
        }

        public bool Ready
        {
            get { return ready; }
        }

        public BassDecoder(string file)
        {
            ready = false;

            // Let BASS do the resampling.
            if (Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
            {
                // create a stream channel from a file (use BASS_STREAM_PRESCANfor mp3?)
                bassStream = Bass.BASS_StreamCreateFile(file, 0L, 0L, BASSFlag.BASS_STREAM_DECODE);
                if (bassStream != 0)
                {
                    var info = Bass.BASS_ChannelGetInfo(bassStream);

                    sampleRate = info.freq;
                    bitsPerSample = info.Is8bit ? 8 : (info.Is32bit ? 32 : 16);
                    channels = info.chans;

                    duration = (int)Bass.BASS_ChannelBytes2Seconds(bassStream, Bass.BASS_ChannelGetLength(bassStream));

                    ready = (!info.Is8bit && !info.Is32bit);
                }
            }
        }

        /// <summary>
        /// Decode an audio file and let Bass do the resampling to 11025Hz.
        /// </summary>
        /// <remarks>
        /// If you use this method, make sure to always set the chroma context 
        /// parameters accordingly!!!
        /// </remarks>
        public bool Decode(IAudioConsumer consumer, int maxLength)
        {
            if (!ready)
            {
                return false;
            }

            // Resample to 11025Hz. Could also convert to mono her, but this
            // seems to be slower overall ...
            bassMixer = BassMix.BASS_Mixer_StreamCreate(11025, channels,
                BASSFlag.BASS_MIXER_END | BASSFlag.BASS_STREAM_DECODE);

            BassMix.BASS_Mixer_StreamAddChannel(bassMixer, bassStream, 0);

            // Samples to read to get maxLength seconds of audio
            int remaining = maxLength * 11025;
            int size = BUFFER_SIZE;
            short[] data = new short[BUFFER_SIZE / 2];

            while (Bass.BASS_ChannelIsActive(bassMixer) == BASSActive.BASS_ACTIVE_PLAYING)
            {
                int length = Bass.BASS_ChannelGetData(bassMixer, data, size);
                if (length > 0)
                {
                    consumer.Consume(data, size / 2);

                    remaining -= length / 2;
                    if (remaining <= 0)
                    {
                        break;
                    }
                    size = 2 * Math.Min(remaining, BUFFER_SIZE / 2);
                }
            }

            return true;
        }


        public bool Decode_NoResample(IAudioConsumer consumer, int maxLength)
        {
            if (!ready)
            {
                return false;
            }

            // Samples to read to get maxLength seconds of audio
            int remaining = maxLength * sampleRate * channels;
            int size = BUFFER_SIZE;
            short[] data = new short[BUFFER_SIZE / 2];

            while (Bass.BASS_ChannelIsActive(bassStream) == BASSActive.BASS_ACTIVE_PLAYING)
            {
                int length = Bass.BASS_ChannelGetData(bassStream, data, size);
                if (length > 0)
                {
                    consumer.Consume(data, size / 2);

                    remaining -= length / 2;
                    if (remaining <= 0)
                    {
                        break;
                    }
                    size = 2 * Math.Min(remaining, BUFFER_SIZE / 2);
                }
            }

            return true;
        }

        #region IDisposable implementation

        private bool hasDisposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!hasDisposed)
            {
                Bass.BASS_StreamFree(bassStream);
                Bass.BASS_StreamFree(bassMixer);
                Bass.BASS_Free();

                hasDisposed = true;
            }
        }

        ~BassDecoder()
        {
            Dispose(false);
        }

        #endregion
    }

    // */
}
