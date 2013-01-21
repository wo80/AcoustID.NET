// -----------------------------------------------------------------------
// <copyright file="Decoder.cs" company="">
// Christian Woltering, https://github.com/wo80
// </copyright>
// -----------------------------------------------------------------------

namespace Fingerprinter.Audio
{
    using System;
    using AcoustID.Audio;
    using AcoustID.Chromaprint;
    using NAudio.Wave;
    using System.IO;

    /// <summary>
    /// Decode using the NAudio library. Great audio library, but the MP3 decoder is kinda slow.
    /// </summary>
    public class NAudioDecoder : IDecoder, IDisposable
    {
        private static readonly int BUFFER_SIZE = 2 * 192000;

        WaveStream reader;

        string extension;
        
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

        public NAudioDecoder(string file)
        {
            extension = Path.GetExtension(file).ToLowerInvariant();

            ready = false;

            if (extension.Equals(".wav"))
            {
                reader = new WaveFileReader(file);
            }
            else
            {
                reader = new Mp3FileReader(file);
            }

            var format = reader.WaveFormat;

            bitsPerSample = format.BitsPerSample;
            sampleRate = format.SampleRate;
            channels = format.Channels;
            
            duration = (int)reader.TotalTime.TotalSeconds;

            ready = (format.BitsPerSample == 16);
        }

        public bool Decode(IAudioConsumer consumer, int maxLength)
        {
            if (!ready)
            {
                return false;
            }

            int remaining, length, size;
            byte[] buffer = new byte[2 * BUFFER_SIZE];
            short[] data = new short[BUFFER_SIZE];

            // Samples to read to get maxLength seconds of audio
            remaining = maxLength * Channels * SampleRate;

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!hasDisposed)
            {
                reader.Close();
                reader.Dispose();

                hasDisposed = true;
            }
        }

        ~NAudioDecoder()
        {
            Dispose(false);
        }

        #endregion
    }
}
