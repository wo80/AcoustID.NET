// -----------------------------------------------------------------------
// <copyright file="Fpcalc.cs" company="">
// Christian Woltering, https://github.com/wo80
// </copyright>
// -----------------------------------------------------------------------

namespace Fingerprinter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;

    /// <summary>
    /// Executes fpcalc to compute a fingerprint.
    /// </summary>
    public class Fpcalc
    {
        public static string Path;

        public static Dictionary<string, string> Execute(string file)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            // Create the child process.
            Process process = new Process();

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.FileName = Path;
            process.StartInfo.Arguments = "\"" + file + "\"";

            process.Start();

            string output = process.StandardOutput.ReadToEnd();

            process.WaitForExit(10000);
            process.Close();

            string[] lines = output.Split('\n');

            foreach (var line in lines)
            {
                string[] pair = line.Split('=');

                if (pair.Length == 2)
                {
                    result.Add(pair[0].ToLowerInvariant(), pair[1]);
                }
            }

            return result;
        }
    }
}
