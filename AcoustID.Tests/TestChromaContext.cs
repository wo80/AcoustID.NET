using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AcoustID.Chromaprint;
using AcoustID.Util;

namespace AcoustID.Tests
{
    [TestClass]
    public class TestChromaContext
    {
        [TestMethod]
        public void Test2SilenceFp()
        {
            short[] zeroes = new short[1024];

            ChromaContext ctx = new ChromaContext(ChromaprintAlgorithm.TEST2);
            ctx.Start(44100, 1);

            for (int i = 0; i < 130; i++)
            {
                ctx.Feed(zeroes, 1024);
            }

            ;

            ctx.Finish();
            string fp = ctx.GetFingerprint();

            Assert.AreEqual(18, fp.Length);
            Assert.AreEqual("AQAAA0mUaEkSRZEGAA", fp);
        }

        [TestMethod]
        public void Test2SilenceRawFp()
        {
            short[] zeroes = new short[1024];

            ChromaContext ctx = new ChromaContext(ChromaprintAlgorithm.TEST2);
            ctx.Start(44100, 1);
            for (int i = 0; i < 130; i++)
            {
                ctx.Feed(zeroes, 1024);
            }


            ctx.Finish();
            int[] fp = ctx.GetRawFingerprint();

            Assert.AreEqual(3, fp.Length);
            Assert.AreEqual(627964279, fp[0]);
            Assert.AreEqual(627964279, fp[1]);
            Assert.AreEqual(627964279, fp[2]);
        }

        [TestMethod]
        public void TestEncodeFingerprint()
        {
            int[] fingerprint = { 1, 0 };
            byte[] expected = new byte[] { 55, 0, 0, 2, 65, 0 };


            byte[] encoded = ChromaContext.EncodeFingerprint(fingerprint, 55, false);

            Assert.AreEqual(6, encoded.Length);
            for (int i = 0; i < encoded.Length; i++)
            {
                Assert.AreEqual(expected[i], encoded[i]);// << "Different at " << i;
            }
        }

        [TestMethod]
        public void TestEncodeFingerprintBase64()
        {
            int[] fingerprint = { 1, 0 };
            byte[] expected = Base64.ByteEncoding.GetBytes("NwAAAkEA");


            byte[] encoded = ChromaContext.EncodeFingerprint(fingerprint, 55, true);

            Assert.AreEqual(8, encoded.Length);
            for (int i = 0; i < encoded.Length; i++)
            {
                Assert.AreEqual(expected[i], encoded[i]);
            }
        }

        [TestMethod]
        public void TestDecodeFingerprint()
        {
            byte[] data = { 55, 0, 0, 2, 65, 0 };

            int algorithm;
            int[] fingerprint = ChromaContext.DecodeFingerprint(data, false, out algorithm);

            Assert.AreEqual(2, fingerprint.Length);
            Assert.AreEqual(55, algorithm);
            Assert.AreEqual(1, fingerprint[0]);
            Assert.AreEqual(0, fingerprint[1]);
        }
    }
}
