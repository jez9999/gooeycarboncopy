namespace CarbonCopy {
	partial class frmMain {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.lstSourceDirs = new System.Windows.Forms.ListBox();
            this.txtSourceDir = new System.Windows.Forms.TextBox();
            this.btnSourceBrowse = new System.Windows.Forms.Button();
            this.btnAddDir = new System.Windows.Forms.Button();
            this.btnRemDir = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtDestDir = new System.Windows.Forms.TextBox();
            this.btnDestBrowse = new System.Windows.Forms.Button();
            this.grpBackupDetails = new System.Windows.Forms.GroupBox();
            this.radIncremental = new System.Windows.Forms.RadioButton();
            this.radCarbon = new System.Windows.Forms.RadioButton();
            this.grpVerbosity = new System.Windows.Forms.GroupBox();
            this.lblVerbose = new System.Windows.Forms.Label();
            this.lblDebug = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.lblInformational = new System.Windows.Forms.Label();
            this.lblThisWillDisplay = new System.Windows.Forms.Label();
            this.lstVerbosity = new System.Windows.Forms.ComboBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.lblDestDir = new System.Windows.Forms.Label();
            this.btnDestSet = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.grpSource = new System.Windows.Forms.GroupBox();
            this.grpDest = new System.Windows.Forms.GroupBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.chkDryRun = new System.Windows.Forms.CheckBox();
            this.grpBackupDetails.SuspendLayout();
            this.grpVerbosity.SuspendLayout();
            this.grpSource.SuspendLayout();
            this.grpDest.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstSourceDirs
            // 
            this.lstSourceDirs.FormattingEnabled = true;
            this.lstSourceDirs.Location = new System.Drawing.Point(6, 19);
            this.lstSourceDirs.Name = "lstSourceDirs";
            this.lstSourceDirs.Size = new System.Drawing.Size(647, 108);
            this.lstSourceDirs.Sorted = true;
            this.lstSourceDirs.TabIndex = 101;
            this.lstSourceDirs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstSourceDirs_KeyDown);
            // 
            // txtSourceDir
            // 
            this.txtSourceDir.Location = new System.Drawing.Point(6, 133);
            this.txtSourceDir.Name = "txtSourceDir";
            this.txtSourceDir.Size = new System.Drawing.Size(578, 20);
            this.txtSourceDir.TabIndex = 102;
            this.txtSourceDir.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSourceDir_KeyPress);
            // 
            // btnSourceBrowse
            // 
            this.btnSourceBrowse.Location = new System.Drawing.Point(590, 133);
            this.btnSourceBrowse.Name = "btnSourceBrowse";
            this.btnSourceBrowse.Size = new System.Drawing.Size(63, 23);
            this.btnSourceBrowse.TabIndex = 103;
            this.btnSourceBrowse.Text = "Browse...";
            this.btnSourceBrowse.UseVisualStyleBackColor = true;
            this.btnSourceBrowse.Click += new System.EventHandler(this.btnSourceBrowse_Click);
            // 
            // btnAddDir
            // 
            this.btnAddDir.Location = new System.Drawing.Point(483, 159);
            this.btnAddDir.Name = "btnAddDir";
            this.btnAddDir.Size = new System.Drawing.Size(170, 23);
            this.btnAddDir.TabIndex = 105;
            this.btnAddDir.Text = "&Add directory";
            this.btnAddDir.UseVisualStyleBackColor = true;
            this.btnAddDir.Click += new System.EventHandler(this.btnAddDir_Click);
            // 
            // btnRemDir
            // 
            this.btnRemDir.Location = new System.Drawing.Point(6, 159);
            this.btnRemDir.Name = "btnRemDir";
            this.btnRemDir.Size = new System.Drawing.Size(170, 23);
            this.btnRemDir.TabIndex = 104;
            this.btnRemDir.Text = "&Remove directory";
            this.btnRemDir.UseVisualStyleBackColor = true;
            this.btnRemDir.Click += new System.EventHandler(this.btnRemDir_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 461);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(351, 23);
            this.btnStart.TabIndex = 1000;
            this.btnStart.Text = "S&tart backup";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtDestDir
            // 
            this.txtDestDir.Location = new System.Drawing.Point(6, 19);
            this.txtDestDir.Name = "txtDestDir";
            this.txtDestDir.Size = new System.Drawing.Size(497, 20);
            this.txtDestDir.TabIndex = 201;
            this.txtDestDir.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDestDir_KeyPress);
            // 
            // btnDestBrowse
            // 
            this.btnDestBrowse.Location = new System.Drawing.Point(509, 19);
            this.btnDestBrowse.Name = "btnDestBrowse";
            this.btnDestBrowse.Size = new System.Drawing.Size(63, 23);
            this.btnDestBrowse.TabIndex = 202;
            this.btnDestBrowse.Text = "Browse...";
            this.btnDestBrowse.UseVisualStyleBackColor = true;
            this.btnDestBrowse.Click += new System.EventHandler(this.btnDestBrowse_Click);
            // 
            // grpBackupDetails
            // 
            this.grpBackupDetails.Controls.Add(this.chkDryRun);
            this.grpBackupDetails.Controls.Add(this.radIncremental);
            this.grpBackupDetails.Controls.Add(this.radCarbon);
            this.grpBackupDetails.Location = new System.Drawing.Point(12, 285);
            this.grpBackupDetails.Name = "grpBackupDetails";
            this.grpBackupDetails.Size = new System.Drawing.Size(659, 66);
            this.grpBackupDetails.TabIndex = 300;
            this.grpBackupDetails.TabStop = false;
            this.grpBackupDetails.Text = "Backup details";
            // 
            // radIncremental
            // 
            this.radIncremental.AutoSize = true;
            this.radIncremental.Location = new System.Drawing.Point(111, 19);
            this.radIncremental.Name = "radIncremental";
            this.radIncremental.Size = new System.Drawing.Size(119, 17);
            this.radIncremental.TabIndex = 302;
            this.radIncremental.TabStop = true;
            this.radIncremental.Text = "Incremental backup";
            this.radIncremental.UseVisualStyleBackColor = true;
            // 
            // radCarbon
            // 
            this.radCarbon.AutoSize = true;
            this.radCarbon.Location = new System.Drawing.Point(11, 19);
            this.radCarbon.Name = "radCarbon";
            this.radCarbon.Size = new System.Drawing.Size(85, 17);
            this.radCarbon.TabIndex = 301;
            this.radCarbon.TabStop = true;
            this.radCarbon.Text = "Carbon copy";
            this.radCarbon.UseVisualStyleBackColor = true;
            // 
            // grpVerbosity
            // 
            this.grpVerbosity.Controls.Add(this.lblVerbose);
            this.grpVerbosity.Controls.Add(this.lblDebug);
            this.grpVerbosity.Controls.Add(this.lblError);
            this.grpVerbosity.Controls.Add(this.lblInformational);
            this.grpVerbosity.Controls.Add(this.lblThisWillDisplay);
            this.grpVerbosity.Controls.Add(this.lstVerbosity);
            this.grpVerbosity.Location = new System.Drawing.Point(12, 357);
            this.grpVerbosity.Name = "grpVerbosity";
            this.grpVerbosity.Size = new System.Drawing.Size(659, 80);
            this.grpVerbosity.TabIndex = 400;
            this.grpVerbosity.TabStop = false;
            this.grpVerbosity.Text = "Output verbosity";
            // 
            // lblVerbose
            // 
            this.lblVerbose.AutoSize = true;
            this.lblVerbose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVerbose.Location = new System.Drawing.Point(260, 58);
            this.lblVerbose.Name = "lblVerbose";
            this.lblVerbose.Size = new System.Drawing.Size(251, 13);
            this.lblVerbose.TabIndex = 1010;
            this.lblVerbose.Text = "Verbose messages (eg. \"Copying [file1] to [file2] ...\")";
            // 
            // lblDebug
            // 
            this.lblDebug.AutoSize = true;
            this.lblDebug.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDebug.Location = new System.Drawing.Point(260, 42);
            this.lblDebug.Name = "lblDebug";
            this.lblDebug.Size = new System.Drawing.Size(268, 13);
            this.lblDebug.TabIndex = 1009;
            this.lblDebug.Text = "Debug messages (eg. \"Synchronizing [dir1] to [dir2] ...\")";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.Location = new System.Drawing.Point(260, 27);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(208, 13);
            this.lblError.TabIndex = 1008;
            this.lblError.Text = "Error messages (eg. \"Couldn\'t copy file ...\")";
            // 
            // lblInformational
            // 
            this.lblInformational.AutoSize = true;
            this.lblInformational.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInformational.Location = new System.Drawing.Point(260, 12);
            this.lblInformational.Name = "lblInformational";
            this.lblInformational.Size = new System.Drawing.Size(232, 13);
            this.lblInformational.TabIndex = 1007;
            this.lblInformational.Text = "Informational messages (eg. \"Starting backup.\")";
            // 
            // lblThisWillDisplay
            // 
            this.lblThisWillDisplay.AutoSize = true;
            this.lblThisWillDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThisWillDisplay.Location = new System.Drawing.Point(116, 51);
            this.lblThisWillDisplay.Name = "lblThisWillDisplay";
            this.lblThisWillDisplay.Size = new System.Drawing.Size(100, 13);
            this.lblThisWillDisplay.TabIndex = 1006;
            this.lblThisWillDisplay.Text = "This will display:";
            // 
            // lstVerbosity
            // 
            this.lstVerbosity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lstVerbosity.FormattingEnabled = true;
            this.lstVerbosity.Location = new System.Drawing.Point(9, 19);
            this.lstVerbosity.Name = "lstVerbosity";
            this.lstVerbosity.Size = new System.Drawing.Size(207, 21);
            this.lstVerbosity.TabIndex = 401;
            this.lstVerbosity.SelectedIndexChanged += new System.EventHandler(this.lstVerbosity_SelectedIndexChanged);
            // 
            // lblDestDir
            // 
            this.lblDestDir.AutoSize = true;
            this.lblDestDir.Location = new System.Drawing.Point(6, 48);
            this.lblDestDir.Name = "lblDestDir";
            this.lblDestDir.Size = new System.Drawing.Size(52, 13);
            this.lblDestDir.TabIndex = 12;
            this.lblDestDir.Text = "lblDestDir";
            // 
            // btnDestSet
            // 
            this.btnDestSet.Location = new System.Drawing.Point(578, 19);
            this.btnDestSet.Name = "btnDestSet";
            this.btnDestSet.Size = new System.Drawing.Size(75, 23);
            this.btnDestSet.TabIndex = 203;
            this.btnDestSet.Text = "S&et";
            this.btnDestSet.UseVisualStyleBackColor = true;
            this.btnDestSet.Click += new System.EventHandler(this.btnDestSet_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new System.Drawing.Point(503, 461);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(81, 23);
            this.btnAbout.TabIndex = 1003;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // grpSource
            // 
            this.grpSource.Controls.Add(this.lstSourceDirs);
            this.grpSource.Controls.Add(this.txtSourceDir);
            this.grpSource.Controls.Add(this.btnRemDir);
            this.grpSource.Controls.Add(this.btnAddDir);
            this.grpSource.Controls.Add(this.btnSourceBrowse);
            this.grpSource.Location = new System.Drawing.Point(12, 12);
            this.grpSource.Name = "grpSource";
            this.grpSource.Size = new System.Drawing.Size(659, 190);
            this.grpSource.TabIndex = 100;
            this.grpSource.TabStop = false;
            this.grpSource.Text = "Backup these directories";
            // 
            // grpDest
            // 
            this.grpDest.Controls.Add(this.txtDestDir);
            this.grpDest.Controls.Add(this.btnDestBrowse);
            this.grpDest.Controls.Add(this.btnDestSet);
            this.grpDest.Controls.Add(this.lblDestDir);
            this.grpDest.Location = new System.Drawing.Point(12, 208);
            this.grpDest.Name = "grpDest";
            this.grpDest.Size = new System.Drawing.Size(659, 71);
            this.grpDest.TabIndex = 200;
            this.grpDest.TabStop = false;
            this.grpDest.Text = "Backup to this destination";
            this.grpDest.Paint += new System.Windows.Forms.PaintEventHandler(this.grpDest_Paint);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(590, 461);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(81, 23);
            this.btnExit.TabIndex = 1004;
            this.btnExit.Text = "E&xit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(378, 461);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(52, 23);
            this.btnSave.TabIndex = 1001;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(436, 461);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(52, 23);
            this.btnLoad.TabIndex = 1002;
            this.btnLoad.Text = "&Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // chkDryRun
            // 
            this.chkDryRun.AutoSize = true;
            this.chkDryRun.Location = new System.Drawing.Point(11, 39);
            this.chkDryRun.Name = "chkDryRun";
            this.chkDryRun.Size = new System.Drawing.Size(358, 17);
            this.chkDryRun.TabIndex = 303;
            this.chkDryRun.Text = "Is dry run (doesn\'t modify any files/dirs; just shows what would happen)";
            this.chkDryRun.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 495);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.grpDest);
            this.Controls.Add(this.grpSource);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.grpVerbosity);
            this.Controls.Add(this.grpBackupDetails);
            this.Controls.Add(this.btnStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.Text = "Carbon Copy v?...";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmMain_Paint);
            this.grpBackupDetails.ResumeLayout(false);
            this.grpBackupDetails.PerformLayout();
            this.grpVerbosity.ResumeLayout(false);
            this.grpVerbosity.PerformLayout();
            this.grpSource.ResumeLayout(false);
            this.grpSource.PerformLayout();
            this.grpDest.ResumeLayout(false);
            this.grpDest.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lstSourceDirs;
		private System.Windows.Forms.TextBox txtSourceDir;
		private System.Windows.Forms.Button btnSourceBrowse;
		private System.Windows.Forms.Button btnAddDir;
		private System.Windows.Forms.Button btnRemDir;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.TextBox txtDestDir;
		private System.Windows.Forms.Button btnDestBrowse;
		private System.Windows.Forms.GroupBox grpBackupDetails;
		private System.Windows.Forms.RadioButton radIncremental;
		private System.Windows.Forms.RadioButton radCarbon;
		private System.Windows.Forms.GroupBox grpVerbosity;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Label lblDestDir;
		private System.Windows.Forms.Button btnDestSet;
		private System.Windows.Forms.Button btnAbout;
		private System.Windows.Forms.GroupBox grpSource;
		private System.Windows.Forms.GroupBox grpDest;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.ComboBox lstVerbosity;
		private System.Windows.Forms.Label lblThisWillDisplay;
		private System.Windows.Forms.Label lblInformational;
		private System.Windows.Forms.Label lblVerbose;
		private System.Windows.Forms.Label lblDebug;
		private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.CheckBox chkDryRun;


	}
}

