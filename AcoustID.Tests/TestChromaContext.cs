
namespace AcoustID.Tests
{
    using AcoustID.Chromaprint;
    using AcoustID.Util;
    using NUnit.Framework;

    public class TestChromaContext
    {
        [Test]
        public void Test2SilenceFp()
        {
            short[] zeroes = new short[1024];

            ChromaContext ctx = new ChromaContext(ChromaprintAlgorithm.TEST2);
            ctx.Start(44100, 1);

            for (int i = 0; i < 130; i++)
            {
                ctx.Feed(zeroes, 1024);
            }

            ctx.Finish();

            string fp = ctx.GetFingerprint();
            int hash = ctx.GetFingerprintHash();

            Assert.That(fp.Length, Is.EqualTo(18));
            Assert.That(fp, Is.EqualTo("AQAAA0mUaEkSRZEGAA"));
            Assert.That(hash, Is.EqualTo(627964279));
        }

        [Test]
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

            Assert.That(fp.Length, Is.EqualTo(3));
            Assert.That(fp[0], Is.EqualTo(627964279));
            Assert.That(fp[1], Is.EqualTo(627964279));
            Assert.That(fp[2], Is.EqualTo(627964279));
        }

        [Test]
        public void TestEncodeFingerprint()
        {
            int[] fingerprint = { 1, 0 };
            byte[] expected = new byte[] { 55, 0, 0, 2, 65, 0 };


            byte[] encoded = ChromaContext.EncodeFingerprint(fingerprint, 55, false);

            Assert.That(encoded.Length, Is.EqualTo(6));
            for (int i = 0; i < encoded.Length; i++)
            {
                Assert.That(encoded[i], Is.EqualTo(expected[i]));// << "Different at " << i;
            }
        }

        [Test]
        public void TestEncodeFingerprintBase64()
        {
            int[] fingerprint = { 1, 0 };
            byte[] expected = Base64.ByteEncoding.GetBytes("NwAAAkEA");


            byte[] encoded = ChromaContext.EncodeFingerprint(fingerprint, 55, true);

            Assert.That(encoded.Length, Is.EqualTo(8));
            for (int i = 0; i < encoded.Length; i++)
            {
                Assert.That(encoded[i], Is.EqualTo(expected[i]));
            }
        }

        [Test]
        public void TestDecodeFingerprint()
        {
            byte[] data = { 55, 0, 0, 2, 65, 0 };

            int[] fingerprint = ChromaContext.DecodeFingerprint(data, false, out int algorithm);

            Assert.That(fingerprint.Length, Is.EqualTo(2));
            Assert.That(algorithm, Is.EqualTo(55));
            Assert.That(fingerprint[0], Is.EqualTo(1));
            Assert.That(fingerprint[1], Is.EqualTo(0));
        }

        [Test]
        public void TestHashFingerprint()
        {
            int[] fingerprint = { 19681, 22345, 312312, 453425 };

            Assert.That(SimHash.Compute(fingerprint), Is.EqualTo(17249));
        }
    }
}
