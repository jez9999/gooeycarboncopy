namespace GooeyUtilitiesTester
{
	partial class frmMain
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
			this.tbOutput = new System.Windows.Forms.TextBox();
			this.btntestBtree = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tbOutput
			// 
			this.tbOutput.Location = new System.Drawing.Point(12, 41);
			this.tbOutput.Multiline = true;
			this.tbOutput.Name = "tbOutput";
			this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbOutput.Size = new System.Drawing.Size(868, 487);
			this.tbOutput.TabIndex = 0;
			// 
			// btntestBtree
			// 
			this.btntestBtree.Location = new System.Drawing.Point(12, 12);
			this.btntestBtree.Name = "btntestBtree";
			this.btntestBtree.Size = new System.Drawing.Size(112, 23);
			this.btntestBtree.TabIndex = 1;
			this.btntestBtree.Text = "Test GooeyBTree";
			this.btntestBtree.UseVisualStyleBackColor = true;
			this.btntestBtree.Click += new System.EventHandler(this.btntestBtree_Click);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(892, 540);
			this.Controls.Add(this.btntestBtree);
			this.Controls.Add(this.tbOutput);
			this.Name = "frmMain";
			this.Text = "Gooey Utilities tester app";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbOutput;
		private System.Windows.Forms.Button btntestBtree;
	}
}

