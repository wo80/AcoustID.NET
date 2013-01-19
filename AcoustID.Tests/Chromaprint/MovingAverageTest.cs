using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AcoustID.Chromaprint;

namespace AcoustID.Tests.Chromaprint
{
    [TestClass]
    public class MovingAverageTest
    {
        [TestMethod]
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
