
namespace AcoustID.Tests.Chromaprint
{
    using AcoustID.Chromaprint;
    using NUnit.Framework;

    public class AudioProcessorTest
    {
        [Test]
        public void TestAccessors()
        {
            AudioBuffer buffer = new AudioBuffer();
            AudioBuffer buffer2 = new AudioBuffer();
            AudioProcessor processor = new AudioProcessor(44100, buffer);

            Assert.AreEqual(44100, processor.TargetSampleRate);
            Assert.AreEqual(buffer, processor.Consumer);

            processor.TargetSampleRate = 11025;
            Assert.AreEqual(11025, processor.TargetSampleRate);

            processor.Consumer = buffer2;
            Assert.AreEqual(buffer2, processor.Consumer);
        }

#if TEST_LOCAL_FILES
        [Test]
        public void TestPassThrough()
        {
            short[] data = TestsHelper.LoadAudioFile("test_mono_44100.raw");

            Assert.IsNotNull(data, "Failed to load test data (check DATA_PATH in TestsHelper.cs)");

            AudioBuffer buffer = new AudioBuffer();
            AudioProcessor processor = new AudioProcessor(44100, buffer);
            processor.Reset(44100, 1);
            processor.Consume(data, data.Length);
            processor.Flush();

            CollectionAssert.AreEqual(data, buffer.data);
        }

        [Test]
        public void TestStereoToMono()
        {
            short[] data1 = TestsHelper.LoadAudioFile("test_stereo_44100.raw");
            short[] data2 = TestsHelper.LoadAudioFile("test_mono_44100.raw");

            Assert.IsNotNull(data1, "Failed to load test data (check DATA_PATH in TestsHelper.cs)");
            Assert.IsNotNull(data2, "Failed to load test data (check DATA_PATH in TestsHelper.cs)");

            AudioBuffer buffer = new AudioBuffer();
            AudioProcessor processor = new AudioProcessor(44100, buffer);
            processor.Reset(44100, 2);
            processor.Consume(data1, data1.Length);
            processor.Flush();

            CollectionAssert.AreEqual(data2, buffer.data);
        }

        [Test]
        public void TestResampleMono()
        {
            short[] data1 = TestsHelper.LoadAudioFile("test_mono_44100.raw");
            short[] data2 = TestsHelper.LoadAudioFile("test_mono_11025.raw");

            Assert.IsNotNull(data1, "Failed to load test data (check DATA_PATH in TestsHelper.cs)");
            Assert.IsNotNull(data2, "Failed to load test data (check DATA_PATH in TestsHelper.cs)");

            AudioBuffer buffer = new AudioBuffer();
            AudioProcessor processor = new AudioProcessor(11025, buffer);
            processor.Reset(44100, 1);
            processor.Consume(data1, data1.Length);
            processor.Flush();

            CollectionAssert.AreEqual(data2, buffer.data);
        }

        [Test]
        public void TestResampleMonoNonInteger()
        {
            short[] data1 = TestsHelper.LoadAudioFile("test_mono_44100.raw");
            short[] data2 = TestsHelper.LoadAudioFile("test_mono_8000.raw");

            Assert.IsNotNull(data1, "Failed to load test data (check DATA_PATH in TestsHelper.cs)");
            Assert.IsNotNull(data2, "Failed to load test data (check DATA_PATH in TestsHelper.cs)");

            AudioBuffer buffer = new AudioBuffer();
            AudioProcessor processor = new AudioProcessor(8000, buffer);
            processor.Reset(44100, 1);
            processor.Consume(data1, data1.Length);
            processor.Flush();

            CollectionAssert.AreEqual(data2, buffer.data);
        }

        [Test]
        public void TestStereoToMonoAndResample()
        {
            short[] data1 = TestsHelper.LoadAudioFile("test_stereo_44100.raw");
            short[] data2 = TestsHelper.LoadAudioFile("test_mono_11025.raw");

            Assert.IsNotNull(data1, "Failed to load test data (check DATA_PATH in TestsHelper.cs)");
            Assert.IsNotNull(data2, "Failed to load test data (check DATA_PATH in TestsHelper.cs)");

            AudioBuffer buffer = new AudioBuffer();
            AudioProcessor processor = new AudioProcessor(11025, buffer);
            processor.Reset(44100, 2);
            processor.Consume(data1, data1.Length);
            processor.Flush();

            CollectionAssert.AreEqual(data2, buffer.data);
        }
#endif
    }
}
