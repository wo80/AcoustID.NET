
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
                Assert.That(window[i], Is.EqualTo(window_ex[i]).Within(TestsHelper.EPS));
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
                Assert.That(inoutput[i], Is.EqualTo(window_ex[i]).Within(TestsHelper.EPS));
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
                Assert.That(inoutput[i], Is.EqualTo(0.0).Within(TestsHelper.EPS));
            }
        }

        [Test]
        public void TestSum()
        {
            double[] data = { 0.1, 0.2, 0.4, 1.0 };
            Assert.That(Helper.Sum(data, 0, 4), Is.EqualTo(1.7).Within(TestsHelper.EPS));
        }

        [Test]
        public void TestEuclideanNorm()
        {
            double[] data = new double[] { 0.1, 0.2, 0.4, 1.0 };
            Assert.That(Helper.EuclideanNorm(data), Is.EqualTo(1.1).Within(TestsHelper.EPS));
        }

        [Test]
        public void TestNormalizeVector()
        {
            double[] data = new double[] { 0.1, 0.2, 0.4, 1.0 };
            double[] normalized_data = { 0.090909, 0.181818, 0.363636, 0.909091 };
            Helper.NormalizeVector(data, Helper.EuclideanNorm(data), 0.01);
            for (int i = 0; i < 4; i++)
            {
                Assert.That(data[i], Is.EqualTo(normalized_data[i]).Within(1e-5)); // "Wrong data at index " + i;
            }
        }

        [Test]
        public void TestNormalizeVectorNearZero()
        {
            double[] data = new double[] { 0.0, 0.001, 0.002, 0.003 };
            Helper.NormalizeVector(data, Helper.EuclideanNorm(data), 0.01);
            for (int i = 0; i < 4; i++)
            {
                Assert.That(data[i], Is.EqualTo(0.0).Within(TestsHelper.EPS)); // "Wrong data at index " + i;
            }
        }

        [Test]
        public void TestNormalizeVectorZero()
        {
            double[] data = new double[] { 0.0, 0.0, 0.0, 0.0 };
            Helper.NormalizeVector(data, Helper.EuclideanNorm(data), 0.01);
            for (int i = 0; i < 4; i++)
            {
                Assert.That(data[i], Is.EqualTo(0.0).Within(TestsHelper.EPS)); // "Wrong data at index " + i;
            }
        }
    }
}
