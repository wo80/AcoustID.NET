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
using System.Threading;

namespace Fingerprinter
{
    public partial class MainForm : Form
    {
        IAudioDecoder decoder;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Fpcalc.Path = @"D:\Projects\AcoustId\extern\fpcalc.exe";

            decoder = new NAudioDecoder();
            //decoder = new BassDecoder();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "MP3 files (*.mp3)|*.mp3|WAV files (*.wav)|*.wav";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                lbFile.Text = dlg.FileName;

                ResetAll();

                decoder.Load(dlg.FileName);

                int bits = decoder.SourceBitDepth;
                int channels = decoder.SourceChannels;

                if (decoder.Ready)
                {
                    lbAudio.Text = String.Format("{0}Hz, {1}bit{2}, {3}",
                        decoder.SourceSampleRate, bits, bits != 16 ? " (not supported)" : "",
                        channels == 2 ? "stereo" : (channels == 1 ? "mono" : "multi-channel"));

                    lbDuration.Text = decoder.Duration.ToString();

                    btnFingerPrint.Enabled = true;
                }
                else
                {
                    lbAudio.Text = "Failed to load audio";
                    lbDuration.Text = String.Empty;
                }
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
            if (String.IsNullOrEmpty(AcoustID.Configuration.ApiKey))
            {
                // To get your own api key, visit https://acoustid.org/.
                AcoustID.Configuration.ApiKey = "CImoilnU";
            }

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

        private void listView2_DoubleClick(object sender, EventArgs e)
        {
            var selection = listView2.SelectedItems;

            if (selection.Count <= 0)
            {
                return;
            }

            try
            {
                var pos = listView2.PointToClient(Control.MousePosition);
                var hit = listView2.HitTest(pos);

                int index = hit.Item.SubItems.IndexOf(hit.SubItem);

                Clipboard.SetText(selection[0].SubItems[index].Text);
            }
            catch (Exception)
            {
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

            var context = TaskScheduler.FromCurrentSynchronizationContext();

            var task = service.GetAsync(fingerprint, duration, new string[] { "recordings", "compress" });

            // Error handling:
            task.ContinueWith(t =>
            {
                foreach (var e in t.Exception.InnerExceptions)
                {
                    MessageBox.Show(e.Message, "Webservice error");
                }
            },
            CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, context);

            // On success:
            task.ContinueWith(t =>
            {
                btnOpen.Enabled = true;

                var results = t.Result;

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
            },
            CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, context);
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
                    //btnOpen.Enabled = false;
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
                //btnOpen.Enabled = false;
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

                // Release audio file handle.
                decoder.Dispose();
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
