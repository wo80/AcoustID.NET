using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AcoustID.Chromaprint;

namespace AcoustID.Tests.Chromaprint
{
    [TestClass]
    public class LloydsTest
    {
        [TestMethod]
        public void TestStaticLloyds1()
        {
            double[] data = {
		        1.0, 1.1, 1.2,
		        3.0, 3.1, 3.2,
	        };
            List<double> sig = new List<double>(data);
            double[] table = Lloyds.Compute(sig, 2);
            Assert.AreEqual(1, table.Length);
            Assert.AreEqual(2.1, table[0], TestsHelper.EPS);
        }

        [TestMethod]
        public void TestStaticLloyds2()
        {
            double[] data = {
		        1.0, 1.1, 1.2,
	        };
            List<double> sig = new List<double>(data);
            double[] table = Lloyds.Compute(sig, 2);
            Assert.AreEqual(1, table.Length);
            Assert.AreEqual(1.075, table[0], TestsHelper.EPS);
        }

        [TestMethod]
        public void TestStaticLloyds3()
        {
            double[] data = {
		        1.0, 1.1, 1.2,
	        };
            List<double> sig = new List<double>(data);
            double[] table = Lloyds.Compute(sig, 3);
            Assert.AreEqual(2, table.Length);
            Assert.AreEqual(1.05, table[0], TestsHelper.EPS);
            Assert.AreEqual(1.15, table[1], TestsHelper.EPS);
        }

        [TestMethod]
        public void TestStaticLloyds4()
        {
            double[] data = {
		        435,219,891,906,184,572,301,892,875,121,245,146,640,137,938,25,
                668,288,848,790,141,890,528,145,289,861,339,769,293,757
	        };
            List<double> sig = new List<double>(data);
            double[] table = Lloyds.Compute(sig, 4);
            Assert.AreEqual(3, table.Length);
            Assert.AreEqual(214.77678, table[0], 0.0001);
            Assert.AreEqual(451.5625, table[1], 0.0001);
            Assert.AreEqual(729.04547, table[2], 0.0001);
        }
    }
}
