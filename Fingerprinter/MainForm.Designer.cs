namespace Fingerprinter
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnRequest = new System.Windows.Forms.Button();
            this.btnFingerPrint = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbFile = new System.Windows.Forms.Label();
            this.lbDuration = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbFingerprint = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lbAudio = new System.Windows.Forms.Label();
            this.lbBenchmark = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listView2 = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cbFpcalc = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(12, 12);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(109, 23);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Open File";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnRequest
            // 
            this.btnRequest.Enabled = false;
            this.btnRequest.Location = new System.Drawing.Point(242, 12);
            this.btnRequest.Name = "btnRequest";
            this.btnRequest.Size = new System.Drawing.Size(109, 23);
            this.btnRequest.TabIndex = 0;
            this.btnRequest.Text = "Get AcoustID";
            this.btnRequest.UseVisualStyleBackColor = true;
            this.btnRequest.Click += new System.EventHandler(this.btnRequest_Click);
            // 
            // btnFingerPrint
            // 
            this.btnFingerPrint.Enabled = false;
            this.btnFingerPrint.Location = new System.Drawing.Point(127, 12);
            this.btnFingerPrint.Name = "btnFingerPrint";
            this.btnFingerPrint.Size = new System.Drawing.Size(109, 23);
            this.btnFingerPrint.TabIndex = 0;
            this.btnFingerPrint.Text = "Get Fingerprint";
            this.btnFingerPrint.UseVisualStyleBackColor = true;
            this.btnFingerPrint.Click += new System.EventHandler(this.btnFingerPrint_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "File:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Duration:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbFile
            // 
            this.lbFile.AutoSize = true;
            this.lbFile.Location = new System.Drawing.Point(89, 53);
            this.lbFile.Name = "lbFile";
            this.lbFile.Size = new System.Drawing.Size(11, 13);
            this.lbFile.TabIndex = 1;
            this.lbFile.Text = "-";
            this.lbFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbDuration
            // 
            this.lbDuration.AutoSize = true;
            this.lbDuration.Location = new System.Drawing.Point(89, 87);
            this.lbDuration.Name = "lbDuration";
            this.lbDuration.Size = new System.Drawing.Size(11, 13);
            this.lbDuration.TabIndex = 1;
            this.lbDuration.Text = "-";
            this.lbDuration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Fingerprint:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbFingerprint
            // 
            this.tbFingerprint.Location = new System.Drawing.Point(92, 112);
            this.tbFingerprint.Name = "tbFingerprint";
            this.tbFingerprint.ReadOnly = true;
            this.tbFingerprint.Size = new System.Drawing.Size(480, 22);
            this.tbFingerprint.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(42, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Audio:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbAudio
            // 
            this.lbAudio.AutoSize = true;
            this.lbAudio.Location = new System.Drawing.Point(89, 70);
            this.lbAudio.Name = "lbAudio";
            this.lbAudio.Size = new System.Drawing.Size(11, 13);
            this.lbAudio.TabIndex = 1;
            this.lbAudio.Text = "-";
            this.lbAudio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbBenchmark
            // 
            this.lbBenchmark.Location = new System.Drawing.Point(92, 137);
            this.lbBenchmark.Name = "lbBenchmark";
            this.lbBenchmark.Size = new System.Drawing.Size(480, 13);
            this.lbBenchmark.TabIndex = 1;
            this.lbBenchmark.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.Location = new System.Drawing.Point(12, 163);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(560, 140);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "AcoustId";
            this.columnHeader1.Width = 230;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Score";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Recordings";
            this.columnHeader3.Width = 100;
            // 
            // listView2
            // 
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.listView2.FullRowSelect = true;
            this.listView2.GridLines = true;
            this.listView2.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView2.Location = new System.Drawing.Point(12, 309);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(560, 140);
            this.listView2.TabIndex = 3;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            this.listView2.DoubleClick += new System.EventHandler(this.listView2_DoubleClick);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Recording";
            this.columnHeader4.Width = 200;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Title";
            this.columnHeader5.Width = 150;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Duration";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Artist";
            this.columnHeader7.Width = 110;
            // 
            // cbFpcalc
            // 
            this.cbFpcalc.AutoSize = true;
            this.cbFpcalc.Location = new System.Drawing.Point(380, 16);
            this.cbFpcalc.Name = "cbFpcalc";
            this.cbFpcalc.Size = new System.Drawing.Size(192, 17);
            this.cbFpcalc.TabIndex = 4;
            this.cbFpcalc.Text = "Use fpcalc.exe for fingerprinting";
            this.cbFpcalc.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 462);
            this.Controls.Add(this.cbFpcalc);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.tbFingerprint);
            this.Controls.Add(this.lbBenchmark);
            this.Controls.Add(this.lbDuration);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbAudio);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRequest);
            this.Controls.Add(this.btnFingerPrint);
            this.Controls.Add(this.btnOpen);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.Text = "AcoustID.NET Fingerprinter";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnRequest;
        private System.Windows.Forms.Button btnFingerPrint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbFile;
        private System.Windows.Forms.Label lbDuration;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbFingerprint;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbAudio;
        private System.Windows.Forms.Label lbBenchmark;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.CheckBox cbFpcalc;

    }
}

