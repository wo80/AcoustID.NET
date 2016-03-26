
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
        public static string DATA_PATH = @"../../AcoustID.Tests/data/";

        public static int GrayCode(int i)
        {
            int[] CODES = { 0, 1, 3, 2 };
            return CODES[i];
        }

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
    }
}
