
namespace AcoustID.Tests
{
    using AcoustID.Native;
    using AcoustID.Util;
    using NUnit.Framework;

    public class TestNativeChromaContext
    {
#if TEST_NATIVE
        [Test]
        public void Test2SilenceFp()
        {
            short[] zeroes = new short[1024];

            var ctx = new NativeChromaContext();

            ctx.Start(44100, 1);

            for (int i = 0; i < 130; i++)
            {
                ctx.Feed(zeroes, 1024);
            }

            ctx.Finish();

            string fp = ctx.GetFingerprint();
            int hash = ctx.GetFingerprintHash();

            Assert.AreEqual(18, fp.Length);
            Assert.AreEqual("AQAAA0mUaEkSRZEGAA", fp);
            Assert.AreEqual(627964279, hash);

            ctx.Dispose();
        }

        [Test]
        public void Test2SilenceRawFp()
        {
            short[] zeroes = new short[1024];

            var ctx = new NativeChromaContext();

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

            ctx.Dispose();
        }

        [Test]
        public void TestEncodeFingerprint()
        {
            int[] fingerprint = { 1, 0 };
            byte[] expected = new byte[] { 55, 0, 0, 2, 65, 0 };


            byte[] encoded = NativeChromaContext.EncodeFingerprint(fingerprint, 55, false);

            Assert.AreEqual(6, encoded.Length);

            for (int i = 0; i < 6; i++)
            {
                Assert.AreEqual(expected[i], encoded[i]);// << "Different at " << i;
            }
        }

        [Test]
        public void TestEncodeFingerprintBase64()
        {
            int[] fingerprint = { 1, 0 };
            byte[] expected = Base64.ByteEncoding.GetBytes("NwAAAkEA");


            byte[] encoded = NativeChromaContext.EncodeFingerprint(fingerprint, 55, true);

            Assert.AreEqual(8, encoded.Length);

            for (int i = 0; i < encoded.Length; i++)
            {
                Assert.AreEqual(expected[i], encoded[i]);
            }
        }

        [Test]
        public void TestDecodeFingerprint()
        {
            byte[] data = { 55, 0, 0, 2, 65, 0 };

            int algorithm;
            int[] fingerprint = NativeChromaContext.DecodeFingerprint(data, false, out algorithm);

            Assert.AreEqual(2, fingerprint.Length);
            Assert.AreEqual(55, algorithm);
            Assert.AreEqual(1, fingerprint[0]);
            Assert.AreEqual(0, fingerprint[1]);
        }

        [Test]
        public void TestHashFingerprint()
        {
            int[] fingerprint = { 19681, 22345, 312312, 453425 };

            Assert.AreEqual(17249, NativeChromaContext.HashFingerprint(fingerprint));
        }
#endif
    }
}
