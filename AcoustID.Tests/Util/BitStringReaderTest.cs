using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AcoustID.Util;

namespace AcoustID.Tests.Util
{
    [TestClass]
    public class BitStringReaderTest
    {
        [TestMethod]
        public void TestOneByte()
        {
            byte[] data = { unchecked((byte)-28) };
            BitStringReader reader = new BitStringReader(Base64.ByteEncoding.GetString(data));

            Assert.AreEqual((uint)0, reader.Read(2));
            Assert.AreEqual((uint)1, reader.Read(2));
            Assert.AreEqual((uint)2, reader.Read(2));
            Assert.AreEqual((uint)3, reader.Read(2));
        }

        [TestMethod]
        public void TestTwoBytesIncomplete()
        {
            byte[] data = { unchecked((byte)-28), unchecked((byte)1) };
            BitStringReader reader = new BitStringReader(Base64.ByteEncoding.GetString(data));

            Assert.AreEqual((uint)0, reader.Read(2));
            Assert.AreEqual((uint)1, reader.Read(2));
            Assert.AreEqual((uint)2, reader.Read(2));
            Assert.AreEqual((uint)3, reader.Read(2));
            Assert.AreEqual((uint)1, reader.Read(2));
        }

        [TestMethod]
        public void TestTwoBytesSplit()
        {
            byte[] data = { unchecked((byte)-120), unchecked((byte)6) };
            BitStringReader reader = new BitStringReader(Base64.ByteEncoding.GetString(data));

            Assert.AreEqual((uint)0, reader.Read(3));
            Assert.AreEqual((uint)1, reader.Read(3));
            Assert.AreEqual((uint)2, reader.Read(3));
            Assert.AreEqual((uint)3, reader.Read(3));
        }


        [TestMethod]
        public void TestAvailableBitsAndEOF()
        {
            byte[] data = { unchecked((byte)-120), unchecked((byte)6) };
            BitStringReader reader = new BitStringReader(Base64.ByteEncoding.GetString(data));

            Assert.AreEqual(16, reader.AvailableBits());
            Assert.AreEqual(false, reader.Eof);

            reader.Read(3);
            Assert.AreEqual(13, reader.AvailableBits());
            Assert.AreEqual(false, reader.Eof);

            reader.Read(3);
            Assert.AreEqual(10, reader.AvailableBits());
            Assert.AreEqual(false, reader.Eof);

            reader.Read(3);
            Assert.AreEqual(7, reader.AvailableBits());
            Assert.AreEqual(false, reader.Eof);

            reader.Read(7);
            Assert.AreEqual(0, reader.AvailableBits());
            Assert.AreEqual(true, reader.Eof);

            Assert.AreEqual((uint)0, reader.Read(3));
            Assert.AreEqual(true, reader.Eof);
            Assert.AreEqual(0, reader.AvailableBits());
        }
    }
}
