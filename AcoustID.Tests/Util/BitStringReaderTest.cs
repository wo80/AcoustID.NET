
namespace AcoustID.Tests.Util
{
    using AcoustID.Util;
    using NUnit.Framework;

    public class BitStringReaderTest
    {
        [Test]
        public void TestOneByte()
        {
            byte[] data = { unchecked((byte)-28) };
            BitStringReader reader = new BitStringReader(Base64.ByteEncoding.GetString(data));

            Assert.That(reader.Read(2), Is.EqualTo((uint)0));
            Assert.That(reader.Read(2), Is.EqualTo((uint)1));
            Assert.That(reader.Read(2), Is.EqualTo((uint)2));
            Assert.That(reader.Read(2), Is.EqualTo((uint)3));
        }

        [Test]
        public void TestTwoBytesIncomplete()
        {
            byte[] data = { unchecked((byte)-28), unchecked((byte)1) };
            BitStringReader reader = new BitStringReader(Base64.ByteEncoding.GetString(data));

            Assert.That(reader.Read(2), Is.EqualTo((uint)0));
            Assert.That(reader.Read(2), Is.EqualTo((uint)1));
            Assert.That(reader.Read(2), Is.EqualTo((uint)2));
            Assert.That(reader.Read(2), Is.EqualTo((uint)3));
            Assert.That(reader.Read(2), Is.EqualTo((uint)1));
        }

        [Test]
        public void TestTwoBytesSplit()
        {
            byte[] data = { unchecked((byte)-120), unchecked((byte)6) };
            BitStringReader reader = new BitStringReader(Base64.ByteEncoding.GetString(data));

            Assert.That(reader.Read(3), Is.EqualTo((uint)0));
            Assert.That(reader.Read(3), Is.EqualTo((uint)1));
            Assert.That(reader.Read(3), Is.EqualTo((uint)2));
            Assert.That(reader.Read(3), Is.EqualTo((uint)3));
        }


        [Test]
        public void TestAvailableBitsAndEOF()
        {
            byte[] data = { unchecked((byte)-120), unchecked((byte)6) };
            BitStringReader reader = new BitStringReader(Base64.ByteEncoding.GetString(data));

            Assert.That(reader.AvailableBits(), Is.EqualTo(16));
            Assert.That(reader.Eof, Is.EqualTo(false));

            reader.Read(3);
            Assert.That(reader.AvailableBits(), Is.EqualTo(13));
            Assert.That(reader.Eof, Is.EqualTo(false));

            reader.Read(3);
            Assert.That(reader.AvailableBits(), Is.EqualTo(10));
            Assert.That(reader.Eof, Is.EqualTo(false));

            reader.Read(3);
            Assert.That(reader.AvailableBits(), Is.EqualTo(7));
            Assert.That(reader.Eof, Is.EqualTo(false));

            reader.Read(7);
            Assert.That(reader.AvailableBits(), Is.EqualTo(0));
            Assert.That(reader.Eof, Is.EqualTo(true));

            Assert.That(reader.Read(3), Is.EqualTo((uint)0));
            Assert.That(reader.Eof, Is.EqualTo(true));
            Assert.That(reader.AvailableBits(), Is.EqualTo(0));
        }
    }
}
