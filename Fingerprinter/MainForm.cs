using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using AcoustID;
using AcoustID.Web;
using Fingerprinter.Audio;

namespace Fingerprinter
{
    public partial class MainForm : Form
    {
        NAudioDecoder decoder;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "MP3 files (*.mp3)|*.mp3";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                lbFile.Text = dlg.FileName;

                if (decoder != null)
                {
                    decoder.Dispose();
                }

                decoder = new NAudioDecoder(dlg.FileName);

                lbAudio.Text = String.Format("{0}Hz, {1}bit{2}, {3}", 
                    decoder.SampleRate, 
                    decoder.BitsPerSample, 
                    decoder.BitsPerSample != 16 ? " (not supported)" : "",
                    decoder.Channels == 2 ? "stereo" : (decoder.Channels == 1 ? "mono" : "multi-channel"));

                lbDuration.Text = decoder.Duration.ToString();

                if (decoder.Ready)
                {
                    btnFingerPrint.Enabled = true;
                }

                ResetAll();
            }
        }

        private void btnFingerPrint_Click(object sender, EventArgs e)
        {
            if (cbFpcalc.Checked)
            {
                if (Fpcalc.Path == null || !File.Exists(Fpcalc.Path))
                {
                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.Filter = "fpcalc.exe|fpcalc.exe";

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        Fpcalc.Path = dlg.FileName;
                    }
					else return; // Do nothing ...
                }

                ProcessFileFpcalc(lbFile.Text);
            }
            else
            {
                ProcessFile(lbFile.Text);
            }
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            Lookup(tbFingerprint.Text, int.Parse(lbDuration.Text));
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selection = listView1.SelectedItems;

            if (selection.Count <= 0)
            {
                return;
            }

            var recordings = selection[0].Tag as List<Recording>;

            if (recordings == null)
            {
                return;
            }

            listView2.Items.Clear();

            foreach (var record in recordings)
            {
                string artist = String.Empty;

                int count = record.Artists.Count;

                if (count > 0)
                {
                    artist = record.Artists[0].Name;

                    if (count > 1)
                    {
                        artist += (" (" + (count - 1) + " more)");
                    }
                }

                var item = new ListViewItem(new string[]
                {
                    record.Id,
                    record.Title,
                    record.Duration.ToString(),
                    artist
                });

                listView2.Items.Add(item);
            }
        }

        private void ResetAll()
        {
            lbBenchmark.Text = String.Empty;
            tbFingerprint.Text = String.Empty;

            listView1.Items.Clear();
            listView2.Items.Clear();
        }

        private void Lookup(string fingerprint, int duration)
        {
            btnOpen.Enabled = false;
            btnFingerPrint.Enabled = false;
            btnRequest.Enabled = false;

            LookupService service = new LookupService();

            service.GetAsync((results) =>
            {
                btnOpen.Enabled = true;

                if (results.Count == 0)
                {
                    if (String.IsNullOrEmpty(service.Error))
                    {
                        MessageBox.Show("No results for given fingerprint.");
                    }
                    else MessageBox.Show(service.Error, "Webservice error");

                    return;
                }

                foreach (var result in results)
                {
                    var item = new ListViewItem(new string[]
                    {
                        result.Id,
                        result.Score.ToString(CultureInfo.InvariantCulture),
                        result.Recordings.Count.ToString()
                    });

                    item.Tag = result.Recordings;

                    listView1.Items.Add(item);
                }
            }, fingerprint, duration, new string[] { "recordings", "compress" });
        }

        private void ExportImage(string file)
        {
            //var image = Chromagram.Compute(file, new NAudioDecoder(file));
            //PngExport.ExportImage(image, "chroma.png");
        }

        private void ProcessFile(string file)
        {
            if (File.Exists(file))
            {
                if (decoder.Ready)
                {
                    btnOpen.Enabled = false;
                    btnFingerPrint.Enabled = false;
                    btnRequest.Enabled = false;

                    Task.Factory.StartNew(() =>
                    {
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();

                        ChromaContext context = new ChromaContext();
                        context.Start(decoder.SampleRate, decoder.Channels);
                        decoder.Decode(context.Consumer, 120);
                        context.Finish();

                        stopwatch.Stop();

                        ProcessFileCallback(context.GetFingerprint(), stopwatch.ElapsedMilliseconds);
                    });
                }
            }
        }

        private void ProcessFileFpcalc(string file)
        {
            if (File.Exists(file))
            {
                btnOpen.Enabled = false;
                btnFingerPrint.Enabled = false;
                btnRequest.Enabled = false;

                Task.Factory.StartNew(() =>
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    var result = Fpcalc.Execute(file);

                    stopwatch.Stop();

                    if (result.ContainsKey("fingerprint"))
                    {
                        ProcessFileCallback(result["fingerprint"], stopwatch.ElapsedMilliseconds);
                    }
                    else
                    {
                        ProcessFileCallback(String.Empty, 0);
                    }
                });
            }
        }

        private void ProcessFileCallback(string fingerprint, long time)
        {
            Action action = () =>
            {
                tbFingerprint.Text = fingerprint;

                lbBenchmark.Text = String.Format("Fingerprint length: {0} (calculated in {1}ms)",
                    fingerprint.Length, time);

                btnOpen.Enabled = true;
                btnRequest.Enabled = true;
            };

            if (tbFingerprint.InvokeRequired)
            {
                tbFingerprint.Invoke(action);
            }
            else
            {
                action();
            }
        }
    }
}
