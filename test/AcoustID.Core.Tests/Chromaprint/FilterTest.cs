
namespace AcoustID.Tests.Chromaprint
{
    using AcoustID.Chromaprint;
    using NUnit.Framework;

    public class FilterTest
    {
        [Test]
        public void TestFilter0()
        {
            Image image = new Image(2, 2);
            image.Set(0, 0, 0.0);
            image.Set(0, 1, 1.0);
            image.Set(1, 0, 2.0);
            image.Set(1, 1, 3.0);

            Filter flt1 = new Filter(0, 0, 1, 1);
            IntegralImage integral_image = new IntegralImage(image);
            Assert.AreEqual(0.0, flt1.Apply(integral_image, 0), TestsHelper.EPS);
            Assert.AreEqual(1.0986123, flt1.Apply(integral_image, 1), TestsHelper.EPS);
        }

        [Test]
        public void TestStaticCompareSubtract()
        {
            double res = Filter.Subtract(2.0, 1.0);
            Assert.AreEqual(1.0, res, TestsHelper.EPS);
        }

        [Test]
        public void TestStaticCompareSubtractLog()
        {
            double res = Filter.SubtractLog(2.0, 1.0);
            Assert.AreEqual(0.4054651, res, TestsHelper.EPS);
        }

        [Test]
        public void TestStaticFilter0()
        {
            double[] data = {
		        1.0, 2.0, 3.0,
		        4.0, 5.0, 6.0,
		        7.0, 8.0, 9.0,
	        };
            Image image = new Image(3, data);
            IntegralImage integral_image = new IntegralImage(image);
            double res;
            res = Filter.Filter0(integral_image, 0, 0, 1, 1, Filter.Subtract);
            Assert.AreEqual(1.0, res, TestsHelper.EPS);
            res = Filter.Filter0(integral_image, 0, 0, 2, 2, Filter.Subtract);
            Assert.AreEqual(12.0, res, TestsHelper.EPS);
            res = Filter.Filter0(integral_image, 0, 0, 3, 3, Filter.Subtract);
            Assert.AreEqual(45.0, res, TestsHelper.EPS);
            res = Filter.Filter0(integral_image, 1, 1, 2, 2, Filter.Subtract);
            Assert.AreEqual(28.0, res, TestsHelper.EPS);
            res = Filter.Filter0(integral_image, 2, 2, 1, 1, Filter.Subtract);
            Assert.AreEqual(9.0, res, TestsHelper.EPS);
            res = Filter.Filter0(integral_image, 0, 0, 3, 1, Filter.Subtract);
            Assert.AreEqual(12.0, res, TestsHelper.EPS);
            res = Filter.Filter0(integral_image, 0, 0, 1, 3, Filter.Subtract);
            Assert.AreEqual(6.0, res, TestsHelper.EPS);
        }

        [Test]
        public void TestStaticFilter1()
        {
            double[] data = {
		        1.0, 2.1, 3.4,
		        3.1, 4.1, 5.1,
		        6.0, 7.1, 8.0
	        };
            Image image = new Image(3, data);
            IntegralImage integral_image = new IntegralImage(image);
            double res;

            res = Filter.Filter1(integral_image, 0, 0, 1, 1, Filter.Subtract);
            Assert.AreEqual(1.0 - 0.0, res);
            res = Filter.Filter1(integral_image, 1, 1, 1, 1, Filter.Subtract);
            Assert.AreEqual(4.1 - 0.0, res);
            res = Filter.Filter1(integral_image, 0, 0, 1, 2, Filter.Subtract);
            Assert.AreEqual(2.1 - 1.0, res);
            res = Filter.Filter1(integral_image, 0, 0, 2, 2, Filter.Subtract);
            Assert.AreEqual((2.1 + 4.1) - (1.0 + 3.1), res);
            res = Filter.Filter1(integral_image, 0, 0, 3, 2, Filter.Subtract);
            Assert.AreEqual((2.1 + 4.1 + 7.1) - (1.0 + 3.1 + 6.0), res);
        }

        [Test]
        public void TestStaticFilter2()
        {
            double[] data = {
		        1.0, 2.0, 3.0,
		        3.0, 4.0, 5.0,
		        6.0, 7.0, 8.0,
	        };
            Image image = new Image(3, data);
            IntegralImage integral_image = new IntegralImage(image);
            double res;
            res = Filter.Filter2(integral_image, 0, 0, 2, 1, Filter.Subtract);
            Assert.AreEqual(2.0, res, TestsHelper.EPS); // 3 - 1
            res = Filter.Filter2(integral_image, 0, 0, 2, 2, Filter.Subtract);
            Assert.AreEqual(4.0, res, TestsHelper.EPS); // 3+4 - 1+2
            res = Filter.Filter2(integral_image, 0, 0, 2, 3, Filter.Subtract);
            Assert.AreEqual(6.0, res, TestsHelper.EPS); // 3+4+5 - 1+2+3
        }

        [Test]
        public void TestStaticFilter3()
        {
            double[] data = {
		        1.0, 2.1, 3.4,
		        3.1, 4.1, 5.1,
		        6.0, 7.1, 8.0,
	        };
            Image image = new Image(3, data);
            IntegralImage integral_image = new IntegralImage(image);
            double res;
            res = Filter.Filter3(integral_image, 0, 0, 2, 2, Filter.Subtract);
            Assert.AreEqual(0.1, res, TestsHelper.EPS); // 2.1+3.1 - 1+4.1
            res = Filter.Filter3(integral_image, 1, 1, 2, 2, Filter.Subtract);
            Assert.AreEqual(0.1, res, TestsHelper.EPS); // 4+8 - 5+7
            res = Filter.Filter3(integral_image, 0, 1, 2, 2, Filter.Subtract);
            Assert.AreEqual(0.3, res, TestsHelper.EPS); // 2.1+5.1 - 3.4+4.1
        }

        [Test]
        public void TestStaticFilter4()
        {
            double[] data = {
		        1.0, 2.0, 3.0,
		        3.0, 4.0, 5.0,
		        6.0, 7.0, 8.0,
	        };
            Image image = new Image(3, data);
            IntegralImage integral_image = new IntegralImage(image);
            double res = Filter.Filter4(integral_image, 0, 0, 3, 3, Filter.Subtract);
            Assert.AreEqual(-13.0, res, TestsHelper.EPS); // 2+4+7 - (1+3+6) - (3+5+8)
        }

        [Test]
        public void TestStaticFilter5()
        {
            double[] data = {
		        1.0, 2.0, 3.0,
		        3.0, 4.0, 5.0,
		        6.0, 7.0, 8.0,
	        };
            Image image = new Image(3, data);
            IntegralImage integral_image = new IntegralImage(image);
            double res = Filter.Filter5(integral_image, 0, 0, 3, 3, Filter.Subtract);
            Assert.AreEqual(-15.0, res, TestsHelper.EPS); // 3+4+5 - (1+2+3) - (6+7+8)
        }
    }
}
