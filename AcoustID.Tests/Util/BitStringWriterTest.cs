
namespace AcoustID.Tests.Util
{
    using AcoustID.Util;
    using NUnit.Framework;

    public class BitStringWriterTest
    {
        [Test]
        public void TestOneByte()
        {
            BitStringWriter writer = new BitStringWriter();
            writer.Write(0, 2);
            writer.Write(1, 2);
            writer.Write(2, 2);
            writer.Write(3, 2);
            writer.Flush();

            byte[] expected = { unchecked((byte)-28) };
            byte[] actual = Base64.ByteEncoding.GetBytes(writer.Value);

            CollectionAssert.AreEqual(actual, expected);
        }

        [Test]
        public void TestTwoBytesIncomplete()
        {
            BitStringWriter writer = new BitStringWriter();
            writer.Write(0, 2);
            writer.Write(1, 2);
            writer.Write(2, 2);
            writer.Write(3, 2);
            writer.Write(1, 2);
            writer.Flush();

            byte[] expected = { unchecked((byte)-28), unchecked((byte)1) };
            byte[] actual = Base64.ByteEncoding.GetBytes(writer.Value);

            CollectionAssert.AreEqual(actual, expected);
        }

        [Test]
        public void TestTwoBytesSplit()
        {
            BitStringWriter writer = new BitStringWriter();
            writer.Write(0, 3);
            writer.Write(1, 3);
            writer.Write(2, 3);
            writer.Write(3, 3);
            writer.Flush();

            byte[] expected = { unchecked((byte)-120), unchecked((byte)6) };
            byte[] actual = Base64.ByteEncoding.GetBytes(writer.Value);

            CollectionAssert.AreEqual(actual, expected);
        }
    }
}
