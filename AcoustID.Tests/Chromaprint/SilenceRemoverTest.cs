
namespace AcoustID.Tests.Chromaprint
{
    using AcoustID.Chromaprint;
    using NUnit.Framework;

    public class SilenceRemoverTest
    {
        [Test]
        public void TestPassThrough()
        {
            short[] samples = { 1000, 2000, 3000, 4000, 5000, 6000 };
            short[] data = (short[])(samples.Clone());

            AudioBuffer buffer = new AudioBuffer();
            SilenceRemover processor = new SilenceRemover(buffer);
            processor.Reset(44100, 1);
            processor.Consume(data, data.Length);
            processor.Flush();

            Assert.AreEqual(data.Length, buffer.data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                Assert.AreEqual(data[i], buffer.data[i]); // << "Signals differ at index " << i;
            }
        }

        [Test]
        public void TestRemoveLeadingSilence()
        {
            short[] samples1 = { 0, 60, 0, 1000, 2000, 0, 4000, 5000, 0 };
            short[] data1 = (short[])(samples1.Clone());

            short[] samples2 = { 1000, 2000, 0, 4000, 5000, 0 };
            short[] data2 = (short[])(samples2.Clone());

            AudioBuffer buffer = new AudioBuffer();
            SilenceRemover processor = new SilenceRemover(buffer, 100);
            processor.Reset(44100, 1);
            processor.Consume(data1, data1.Length);
            processor.Flush();

            Assert.AreEqual(data2.Length, buffer.data.Length);
            for (int i = 0; i < data2.Length; i++)
            {
                Assert.AreEqual(data2[i], buffer.data[i]); // << "Signals differ at index " << i;
            }
        }
    }
}
