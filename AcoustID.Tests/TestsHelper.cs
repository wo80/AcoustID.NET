
namespace AcoustID.Tests
{
    using System;
    using System.IO;

    /// <summary>
    /// Some test helpers.
    /// </summary>
    public static class TestsHelper
    {
        public static double EPS = 2e-6;

#if TEST_LOCAL_FILES
        // All tests using raw audio data stored locally are disabled by
        // default. To enable them, define the TEST_LOCAL_FILES constant.
        // If you get errors, try adjusting the DATA_PATH.

        public static string DATA_PATH = @"../../AcoustID.Tests/data/";

        public static short[] LoadAudioFile(string file)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;

            path = Path.Combine(path, DATA_PATH);
            path = Path.GetFullPath(Path.Combine(path, file));

            if (!File.Exists(path))
            {
                return null;
            }

            byte[] bytes = File.ReadAllBytes(path);

            short[] shorts = new short[bytes.Length / 2];
            Buffer.BlockCopy(bytes, 0, shorts, 0, bytes.Length);

            return shorts;
        }

        public static string LoadTextFile(string file)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;

            path = Path.Combine(path, DATA_PATH);
            path = Path.GetFullPath(Path.Combine(path, file));

            if (!File.Exists(path))
            {
                return null;
            }

            return File.ReadAllText(path);
        }
#endif

        public static int GrayCode(int i)
        {
            int[] CODES = { 0, 1, 3, 2 };
            return CODES[i];
        }

        #region Bit count
        
        // Not used anywhere in chromaprint

        public static int HammingDistance(UInt32 a, UInt32 b)
        {
            return CountSetBits(a ^ b);
        }

        public static int HammingDistance(UInt64 a, UInt64 b)
        {
            return CountSetBits(a ^ b);
        }

        public static int CountSetBits(UInt32 v)
        {
            const uint N32 = (uint)~0U;

            v = v - ((v >> 1) & N32 / 3);
            v = (v & N32 / 15 * 3) + ((v >> 2) & N32 / 15 * 3);
            v = (v + (v >> 4)) & N32 / 255 * 15;
            
            return (int)((uint)(v * (N32 / 255)) >> (sizeof(uint) - 1) * 8);
        }

        public static int CountSetBits(UInt64 v)
        {
            const ulong N64 = (ulong)~0UL;

            v = v - ((v >> 1) & N64 / 3);
            v = (v & N64 / 15 * 3) + ((v >> 2) & N64 / 15 * 3);
            v = (v + (v >> 4)) & N64 / 255 * 15;

            return (int)((ulong)(v * (N64 / 255)) >> (sizeof(ulong) - 1) * 8);
        }

        #endregion
    }
}
