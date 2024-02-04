
namespace AcoustID.Tests.Chromaprint
{
    using AcoustID.Chromaprint;
    using NUnit.Framework;

    public class MovingAverageTest
    {
        [Test]
        public void TestMethod1()
        {
            MovingAverage avg = new MovingAverage(2);

            Assert.That(avg.GetAverage(), Is.EqualTo(0));

            avg.AddValue(100);
            Assert.That(avg.GetAverage(), Is.EqualTo(100));

            avg.AddValue(50);
            Assert.That(avg.GetAverage(), Is.EqualTo(75));

            avg.AddValue(0);
            Assert.That(avg.GetAverage(), Is.EqualTo(25));

            avg.AddValue(1000);
            Assert.That(avg.GetAverage(), Is.EqualTo(500));
        }
    }
}
