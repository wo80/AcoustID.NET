using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AcoustID.Chromaprint;

namespace AcoustID.Tests.Chromaprint
{
    [TestClass]
    public class ChromaFilterTest
    {
        [TestMethod]
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
            Assert.AreEqual(2, image.Rows);
            Assert.AreEqual(0.5, image.Get(0, 0));
            Assert.AreEqual(1.5, image.Get(1, 0));
            Assert.AreEqual(5.5, image.Get(0, 1));
            Assert.AreEqual(6.5, image.Get(1, 1));
        }

        [TestMethod]
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
            Assert.AreEqual(2, image.Rows);
            Assert.AreEqual(1.7, image.Get(0, 0), TestsHelper.EPS);
            Assert.AreEqual(3.399999999999999, image.Get(1, 0), TestsHelper.EPS);
            Assert.AreEqual(10.199999999999999, image.Get(0, 1), TestsHelper.EPS);
            Assert.AreEqual(11.899999999999999, image.Get(1, 1), TestsHelper.EPS);
        }

        [TestMethod]
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
            Assert.AreEqual(2, image.Rows);
            Assert.AreEqual(-1.0, image.Get(0, 0));
            Assert.AreEqual(-1.0, image.Get(1, 0));
            Assert.AreEqual(-1.0, image.Get(0, 1));
            Assert.AreEqual(-1.0, image.Get(1, 1));
        }
    }
}
