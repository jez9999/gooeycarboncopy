namespace CarbonCopy {
	partial class frmAbout {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
			this.lblLicence = new System.Windows.Forms.Label();
			this.txtGplLicence = new System.Windows.Forms.TextBox();
			this.lblLicenced = new System.Windows.Forms.Label();
			this.lblVer = new System.Windows.Forms.Label();
			this.picGooeyLogo = new System.Windows.Forms.PictureBox();
			this.lblCopyright = new System.Windows.Forms.Label();
			this.lnkGooeySite = new System.Windows.Forms.LinkLabel();
			this.btnExit = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.picGooeyLogo)).BeginInit();
			this.SuspendLayout();
			// 
			// lblLicence
			// 
			this.lblLicence.AutoSize = true;
			this.lblLicence.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLicence.Location = new System.Drawing.Point(104, 2);
			this.lblLicence.Name = "lblLicence";
			this.lblLicence.Size = new System.Drawing.Size(122, 23);
			this.lblLicence.TabIndex = 6;
			this.lblLicence.Text = "Carbon Copy";
			// 
			// txtGplLicence
			// 
			this.txtGplLicence.BackColor = System.Drawing.SystemColors.Info;
			this.txtGplLicence.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtGplLicence.Location = new System.Drawing.Point(12, 49);
			this.txtGplLicence.MaxLength = 2147483640;
			this.txtGplLicence.Multiline = true;
			this.txtGplLicence.Name = "txtGplLicence";
			this.txtGplLicence.ReadOnly = true;
			this.txtGplLicence.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtGplLicence.Size = new System.Drawing.Size(306, 338);
			this.txtGplLicence.TabIndex = 8;
			this.txtGplLicence.TabStop = false;
			// 
			// lblLicenced
			// 
			this.lblLicenced.AutoSize = true;
			this.lblLicenced.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLicenced.Location = new System.Drawing.Point(12, 32);
			this.lblLicenced.Name = "lblLicenced";
			this.lblLicenced.Size = new System.Drawing.Size(138, 14);
			this.lblLicenced.TabIndex = 7;
			this.lblLicenced.Text = "Licenced under the GPLv2:";
			// 
			// lblVer
			// 
			this.lblVer.AutoSize = true;
			this.lblVer.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblVer.Location = new System.Drawing.Point(222, 9);
			this.lblVer.Name = "lblVer";
			this.lblVer.Size = new System.Drawing.Size(35, 14);
			this.lblVer.TabIndex = 12;
			this.lblVer.Text = "v$ver";
			// 
			// picGooeyLogo
			// 
			this.picGooeyLogo.Image = ((System.Drawing.Image)(resources.GetObject("picGooeyLogo.Image")));
			this.picGooeyLogo.Location = new System.Drawing.Point(32, 400);
			this.picGooeyLogo.Name = "picGooeyLogo";
			this.picGooeyLogo.Size = new System.Drawing.Size(268, 92);
			this.picGooeyLogo.TabIndex = 14;
			this.picGooeyLogo.TabStop = false;
			// 
			// lblCopyright
			// 
			this.lblCopyright.AutoSize = true;
			this.lblCopyright.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCopyright.Location = new System.Drawing.Point(143, 495);
			this.lblCopyright.Name = "lblCopyright";
			this.lblCopyright.Size = new System.Drawing.Size(175, 14);
			this.lblCopyright.TabIndex = 4;
			this.lblCopyright.Text = "Copyright © Gooey Software 2017";
			// 
			// lnkGooeySite
			// 
			this.lnkGooeySite.AutoSize = true;
			this.lnkGooeySite.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lnkGooeySite.Location = new System.Drawing.Point(180, 509);
			this.lnkGooeySite.Name = "lnkGooeySite";
			this.lnkGooeySite.Size = new System.Drawing.Size(137, 14);
			this.lnkGooeySite.TabIndex = 10;
			this.lnkGooeySite.TabStop = true;
			this.lnkGooeySite.Text = "www.gooeysoftware.com";
			this.lnkGooeySite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGooeySite_LinkClicked);
			// 
			// btnExit
			// 
			this.btnExit.Location = new System.Drawing.Point(12, 496);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(75, 23);
			this.btnExit.TabIndex = 11;
			this.btnExit.Text = "Exit";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// frmAbout
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(329, 528);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.lnkGooeySite);
			this.Controls.Add(this.lblCopyright);
			this.Controls.Add(this.picGooeyLogo);
			this.Controls.Add(this.lblVer);
			this.Controls.Add(this.lblLicenced);
			this.Controls.Add(this.txtGplLicence);
			this.Controls.Add(this.lblLicence);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAbout";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About...";
			this.Load += new System.EventHandler(this.frmAbout_Load);
			((System.ComponentModel.ISupportInitialize)(this.picGooeyLogo)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblLicence;
		private System.Windows.Forms.TextBox txtGplLicence;
		private System.Windows.Forms.Label lblLicenced;
		private System.Windows.Forms.Label lblVer;
		private System.Windows.Forms.PictureBox picGooeyLogo;
		private System.Windows.Forms.Label lblCopyright;
		private System.Windows.Forms.LinkLabel lnkGooeySite;
		private System.Windows.Forms.Button btnExit;
	}
}