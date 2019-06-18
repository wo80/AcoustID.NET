// -----------------------------------------------------------------------
// <copyright file="IFFTFrameConsumer.cs" company="">
// Original C++ implementation by Lukas Lalinsky, http://acoustid.org/chromaprint
// </copyright>
// -----------------------------------------------------------------------

namespace AcoustID.Chromaprint
{
    /// <summary>
    /// Consumer of frames produced by FFT.
    /// </summary>
    public interface IFFTFrameConsumer
    {
        void Consume(double[] frame);
    }
}
