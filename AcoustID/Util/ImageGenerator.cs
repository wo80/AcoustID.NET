// -----------------------------------------------------------------------
// <copyright file="ImageGenerator.cs" company="">
// Original C++ implementation by Lukas Lalinsky, http://acoustid.org/chromaprint
// </copyright>
// -----------------------------------------------------------------------

namespace AcoustID.Util
{
    using AcoustID.Audio;
    using AcoustID.Chromaprint;

    public static class ImageGenerator
    {
        private const int SAMPLE_RATE = 11025;
        private const int FRAME_SIZE = 4096;
        private const int OVERLAP = FRAME_SIZE - FRAME_SIZE / 3; // 2720
        private const int MIN_FREQ = 28;
        private const int MAX_FREQ = 3520;
        //private const int MAX_FILTER_WIDTH = 20;

        private static double[] ChromaFilterCoefficients = { 0.25, 0.75, 1.0, 0.75, 0.25 };

        /// <summary>
        /// Computes the chromagram of an audio file.
        /// </summary>
        /// <param name="decoder">The <see cref="IDecoder"/> instance.</param>
        /// <returns>Chroma image.</returns>
        public static Image ComputeChromagram(IDecoder decoder)
        {
            var image = new Image(12);
            var image_builder = new ImageBuilder(image);

            var chroma_normalizer = new ChromaNormalizer(image_builder);
            var chroma_filter = new ChromaFilter(ChromaFilterCoefficients, chroma_normalizer);
            var chroma = new Chroma(MIN_FREQ, MAX_FREQ, FRAME_SIZE, SAMPLE_RATE, chroma_filter);

            var fft = new FFT(FRAME_SIZE, OVERLAP, chroma);
            var processor = new AudioProcessor(SAMPLE_RATE, fft);

            processor.Reset(decoder.SampleRate, decoder.Channels);
            decoder.Decode(processor, 120);
            processor.Flush();

            return image;
        }

        /// <summary>
        /// Computes the spectogram of an audio file.
        /// </summary>
        /// <param name="decoder">The <see cref="IDecoder"/> instance.</param>
        /// <returns>Chroma image.</returns>
        public static Image ComputeSpectrogram(IDecoder decoder)
        {
            int numBands = 72;

            var image = new Image(numBands);
            var image_builder = new ImageBuilder(image);

            var chroma = new Spectrum(numBands, MIN_FREQ, MAX_FREQ, FRAME_SIZE, SAMPLE_RATE, image_builder);

            var fft = new FFT(FRAME_SIZE, OVERLAP, chroma);
            var processor = new AudioProcessor(SAMPLE_RATE, fft);

            processor.Reset(decoder.SampleRate, decoder.Channels);
            decoder.Decode(processor, 120);
            processor.Flush();

            return image;
        }
    }
}
