namespace Carbon_Copy {
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.lstSourceDirs = new System.Windows.Forms.ListBox();
			this.txtSourceDir = new System.Windows.Forms.TextBox();
			this.btnSourceBrowse = new System.Windows.Forms.Button();
			this.btnAddDir = new System.Windows.Forms.Button();
			this.btnRemDir = new System.Windows.Forms.Button();
			this.btnStart = new System.Windows.Forms.Button();
			this.txtDestDir = new System.Windows.Forms.TextBox();
			this.btnDestBrowse = new System.Windows.Forms.Button();
			this.grpCarbonIncrement = new System.Windows.Forms.GroupBox();
			this.radIncremental = new System.Windows.Forms.RadioButton();
			this.radCarbon = new System.Windows.Forms.RadioButton();
			this.grpDisplay = new System.Windows.Forms.GroupBox();
			this.lblVerboseWarning = new System.Windows.Forms.Label();
			this.chkVerbose = new System.Windows.Forms.CheckBox();
			this.chkComments = new System.Windows.Forms.CheckBox();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.lblDestDir = new System.Windows.Forms.Label();
			this.btnDestSet = new System.Windows.Forms.Button();
			this.btnAbout = new System.Windows.Forms.Button();
			this.ttpQuick = new System.Windows.Forms.ToolTip(this.components);
			this.grpSource = new System.Windows.Forms.GroupBox();
			this.grpDest = new System.Windows.Forms.GroupBox();
			this.btnExit = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnLoad = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.grpCarbonIncrement.SuspendLayout();
			this.grpDisplay.SuspendLayout();
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
			this.btnStart.Location = new System.Drawing.Point(12, 431);
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
			// grpCarbonIncrement
			// 
			this.grpCarbonIncrement.Controls.Add(this.radIncremental);
			this.grpCarbonIncrement.Controls.Add(this.radCarbon);
			this.grpCarbonIncrement.Location = new System.Drawing.Point(12, 285);
			this.grpCarbonIncrement.Name = "grpCarbonIncrement";
			this.grpCarbonIncrement.Size = new System.Drawing.Size(659, 66);
			this.grpCarbonIncrement.TabIndex = 300;
			this.grpCarbonIncrement.TabStop = false;
			this.grpCarbonIncrement.Text = "Carbon or incremental?";
			// 
			// radIncremental
			// 
			this.radIncremental.AutoSize = true;
			this.radIncremental.Location = new System.Drawing.Point(6, 42);
			this.radIncremental.Name = "radIncremental";
			this.radIncremental.Size = new System.Drawing.Size(80, 17);
			this.radIncremental.TabIndex = 302;
			this.radIncremental.TabStop = true;
			this.radIncremental.Text = "Incremental";
			this.radIncremental.UseVisualStyleBackColor = true;
			// 
			// radCarbon
			// 
			this.radCarbon.AutoSize = true;
			this.radCarbon.Location = new System.Drawing.Point(6, 19);
			this.radCarbon.Name = "radCarbon";
			this.radCarbon.Size = new System.Drawing.Size(85, 17);
			this.radCarbon.TabIndex = 301;
			this.radCarbon.TabStop = true;
			this.radCarbon.Text = "Carbon copy";
			this.radCarbon.UseVisualStyleBackColor = true;
			// 
			// grpDisplay
			// 
			this.grpDisplay.Controls.Add(this.lblVerboseWarning);
			this.grpDisplay.Controls.Add(this.chkVerbose);
			this.grpDisplay.Controls.Add(this.chkComments);
			this.grpDisplay.Location = new System.Drawing.Point(12, 357);
			this.grpDisplay.Name = "grpDisplay";
			this.grpDisplay.Size = new System.Drawing.Size(659, 68);
			this.grpDisplay.TabIndex = 400;
			this.grpDisplay.TabStop = false;
			this.grpDisplay.Text = "Display...?";
			// 
			// lblVerboseWarning
			// 
			this.lblVerboseWarning.AutoSize = true;
			this.lblVerboseWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblVerboseWarning.Location = new System.Drawing.Point(226, 45);
			this.lblVerboseWarning.Name = "lblVerboseWarning";
			this.lblVerboseWarning.Size = new System.Drawing.Size(164, 13);
			this.lblVerboseWarning.TabIndex = 0;
			this.lblVerboseWarning.Text = "WARNING (hover over me!)";
			// 
			// chkVerbose
			// 
			this.chkVerbose.AutoSize = true;
			this.chkVerbose.ForeColor = System.Drawing.Color.Blue;
			this.chkVerbose.Location = new System.Drawing.Point(7, 42);
			this.chkVerbose.Name = "chkVerbose";
			this.chkVerbose.Size = new System.Drawing.Size(222, 17);
			this.chkVerbose.TabIndex = 402;
			this.chkVerbose.Text = "Verbose (eg. \"Replacing filename.doc...\")";
			this.chkVerbose.UseVisualStyleBackColor = true;
			// 
			// chkComments
			// 
			this.chkComments.AutoSize = true;
			this.chkComments.ForeColor = System.Drawing.Color.Green;
			this.chkComments.Location = new System.Drawing.Point(7, 20);
			this.chkComments.Name = "chkComments";
			this.chkComments.Size = new System.Drawing.Size(198, 17);
			this.chkComments.TabIndex = 401;
			this.chkComments.Text = "Comments (eg. \"Base directory is...\")";
			this.chkComments.UseVisualStyleBackColor = true;
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
			this.btnAbout.Location = new System.Drawing.Point(503, 431);
			this.btnAbout.Name = "btnAbout";
			this.btnAbout.Size = new System.Drawing.Size(81, 23);
			this.btnAbout.TabIndex = 1003;
			this.btnAbout.Text = "About";
			this.btnAbout.UseVisualStyleBackColor = true;
			this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
			// 
			// ttpQuick
			// 
			this.ttpQuick.ShowAlways = true;
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
			this.btnExit.Location = new System.Drawing.Point(590, 431);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(81, 23);
			this.btnExit.TabIndex = 1004;
			this.btnExit.Text = "E&xit";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(378, 431);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(52, 23);
			this.btnSave.TabIndex = 1001;
			this.btnSave.Text = "&Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnLoad
			// 
			this.btnLoad.Location = new System.Drawing.Point(436, 431);
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
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(683, 462);
			this.Controls.Add(this.btnLoad);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.grpDest);
			this.Controls.Add(this.grpSource);
			this.Controls.Add(this.btnAbout);
			this.Controls.Add(this.grpDisplay);
			this.Controls.Add(this.grpCarbonIncrement);
			this.Controls.Add(this.btnStart);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmMain";
			this.Text = "Carbon Copy v?...";
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmMain_Paint);
			this.grpCarbonIncrement.ResumeLayout(false);
			this.grpCarbonIncrement.PerformLayout();
			this.grpDisplay.ResumeLayout(false);
			this.grpDisplay.PerformLayout();
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
		private System.Windows.Forms.GroupBox grpCarbonIncrement;
		private System.Windows.Forms.RadioButton radIncremental;
		private System.Windows.Forms.RadioButton radCarbon;
		private System.Windows.Forms.GroupBox grpDisplay;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Label lblDestDir;
		private System.Windows.Forms.Button btnDestSet;
		private System.Windows.Forms.CheckBox chkVerbose;
		private System.Windows.Forms.CheckBox chkComments;
		private System.Windows.Forms.Button btnAbout;
		private System.Windows.Forms.Label lblVerboseWarning;
		private System.Windows.Forms.ToolTip ttpQuick;
		private System.Windows.Forms.GroupBox grpSource;
		private System.Windows.Forms.GroupBox grpDest;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;


	}
}

