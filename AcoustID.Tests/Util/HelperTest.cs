
namespace AcoustID.Tests.Util
{
    using AcoustID.Util;
    using NUnit.Framework;

    public class HelperTest
    {
        [Test]
        public void TestPrepareHammingWindow()
        {
            double[] window_ex = { 0.08, 0.187619556165, 0.460121838273, 0.77, 0.972258605562, 0.972258605562, 0.77, 0.460121838273, 0.187619556165, 0.08 };
            double[] window = new double[10];
            Helper.PrepareHammingWindow(ref window, 0, 10);

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(window_ex[i], window[i], TestsHelper.EPS);
            }
        }

        [Test]
        public void TestApplyWindow1()
        {
            double[] window_ex = { 0.08, 0.187619556165, 0.460121838273, 0.77, 0.972258605562, 0.972258605562, 0.77, 0.460121838273, 0.187619556165, 0.08 };
            double[] window = new double[10];
            double[] inoutput = new double[10];
            Helper.PrepareHammingWindow(ref window, 0, 10);

            for (int i = 0; i < 10; i++)
            {
                inoutput[i] = short.MaxValue;
            }

            double scale = 1.0 / short.MaxValue;
            Helper.ApplyWindow(ref inoutput, window, 10, scale);

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(window_ex[i], inoutput[i], TestsHelper.EPS);
            }
        }

        [Test]
        public void TestApplyWindow2()
        {
            double[] window = new double[10];
            double[] inoutput = new double[10];
            Helper.PrepareHammingWindow(ref window, 0, 10);

            double scale = 1.0 / short.MaxValue;
            Helper.ApplyWindow(ref inoutput, window, 10, scale);
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(0.0, inoutput[i], TestsHelper.EPS);
            }
        }

        [Test]
        public void TestSum()
        {
            double[] data = { 0.1, 0.2, 0.4, 1.0 };
            Assert.AreEqual(1.7, Helper.Sum(data, 0, 4), TestsHelper.EPS);
        }

        [Test]
        public void TestEuclideanNorm()
        {
            double[] data = new double[] { 0.1, 0.2, 0.4, 1.0 };
            Assert.AreEqual(1.1, Helper.EuclideanNorm(data), TestsHelper.EPS);
        }

        [Test]
        public void TestNormalizeVector()
        {
            double[] data = new double[] { 0.1, 0.2, 0.4, 1.0 };
            double[] normalized_data = { 0.090909, 0.181818, 0.363636, 0.909091 };
            Helper.NormalizeVector(data, Helper.EuclideanNorm(data), 0.01);
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(normalized_data[i], data[i], 1e-5); // "Wrong data at index " + i;
            }
        }

        [Test]
        public void TestNormalizeVectorNearZero()
        {
            double[] data = new double[] { 0.0, 0.001, 0.002, 0.003 };
            Helper.NormalizeVector(data, Helper.EuclideanNorm(data), 0.01);
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(0.0, data[i], TestsHelper.EPS); // "Wrong data at index " + i;
            }
        }

        [Test]
        public void TestNormalizeVectorZero()
        {
            double[] data = new double[] { 0.0, 0.0, 0.0, 0.0 };
            Helper.NormalizeVector(data, Helper.EuclideanNorm(data), 0.01);
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(0.0, data[i], TestsHelper.EPS); // "Wrong data at index " + i;
            }
        }
    }
}
