namespace CarbonCopy {
	partial class frmBackup {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBackup));
			this.btnClose = new System.Windows.Forms.Button();
			this.txtBackupOutput = new System.Windows.Forms.RichTextBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblProcessingTitle = new System.Windows.Forms.Label();
			this.lblProcessing = new System.Windows.Forms.Label();
			this.tmrProcessing = new System.Windows.Forms.Timer(this.components);
			this.pictWorking = new System.Windows.Forms.PictureBox();
			this.tmrPictAnim = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.pictWorking)).BeginInit();
			this.SuspendLayout();
			// 
			// btnClose
			// 
			resources.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.Name = "btnClose";
			this.btnClose.TabStop = false;
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// txtBackupOutput
			// 
			this.txtBackupOutput.DetectUrls = false;
			resources.ApplyResources(this.txtBackupOutput, "txtBackupOutput");
			this.txtBackupOutput.Name = "txtBackupOutput";
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabStop = false;
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lblProcessingTitle
			// 
			resources.ApplyResources(this.lblProcessingTitle, "lblProcessingTitle");
			this.lblProcessingTitle.Name = "lblProcessingTitle";
			// 
			// lblProcessing
			// 
			resources.ApplyResources(this.lblProcessing, "lblProcessing");
			this.lblProcessing.Name = "lblProcessing";
			// 
			// tmrProcessing
			// 
			this.tmrProcessing.Interval = 500;
			this.tmrProcessing.Tick += new System.EventHandler(this.tmrProcessing_Tick);
			// 
			// pictWorking
			// 
			resources.ApplyResources(this.pictWorking, "pictWorking");
			this.pictWorking.Name = "pictWorking";
			this.pictWorking.TabStop = false;
			// 
			// tmrPictAnim
			// 
			this.tmrPictAnim.Tick += new System.EventHandler(this.tmrPictAnim_Tick);
			// 
			// frmBackup
			// 
			this.AllowDrop = true;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pictWorking);
			this.Controls.Add(this.lblProcessing);
			this.Controls.Add(this.lblProcessingTitle);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.txtBackupOutput);
			this.Name = "frmBackup";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBackup_FormClosing);
			this.Load += new System.EventHandler(this.frmBackup_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictWorking)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.RichTextBox txtBackupOutput;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblProcessingTitle;
		private System.Windows.Forms.Label lblProcessing;
		private System.Windows.Forms.Timer tmrProcessing;
		private System.Windows.Forms.PictureBox pictWorking;
		private System.Windows.Forms.Timer tmrPictAnim;
	}
}