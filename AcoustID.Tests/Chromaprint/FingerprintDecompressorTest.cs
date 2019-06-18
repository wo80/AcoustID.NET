
namespace AcoustID.Tests.Chromaprint
{
    using AcoustID.Chromaprint;
    using AcoustID.Util;
    using NUnit.Framework;

    public class FingerprintDecompressorTest
    {
        [Test]
        public void TestOneItemOneBit()
        {
            int[] expected = { 1 };
            byte[] data = { 0, 0, 0, 1, 1 };

            FingerprintDecompressor decompressor = new FingerprintDecompressor();

            int algorithm = -1;
            int[] actual = decompressor.Decompress(Base64.ByteEncoding.GetString(data), out algorithm);
            Assert.AreEqual(0, algorithm);
            CollectionAssert.AreEqual(actual, expected);
        }

        [Test]
        public void TestOneItemThreeBits()
        {
            int[] expected = { 7 };
            byte[] data = { 0, 0, 0, 1, 73, 0 };

            FingerprintDecompressor decompressor = new FingerprintDecompressor();

            int algorithm = -1;
            int[] actual = decompressor.Decompress(Base64.ByteEncoding.GetString(data), out algorithm);
            Assert.AreEqual(0, algorithm);
            CollectionAssert.AreEqual(actual, expected);
        }

        [Test]
        public void TestOneItemOneBitExcept()
        {
            int[] expected = { 1 << 6 };
            byte[] data = { 0, 0, 0, 1, 7, 0 };

            FingerprintDecompressor decompressor = new FingerprintDecompressor();

            int algorithm = -1;
            int[] actual = decompressor.Decompress(Base64.ByteEncoding.GetString(data), out algorithm);
            Assert.AreEqual(0, algorithm);
            CollectionAssert.AreEqual(actual, expected);
        }

        [Test]
        public void TestOneItemOneBitExcept2()
        {
            int[] expected = { 1 << 8 };
            byte[] data = { 0, 0, 0, 1, 7, 2 };

            FingerprintDecompressor decompressor = new FingerprintDecompressor();

            int algorithm = -1;
            int[] actual = decompressor.Decompress(Base64.ByteEncoding.GetString(data), out algorithm);
            Assert.AreEqual(0, algorithm);
            CollectionAssert.AreEqual(actual, expected);
        }

        [Test]
        public void TestTwoItems()
        {
            int[] expected = { 1, 0 };
            byte[] data = { 0, 0, 0, 2, 65, 0 };

            FingerprintDecompressor decompressor = new FingerprintDecompressor();

            int algorithm = -1;
            int[] actual = decompressor.Decompress(Base64.ByteEncoding.GetString(data), out algorithm);
            Assert.AreEqual(0, algorithm);
            CollectionAssert.AreEqual(actual, expected);
        }

        [Test]
        public void TestTwoItemsNoChange()
        {
            int[] expected = { 1, 1 };
            byte[] data = { 0, 0, 0, 2, 1, 0 };

            FingerprintDecompressor decompressor = new FingerprintDecompressor();

            int algorithm = -1;
            int[] actual = decompressor.Decompress(Base64.ByteEncoding.GetString(data), out algorithm);
            Assert.AreEqual(0, algorithm);
            CollectionAssert.AreEqual(actual, expected);
        }

        [Test]
        public void TestInvalid1()
        {
            byte[] data = { 0, 255, 255, 255 };

            FingerprintDecompressor decompressor = new FingerprintDecompressor();

            int algorithm = -1;
            int[] value = decompressor.Decompress(Base64.ByteEncoding.GetString(data), out algorithm);
            Assert.AreEqual(value.Length, 0);
            Assert.AreEqual(0, algorithm);
        }

        [Test]
        public void TestLong()
        {
            int[] expected = { -587455133,-591649759,-574868448,-576973520,-543396544,1330439488,1326360000,1326355649,1191625921,1192674515,1194804466,1195336818,1165981042,1165956451,1157441379,1157441299,1291679571,1291673457,1170079601 };
	
            string data = Base64.Decode("AQAAEwkjrUmSJQpUHflR9mjSJMdZpcO_Imdw9dCO9Clu4_wQPvhCB01w6xAtXNcAp5RASgDBhDSCGGIAcwA");

            FingerprintDecompressor decompressor = new FingerprintDecompressor();

            int algorithm = -1;
            int[] value = decompressor.Decompress(data, out algorithm);

            Assert.AreEqual(1, algorithm);
            CollectionAssert.AreEqual(value, expected);
        }
    }
}
