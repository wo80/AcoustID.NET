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
    /// Decode using the NAudio library.
    /// </summary>
    public class NAudioDecoder : AudioDecoder
    {
        WaveStream reader;
        string file;

        public NAudioDecoder(string file)
        {
            this.file = file;

            // Open the WaveStream and keep it open until Dispose() is called. This might lock
            // the file. A better approach would be to open the stream only when needed.
            Initialize();
        }

        public override bool Decode(IAudioConsumer consumer, int maxLength)
        {
            if (reader == null) return false;

            int remaining, length, size;
            byte[] buffer = new byte[2 * BUFFER_SIZE];
            short[] data = new short[BUFFER_SIZE];

            // Samples to read to get maxLength seconds of audio
            remaining = maxLength * this.Format.Channels * this.sampleRate;

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

        private void Initialize()
        {
            var extension = Path.GetExtension(file).ToLowerInvariant();

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

            this.Format = new AudioProperties(format.SampleRate, format.BitsPerSample,
                format.Channels, (int)reader.TotalTime.TotalSeconds);

            if (format.BitsPerSample != 16)
            {
                Dispose(true);
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
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
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
