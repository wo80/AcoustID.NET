
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

            Assert.AreEqual(0, avg.GetAverage());

            avg.AddValue(100);
            Assert.AreEqual(100, avg.GetAverage());

            avg.AddValue(50);
            Assert.AreEqual(75, avg.GetAverage());

            avg.AddValue(0);
            Assert.AreEqual(25, avg.GetAverage());

            avg.AddValue(1000);
            Assert.AreEqual(500, avg.GetAverage());
        }
    }
}
