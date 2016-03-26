
namespace AcoustID.Tests.Chromaprint
{
    using AcoustID.Chromaprint;
    using NUnit.Framework;

    public class QuantizerTest
    {
        [Test]
        public void TestQuantize()
        {
            Quantizer q = new Quantizer(0.0, 0.1, 0.3);
            Assert.AreEqual(0, q.Quantize(-0.1));
            Assert.AreEqual(1, q.Quantize(0.0));
            Assert.AreEqual(1, q.Quantize(0.03));
            Assert.AreEqual(2, q.Quantize(0.1));
            Assert.AreEqual(2, q.Quantize(0.13));
            Assert.AreEqual(3, q.Quantize(0.3));
            Assert.AreEqual(3, q.Quantize(0.33));
            Assert.AreEqual(3, q.Quantize(1000.0));
        }
    }
}
