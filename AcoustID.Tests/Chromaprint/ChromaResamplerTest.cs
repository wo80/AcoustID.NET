
namespace AcoustID.Tests.Chromaprint
{
    using AcoustID.Chromaprint;
    using NUnit.Framework;

    public class ChromaResamplerTest
    {
        [Test]
        public void Test1()
        {
            Image image = new Image(12, 0);
            ImageBuilder builder = new ImageBuilder(image);
            ChromaResampler resampler = new ChromaResampler(2, builder);
            double[] d1 = { 0.0, 5.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] d2 = { 1.0, 6.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] d3 = { 2.0, 7.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            resampler.Consume(d1);
            resampler.Consume(d2);
            resampler.Consume(d3);
            Assert.That(image.Rows, Is.EqualTo(1));
            Assert.That(image.Get(0, 0), Is.EqualTo(0.5));
            Assert.That(image.Get(0, 1), Is.EqualTo(5.5));
        }

        [Test]
        public void Test2()
        {
            Image image = new Image(12);
            ImageBuilder builder = new ImageBuilder(image);
            ChromaResampler resampler = new ChromaResampler(2, builder);
            double[] d1 = { 0.0, 5.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] d2 = { 1.0, 6.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] d3 = { 2.0, 7.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] d4 = { 3.0, 8.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            resampler.Consume(d1);
            resampler.Consume(d2);
            resampler.Consume(d3);
            resampler.Consume(d4);
            Assert.That(image.Rows, Is.EqualTo(2));
            Assert.That(image.Get(0, 0), Is.EqualTo(0.5));
            Assert.That(image.Get(0, 1), Is.EqualTo(5.5));
            Assert.That(image.Get(1, 0), Is.EqualTo(2.5));
            Assert.That(image.Get(1, 1), Is.EqualTo(7.5));
        }

    }
}
