using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fingerprinter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Test();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        static void Test()
        {
            Console.WriteLine(AcoustID.Native.NativeChromaContext.GetVersion());

            using (var c = new AcoustID.Native.NativeChromaContext())
            {
            }
        }
    }
}
