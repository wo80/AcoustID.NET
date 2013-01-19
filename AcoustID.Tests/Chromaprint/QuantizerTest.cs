using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AcoustID.Chromaprint;

namespace AcoustID.Tests.Chromaprint
{
    [TestClass]
    public class QuantizerTest
    {
        [TestMethod]
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
