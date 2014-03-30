using System;
using AcoustID.Chromaprint;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AcoustID.Tests.Chromaprint
{
    [TestClass]
    public class IntegralImageTest
    {
        [TestMethod]
        public void TestBasic2D()
        {
            double[] data = {
		        1.0, 2.0,
		        3.0, 4.0,
	        };
            Image image = new Image(2, data);
            IntegralImage integral_image = new IntegralImage(image);
            Assert.AreEqual(1.0, integral_image[0][0]);
            Assert.AreEqual(3.0, integral_image[0][1]);
            Assert.AreEqual(4.0, integral_image[1][0]);
            Assert.AreEqual(10.0, integral_image[1][1]);
        }

        [TestMethod]
        public void TestVertical1D()
        {
            double[] data = {
		        1.0, 2.0, 3.0
	        };
            Image image = new Image(1, data);
            IntegralImage integral_image = new IntegralImage(image);
            Assert.AreEqual(1.0, integral_image[0][0]);
            Assert.AreEqual(3.0, integral_image[1][0]);
            Assert.AreEqual(6.0, integral_image[2][0]);
        }

        [TestMethod]
        public void TestHorizontal1D()
        {
            double[] data = {
		        1.0, 2.0, 3.0
	        };
            Image image = new Image(3, data);
            IntegralImage integral_image = new IntegralImage(image);
            Assert.AreEqual(1.0, integral_image[0][0]);
            Assert.AreEqual(3.0, integral_image[0][1]);
            Assert.AreEqual(6.0, integral_image[0][2]);
        }

        [TestMethod]
        public void TestArea()
        {
            double[] data = {
		        1.0, 2.0, 3.0,
		        4.0, 5.0, 6.0,
		        7.0, 8.0, 9.0,
	        };

            Image image = new Image(3, data);
            IntegralImage integral_image = new IntegralImage(image);

            Assert.AreEqual((1.0), integral_image.Area(0, 0, 0, 0));
            Assert.AreEqual((1.0 + 4.0), integral_image.Area(0, 0, 1, 0));
            Assert.AreEqual((1.0 + 4.0 + 7.0), integral_image.Area(0, 0, 2, 0));

            Assert.AreEqual((1.0) + (2.0), integral_image.Area(0, 0, 0, 1));
            Assert.AreEqual((1.0 + 4.0) + (2.0 + 5.0), integral_image.Area(0, 0, 1, 1));
            Assert.AreEqual((1.0 + 4.0 + 7.0) + (2.0 + 5.0 + 8.0), integral_image.Area(0, 0, 2, 1));

            Assert.AreEqual((2.0), integral_image.Area(0, 1, 0, 1));
            Assert.AreEqual((2.0 + 5.0), integral_image.Area(0, 1, 1, 1));
            Assert.AreEqual((2.0 + 5.0 + 8.0), integral_image.Area(0, 1, 2, 1));
        }
    }
}
