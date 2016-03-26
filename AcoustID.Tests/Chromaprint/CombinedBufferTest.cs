
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
            Assert.AreEqual(8, buffer.Size);
            buffer.Shift(1);
            Assert.AreEqual(7, buffer.Size);
        }

        [Test]
        public void TestAccessElements()
        {
            short[] buffer1 = { 1, 2, 3, 4, 5 };
            short[] buffer2 = { 6, 7, 8 };
            CombinedBuffer buffer = new CombinedBuffer(buffer1, 5, buffer2, 3);
            for (int i = 0; i < 8; i++)
            {
                Assert.AreEqual(1 + i, buffer[i]);
            }
            buffer.Shift(1);
            Assert.AreEqual(1, buffer.Offset);
            for (int i = 0; i < 7; i++)
            {
                Assert.AreEqual(2 + i, buffer[i]);
            }
            buffer.Shift(5);
            Assert.AreEqual(6, buffer.Offset);
            for (int i = 0; i < 2; i++)
            {
                Assert.AreEqual(7 + i, buffer[i]);
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
                Assert.AreEqual(0, tmp[i]);
            }

            buffer.Flush(tmp);
            Assert.AreEqual(0, buffer.Offset);

            for (int i = 0; i < 8; i++)
            {
                Assert.AreEqual(1 + i, tmp[i]);
            }
            for (int i = 8; i < 10; i++)
            {
                Assert.AreEqual(0, tmp[i]);
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
                
                Assert.AreEqual(0, tmp[i]);
            }

            buffer.Flush(tmp);
            Assert.AreEqual(6, buffer.Offset);

            for (int i = 0; i < 2; i++)
            {
                Assert.AreEqual(7 + i, tmp[i]);
            }
            for (int i = 2; i < 10; i++)
            {
                Assert.AreEqual(0, tmp[i]);
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
                Assert.AreEqual(0, tmp[i]);
            }

            // Read from first part
            int size = buffer.Read(tmp, 0, 4);
            Assert.AreEqual(0, buffer.Offset);
            Assert.AreEqual(4, size);

            for (int i = 0; i < size; i++)
            {
                Assert.AreEqual(1 + i, tmp[i]);
            }

            // Read from both parts
            size = buffer.Read(tmp, 3, 4);
            Assert.AreEqual(0, buffer.Offset);
            Assert.AreEqual(4, size);

            for (int i = 0; i < size; i++)
            {
                Assert.AreEqual(4 + i, tmp[i]);
            }

            // Read at split point
            size = buffer.Read(tmp, 5, 4);
            Assert.AreEqual(0, buffer.Offset);
            Assert.AreEqual(3, size);

            for (int i = 0; i < size; i++)
            {
                Assert.AreEqual(6 + i, tmp[i]);
            }

            // Read from last parts
            size = buffer.Read(tmp, 6, 4);
            Assert.AreEqual(0, buffer.Offset);
            Assert.AreEqual(2, size);

            for (int i = 0; i < size; i++)
            {
                Assert.AreEqual(7 + i, tmp[i]);
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
            Assert.AreEqual(2, buffer.Offset);
            Assert.AreEqual(4, size);

            for (int i = 0; i < size; i++)
            {
                Assert.AreEqual(3 + i, tmp[i]);
            }

            // Offset 4
            buffer.Shift(2);

            size = buffer.Read(tmp, 0, 4);
            Assert.AreEqual(4, buffer.Offset);
            Assert.AreEqual(4, size);

            for (int i = 0; i < size; i++)
            {
                Assert.AreEqual(5 + i, tmp[i]);
            }

            // Offset 6
            buffer.Shift(2);

            size = buffer.Read(tmp, 0, 4);
            Assert.AreEqual(6, buffer.Offset);
            Assert.AreEqual(2, size);

            for (int i = 0; i < size; i++)
            {
                Assert.AreEqual(7 + i, tmp[i]);
            }
        }
    }
}
