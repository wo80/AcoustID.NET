using AcoustID.Chromaprint;

namespace AcoustID.Tests
{

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
