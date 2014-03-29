// -----------------------------------------------------------------------
// <copyright file="NAudioDecoder.cs" company="">
// Christian Woltering, https://github.com/wo80
// </copyright>
// -----------------------------------------------------------------------

namespace Fingerprinter.Audio
{
    using System;
    using System.IO;
    using AcoustID.Chromaprint;
    using NAudio.Wave;

    /// <summary>
    /// Decode using the NAudio library. Great audio library, but the MP3 decoder is kinda slow.
    /// </summary>
    public class NAudioDecoder : AudioDecoder
    {
        WaveStream reader;
        string extension;
        
        public override void Load(string file)
        {
            // Dispose on every new load
            Dispose(false);

            ready = false;

            extension = Path.GetExtension(file).ToLowerInvariant();

            if (extension.Equals(".wav"))
            {
                reader = new WaveFileReader(file);
            }
            else
            {
                reader = new Mp3FileReader(file);
            }

            var format = reader.WaveFormat;

            this.sampleRate = format.SampleRate;
            this.channels = format.Channels;

            this.sourceSampleRate = format.SampleRate;
            this.sourceBitDepth = format.BitsPerSample;
            this.sourceChannels = format.Channels;
            this.duration = (int)reader.TotalTime.TotalSeconds;
            this.ready = (format.BitsPerSample == 16);
        }

        public override bool Decode(IAudioConsumer consumer, int maxLength)
        {
            if (!ready) return false;

            int remaining, length, size;
            byte[] buffer = new byte[2 * BUFFER_SIZE];
            short[] data = new short[BUFFER_SIZE];

            // Samples to read to get maxLength seconds of audio
            remaining = maxLength * this.sourceChannels * this.sampleRate;

            // Bytes to read
            length = 2 * Math.Min(remaining, BUFFER_SIZE);

            while ((size = reader.Read(buffer, 0, length)) > 0)
            {
                Buffer.BlockCopy(buffer, 0, data, 0, size);

                consumer.Consume(data, size / 2);

                remaining -= size / 2;
                if (remaining <= 0)
                {
                    break;
                }

                length = 2 * Math.Min(remaining, BUFFER_SIZE);
            }

            return true;
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
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }

            hasDisposed = disposing;
        }

        ~NAudioDecoder()
        {
            Dispose(true);
        }

        #endregion
    }
}
