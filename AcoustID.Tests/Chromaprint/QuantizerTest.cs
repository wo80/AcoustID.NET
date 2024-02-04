
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
            Assert.That(q.Quantize(-0.1), Is.EqualTo(0));
            Assert.That(q.Quantize(0.0), Is.EqualTo(1));
            Assert.That(q.Quantize(0.03), Is.EqualTo(1));
            Assert.That(q.Quantize(0.1), Is.EqualTo(2));
            Assert.That(q.Quantize(0.13), Is.EqualTo(2));
            Assert.That(q.Quantize(0.3), Is.EqualTo(3));
            Assert.That(q.Quantize(0.33), Is.EqualTo(3));
            Assert.That(q.Quantize(1000.0), Is.EqualTo(3));
        }
    }
}
