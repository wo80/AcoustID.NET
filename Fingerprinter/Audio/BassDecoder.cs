// -----------------------------------------------------------------------
// <copyright file="BassDecoder.cs" company="">
// Christian Woltering, https://github.com/wo80
// </copyright>
// -----------------------------------------------------------------------

namespace Fingerprinter.Audio
{
    /* README

    // You will need Bass.Net.dll, bass.dll and bassmix.dll
    // from http://www.un4seen.com/

    using AcoustID.Chromaprint;
    using System;
    using Un4seen.Bass;
    using Un4seen.Bass.AddOn.Mix;

    /// <summary>
    /// Decode using the Bass.Net library. Uses Bass to resample the audio, which
    /// is faster than the AcoustId resampling.
    /// </summary>
    public class BassDecoder : AudioDecoder
    {
        int bassStream;
        int bassMixer;

        string file;

        bool resample;

        public BassDecoder(string file)
            : this(file, false)
        {
        }

        public BassDecoder(string file, bool resample)
        {
            this.file = file;
            this.resample = resample;

            // Open the BASS stream and keep it open until Dispose() is called. This might lock
            // the file. A better approach would be to open the stream only when needed.
            Initialize();
        }

        /// <summary>
        /// Initialize Bass (call this on program startup).
        /// </summary>
        public static void InitializeBass()
        {
            BassNet.Registration("your@mail.com", "YOURBASSKEY");

            try
            {
                // Load BASS (will throw if anything goes wrong, for example missing dlls).
                Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            }
            catch (Exception)
            {
                throw new NotSupportedException("BASS init failed.");
            }
        }

        /// <summary>
        /// Decode an audio file.
        /// </summary>
        public override bool Decode(IAudioConsumer consumer, int maxLength)
        {
            if (bassStream == 0)
            {
                return false;
            }

            // Get the right stream:
            int stream = this.resample ? bassMixer : bassStream;

            int remaining, size, length;
            short[] data = new short[BUFFER_SIZE];

            // Samples to read to get maxLength seconds of audio
            remaining = maxLength * this.sampleRate * this.channels;

            // Bytes to read
            length = 2 * Math.Min(remaining, BUFFER_SIZE);

            while (Bass.BASS_ChannelIsActive(stream) == BASSActive.BASS_ACTIVE_PLAYING)
            {
                size = Bass.BASS_ChannelGetData(stream, data, length);
                if (size > 0)
                {
                    consumer.Consume(data, size / 2);

                    remaining -= size / 2;
                    if (remaining <= 0)
                    {
                        break;
                    }
                    length = 2 * Math.Min(remaining, BUFFER_SIZE);
                }
            }

            return true;
        }

        private void Initialize()
        {
            // Create a stream channel from a file (use BASS_STREAM_PRESCAN for mp3?)
            bassStream = Bass.BASS_StreamCreateFile(file, 0L, 0L, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_ASYNCFILE);

            if (bassStream != 0)
            {
                var info = Bass.BASS_ChannelGetInfo(bassStream);

                this.sampleRate = info.freq;
                this.channels = info.chans;

                double duration = Bass.BASS_ChannelBytes2Seconds(bassStream, Bass.BASS_ChannelGetLength(bassStream));

                this.Format = new AudioProperties(info.freq, info.Is8bit ? 8 : (info.Is32bit ? 32 : 16),
                    info.chans, (int)duration);

                if (this.Format.BitDepth != 16)
                {
                    Dispose(true);
                    return;
                }

                if (this.resample)
                {
                    this.sampleRate = 11025;
                    this.channels = 1;

                    // Create resample stream.
                    bassMixer = BassMix.BASS_Mixer_StreamCreate(this.sampleRate, this.channels,
                        BASSFlag.BASS_MIXER_END | BASSFlag.BASS_STREAM_DECODE);

                    if (bassMixer != 0)
                    {
                        BassMix.BASS_Mixer_StreamAddChannel(bassMixer, bassStream, 0);
                    }
                }
            }
        }

        #region IDisposable implementation

        private bool hasDisposed = false;

        public override void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!hasDisposed)
            {
                if (bassStream != 0)
                {
                    Bass.BASS_StreamFree(bassStream);
                    bassStream = 0;
                }

                if (this.resample && bassMixer != 0)
                {
                    Bass.BASS_StreamFree(bassMixer);
                    bassMixer = 0;
                }
            }

            hasDisposed = disposing;
        }

        ~BassDecoder()
        {
            Dispose(true);

            Bass.BASS_Free();
        }

        #endregion
    }

    // */
}
