
namespace AcoustID.Tests.Chromaprint
{
    using AcoustID.Chromaprint;
    using NUnit.Framework;

    public class CombinedBufferTest
    {
        [Test]
        public void TestSize()
        {
            short[] buffer1 = { 1, 2, 3, 4, 5 };
            short[] buffer2 = { 6, 7, 8 };
            CombinedBuffer buffer = new CombinedBuffer(buffer1, 5, buffer2, 3);
            Assert.That(buffer.Size, Is.EqualTo(8));
            buffer.Shift(1);
            Assert.That(buffer.Size, Is.EqualTo(7));
        }

        [Test]
        public void TestAccessElements()
        {
            short[] buffer1 = { 1, 2, 3, 4, 5 };
            short[] buffer2 = { 6, 7, 8 };
            CombinedBuffer buffer = new CombinedBuffer(buffer1, 5, buffer2, 3);
            for (int i = 0; i < 8; i++)
            {
                Assert.That(buffer[i], Is.EqualTo(1 + i));
            }
            buffer.Shift(1);
            Assert.That(buffer.Offset, Is.EqualTo(1));
            for (int i = 0; i < 7; i++)
            {
                Assert.That(buffer[i], Is.EqualTo(2 + i));
            }
            buffer.Shift(5);
            Assert.That(buffer.Offset, Is.EqualTo(6));
            for (int i = 0; i < 2; i++)
            {
                Assert.That(buffer[i], Is.EqualTo(7 + i));
            }
        }

        [Test]
        public void TestFlush()
        {
            short[] buffer1 = { 1, 2, 3, 4, 5 };
            short[] buffer2 = { 6, 7, 8 };
            short[] tmp = new short[10];

            CombinedBuffer buffer = new CombinedBuffer(buffer1, 5, buffer2, 3);

            for (int i = 0; i < 10; i++)
            {
                Assert.That(tmp[i], Is.EqualTo(0));
            }

            buffer.Flush(tmp);
            Assert.That(buffer.Offset, Is.EqualTo(0));

            for (int i = 0; i < 8; i++)
            {
                Assert.That(tmp[i], Is.EqualTo(1 + i));
            }
            for (int i = 8; i < 10; i++)
            {
                Assert.That(tmp[i], Is.EqualTo(0));
            }
        }

        [Test]
        public void TestFlushAfterShift()
        {
            short[] buffer1 = { 1, 2, 3, 4, 5 };
            short[] buffer2 = { 6, 7, 8 };
            short[] tmp = new short[10];

            CombinedBuffer buffer = new CombinedBuffer(buffer1, 5, buffer2, 3);
            buffer.Shift(6);

            for (int i = 0; i < 10; i++)
            {

                Assert.That(tmp[i], Is.EqualTo(0));
            }

            buffer.Flush(tmp);
            Assert.That(buffer.Offset, Is.EqualTo(6));

            for (int i = 0; i < 2; i++)
            {
                Assert.That(tmp[i], Is.EqualTo(7 + i));
            }
            for (int i = 2; i < 10; i++)
            {
                Assert.That(tmp[i], Is.EqualTo(0));
            }
        }

        [Test]
        public void TestRead()
        {
            short[] buffer1 = { 1, 2, 3, 4, 5 };
            short[] buffer2 = { 6, 7, 8 };
            short[] tmp = new short[10];

            CombinedBuffer buffer = new CombinedBuffer(buffer1, 5, buffer2, 3);

            for (int i = 0; i < 10; i++)
            {
                Assert.That(tmp[i], Is.EqualTo(0));
            }

            // Read from first part
            int size = buffer.Read(tmp, 0, 4);
            Assert.That(buffer.Offset, Is.EqualTo(0));
            Assert.That(size, Is.EqualTo(4));

            for (int i = 0; i < size; i++)
            {
                Assert.That(tmp[i], Is.EqualTo(1 + i));
            }

            // Read from both parts
            size = buffer.Read(tmp, 3, 4);
            Assert.That(buffer.Offset, Is.EqualTo(0));
            Assert.That(size, Is.EqualTo(4));

            for (int i = 0; i < size; i++)
            {
                Assert.That(tmp[i], Is.EqualTo(4 + i));
            }

            // Read at split point
            size = buffer.Read(tmp, 5, 4);
            Assert.That(buffer.Offset, Is.EqualTo(0));
            Assert.That(size, Is.EqualTo(3));

            for (int i = 0; i < size; i++)
            {
                Assert.That(tmp[i], Is.EqualTo(6 + i));
            }

            // Read from last parts
            size = buffer.Read(tmp, 6, 4);
            Assert.That(buffer.Offset, Is.EqualTo(0));
            Assert.That(size, Is.EqualTo(2));

            for (int i = 0; i < size; i++)
            {
                Assert.That(tmp[i], Is.EqualTo(7 + i));
            }
        }

        [Test]
        public void TestReadAfterShift()
        {
            short[] buffer1 = { 1, 2, 3, 4, 5 };
            short[] buffer2 = { 6, 7, 8 };
            short[] tmp = new short[10];

            CombinedBuffer buffer = new CombinedBuffer(buffer1, 5, buffer2, 3);

            // Offset 2
            buffer.Shift(2);

            int size = buffer.Read(tmp, 0, 4);
            Assert.That(buffer.Offset, Is.EqualTo(2));
            Assert.That(size, Is.EqualTo(4));

            for (int i = 0; i < size; i++)
            {
                Assert.That(tmp[i], Is.EqualTo(3 + i));
            }

            // Offset 4
            buffer.Shift(2);

            size = buffer.Read(tmp, 0, 4);
            Assert.That(buffer.Offset, Is.EqualTo(4));
            Assert.That(size, Is.EqualTo(4));

            for (int i = 0; i < size; i++)
            {
                Assert.That(tmp[i], Is.EqualTo(5 + i));
            }

            // Offset 6
            buffer.Shift(2);

            size = buffer.Read(tmp, 0, 4);
            Assert.That(buffer.Offset, Is.EqualTo(6));
            Assert.That(size, Is.EqualTo(2));

            for (int i = 0; i < size; i++)
            {
                Assert.That(tmp[i], Is.EqualTo(7 + i));
            }
        }
    }
}
