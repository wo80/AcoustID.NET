
namespace AcoustID.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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
