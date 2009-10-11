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
			this.components = new System.ComponentModel.Container();
			this.tbOutput = new System.Windows.Forms.TextBox();
			this.btnTestBtree = new System.Windows.Forms.Button();
			this.btnTestGooeyTree = new System.Windows.Forms.Button();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.button1 = new System.Windows.Forms.Button();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
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
			// btnTestBtree
			// 
			this.btnTestBtree.Location = new System.Drawing.Point(12, 12);
			this.btnTestBtree.Name = "btnTestBtree";
			this.btnTestBtree.Size = new System.Drawing.Size(112, 23);
			this.btnTestBtree.TabIndex = 1;
			this.btnTestBtree.Text = "Test GooeyBTree";
			this.btnTestBtree.UseVisualStyleBackColor = true;
			this.btnTestBtree.Click += new System.EventHandler(this.btntestBtree_Click);
			// 
			// btnTestGooeyTree
			// 
			this.btnTestGooeyTree.Location = new System.Drawing.Point(744, 12);
			this.btnTestGooeyTree.Name = "btnTestGooeyTree";
			this.btnTestGooeyTree.Size = new System.Drawing.Size(136, 23);
			this.btnTestGooeyTree.TabIndex = 2;
			this.btnTestGooeyTree.Text = "Test GooeyTree";
			this.btnTestGooeyTree.UseVisualStyleBackColor = true;
			this.btnTestGooeyTree.Click += new System.EventHandler(this.btnTestGooeyTree_Click);
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.Text = "notifyIcon1";
			this.notifyIcon1.Visible = true;
			this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(259, 12);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "Modal dialog";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox1.Location = new System.Drawing.Point(340, 16);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(117, 17);
			this.checkBox1.TabIndex = 4;
			this.checkBox1.Text = "Populate tray menu";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(892, 540);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.btnTestGooeyTree);
			this.Controls.Add(this.btnTestBtree);
			this.Controls.Add(this.tbOutput);
			this.Name = "frmMain";
			this.Text = "Gooey Utilities tester app";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbOutput;
		private System.Windows.Forms.Button btnTestBtree;
		private System.Windows.Forms.Button btnTestGooeyTree;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckBox checkBox1;
	}
}

