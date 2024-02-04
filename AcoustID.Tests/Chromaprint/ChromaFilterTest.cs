
namespace AcoustID.Tests.Chromaprint
{
    using AcoustID.Chromaprint;
    using NUnit.Framework;

    public class ChromaFilterTest
    {
        [Test]
        public void TestBlur2()
        {
            double[] coefficients = { 0.5, 0.5 };
            Image image = new Image(12);
            ImageBuilder builder = new ImageBuilder(image);
            ChromaFilter filter = new ChromaFilter(coefficients, builder);
            double[] d1 = { 0.0, 5.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] d2 = { 1.0, 6.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] d3 = { 2.0, 7.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            filter.Consume(d1);
            filter.Consume(d2);
            filter.Consume(d3);
            Assert.That(image.Rows, Is.EqualTo(2));
            Assert.That(image.Get(0, 0), Is.EqualTo(0.5));
            Assert.That(image.Get(1, 0), Is.EqualTo(1.5));
            Assert.That(image.Get(0, 1), Is.EqualTo(5.5));
            Assert.That(image.Get(1, 1), Is.EqualTo(6.5));
        }

        [Test]
        public void TestBlur3()
        {
            double[] coefficients = { 0.5, 0.7, 0.5 };
            Image image = new Image(12, 0);
            ImageBuilder builder = new ImageBuilder(image);
            ChromaFilter filter = new ChromaFilter(coefficients, builder);
            double[] d1 = { 0.0, 5.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] d2 = { 1.0, 6.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] d3 = { 2.0, 7.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] d4 = { 3.0, 8.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            filter.Consume(d1);
            filter.Consume(d2);
            filter.Consume(d3);
            filter.Consume(d4);
            Assert.That(image.Rows, Is.EqualTo(2));
            Assert.That(image.Get(0, 0), Is.EqualTo(1.7).Within(TestsHelper.EPS));
            Assert.That(image.Get(1, 0), Is.EqualTo(3.399999999999999).Within(TestsHelper.EPS));
            Assert.That(image.Get(0, 1), Is.EqualTo(10.199999999999999).Within(TestsHelper.EPS));
            Assert.That(image.Get(1, 1), Is.EqualTo(11.899999999999999).Within(TestsHelper.EPS));
        }

        [Test]
        public void TestDiff()
        {
            double[] coefficients = { 1.0, -1.0 };
            Image image = new Image(12);
            ImageBuilder builder = new ImageBuilder(image);
            ChromaFilter filter = new ChromaFilter(coefficients, builder);
            double[] d1 = { 0.0, 5.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] d2 = { 1.0, 6.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            double[] d3 = { 2.0, 7.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            filter.Consume(d1);
            filter.Consume(d2);
            filter.Consume(d3);
            Assert.That(image.Rows, Is.EqualTo(2));
            Assert.That(image.Get(0, 0), Is.EqualTo(-1.0));
            Assert.That(image.Get(1, 0), Is.EqualTo(-1.0));
            Assert.That(image.Get(0, 1), Is.EqualTo(-1.0));
            Assert.That(image.Get(1, 1), Is.EqualTo(-1.0));
        }
    }
}
