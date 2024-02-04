
namespace AcoustID.Tests.Chromaprint
{
    using AcoustID.Chromaprint;
    using NUnit.Framework;

    public class IntegralImageTest
    {
        [Test]
        public void TestBasic2D()
        {
            double[] data = {
		        1.0, 2.0,
		        3.0, 4.0,
	        };
            Image image = new Image(2, data);
            IntegralImage integral_image = new IntegralImage(image);
            Assert.That(integral_image[0][0], Is.EqualTo(1.0));
            Assert.That(integral_image[0][1], Is.EqualTo(3.0));
            Assert.That(integral_image[1][0], Is.EqualTo(4.0));
            Assert.That(integral_image[1][1], Is.EqualTo(10.0));
        }

        [Test]
        public void TestVertical1D()
        {
            double[] data = {
		        1.0, 2.0, 3.0
	        };
            Image image = new Image(1, data);
            IntegralImage integral_image = new IntegralImage(image);
            Assert.That(integral_image[0][0], Is.EqualTo(1.0));
            Assert.That(integral_image[1][0], Is.EqualTo(3.0));
            Assert.That(integral_image[2][0], Is.EqualTo(6.0));
        }

        [Test]
        public void TestHorizontal1D()
        {
            double[] data = {
		        1.0, 2.0, 3.0
	        };
            Image image = new Image(3, data);
            IntegralImage integral_image = new IntegralImage(image);
            Assert.That(integral_image[0][0], Is.EqualTo(1.0));
            Assert.That(integral_image[0][1], Is.EqualTo(3.0));
            Assert.That(integral_image[0][2], Is.EqualTo(6.0));
        }

        [Test]
        public void TestArea()
        {
            double[] data = {
		        1.0, 2.0, 3.0,
		        4.0, 5.0, 6.0,
		        7.0, 8.0, 9.0,
	        };

            Image image = new Image(3, data);
            IntegralImage integral_image = new IntegralImage(image);

            Assert.That(integral_image.Area(0, 0, 0, 0), Is.EqualTo((1.0)));
            Assert.That(integral_image.Area(0, 0, 1, 0), Is.EqualTo((1.0 + 4.0)));
            Assert.That(integral_image.Area(0, 0, 2, 0), Is.EqualTo((1.0 + 4.0 + 7.0)));

            Assert.That(integral_image.Area(0, 0, 0, 1), Is.EqualTo((1.0) + (2.0)));
            Assert.That(integral_image.Area(0, 0, 1, 1), Is.EqualTo((1.0 + 4.0) + (2.0 + 5.0)));
            Assert.That(integral_image.Area(0, 0, 2, 1), Is.EqualTo((1.0 + 4.0 + 7.0) + (2.0 + 5.0 + 8.0)));

            Assert.That(integral_image.Area(0, 1, 0, 1), Is.EqualTo((2.0)));
            Assert.That(integral_image.Area(0, 1, 1, 1), Is.EqualTo((2.0 + 5.0)));
            Assert.That(integral_image.Area(0, 1, 2, 1), Is.EqualTo((2.0 + 5.0 + 8.0)));
        }
    }
}
