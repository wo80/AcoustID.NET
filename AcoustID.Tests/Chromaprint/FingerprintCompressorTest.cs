using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AcoustID.Chromaprint;
using AcoustID.Util;

namespace AcoustID.Tests.Chromaprint
{
    [TestClass]
    public class FingerprintCompressorTest
    {
        [TestMethod]
        public void TestOneItemOneBit()
        {
            FingerprintCompressor compressor = new FingerprintCompressor();

            int[] fingerprint = { 1 };
            string value = compressor.Compress(fingerprint);

            byte[] expected = { 0, 0, 0, 1, 1 };
            byte[] actual = Base64.ByteEncoding.GetBytes(value);

            CollectionAssert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestOneItemThreeBits()
        {
            FingerprintCompressor compressor = new FingerprintCompressor();

            int[] fingerprint = { 7 };
            string value = compressor.Compress(fingerprint);

            byte[] expected = { 0, 0, 0, 1, 73, 0 };
            byte[] actual = Base64.ByteEncoding.GetBytes(value);

            CollectionAssert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestOneItemOneBitExcept()
        {
            FingerprintCompressor compressor = new FingerprintCompressor();

            int[] fingerprint = { 1 << 6 };
            string value = compressor.Compress(fingerprint);

            byte[] expected = { 0, 0, 0, 1, 7, 0 };
            byte[] actual = Base64.ByteEncoding.GetBytes(value);

            CollectionAssert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestOneItemOneBitExcept2()
        {
            FingerprintCompressor compressor = new FingerprintCompressor();

            int[] fingerprint = { 1 << 8 };
            string value = compressor.Compress(fingerprint);

            byte[] expected = { 0, 0, 0, 1, 7, 2 };
            byte[] actual = Base64.ByteEncoding.GetBytes(value);

            CollectionAssert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestTwoItems()
        {
            FingerprintCompressor compressor = new FingerprintCompressor();

            int[] fingerprint = { 1, 0 };
            string value = compressor.Compress(fingerprint);

            byte[] expected = { 0, 0, 0, 2, 65, 0 };
            byte[] actual = Base64.ByteEncoding.GetBytes(value);

            CollectionAssert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestTwoItemsNoChange()
        {
            FingerprintCompressor compressor = new FingerprintCompressor();

            int[] fingerprint = { 1, 1 };
            string value = compressor.Compress(fingerprint);

            byte[] expected = { 0, 0, 0, 2, 1, 0 };
            byte[] actual = Base64.ByteEncoding.GetBytes(value);

            CollectionAssert.AreEqual(actual, expected);
        }
    }
}
