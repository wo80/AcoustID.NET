
namespace AcoustID.Tests
{
    using AcoustID.Chromaprint;

    /// <summary>
    /// FeatureVectorBuffer interface.
    /// </summary>
    public class FeatureVectorBuffer : IFeatureVectorConsumer
    {
        public double[] features;

        public void Consume(double[] features)
        {
            this.features = features;
        }
    }
}
