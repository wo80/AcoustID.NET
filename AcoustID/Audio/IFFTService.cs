// -----------------------------------------------------------------------
// <copyright file="IFFTService.cs" company="">
// Original C++ implementation by Lukas Lalinsky, http://acoustid.org/chromaprint
// </copyright>
// -----------------------------------------------------------------------

namespace AcoustID.Audio
{
    using AcoustID.Chromaprint;

    /// <summary>
    /// Interface for services computing the FFT.
    /// </summary>
    public interface IFFTService
    {
        void Initialize(int frame_size, double[] window);
        void ComputeFrame(short[] input, double[] output);
    }
}
