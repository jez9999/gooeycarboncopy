namespace GooeyUtilitiesTester
{
	partial class frmGooeyTree
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
			System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Node1");
			System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Node2");
			System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode17,
            treeNode18});
			System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Node5");
			System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Node6");
			System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Node4", new System.Windows.Forms.TreeNode[] {
            treeNode20,
            treeNode21});
			System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Node3", new System.Windows.Forms.TreeNode[] {
            treeNode22});
			System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Node7");
			System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Node1");
			GooeyControls.GooeyTree.GooeyTreeTagData gooeyTreeTagData9 = new GooeyControls.GooeyTree.GooeyTreeTagData();
			System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("Node2");
			GooeyControls.GooeyTree.GooeyTreeTagData gooeyTreeTagData10 = new GooeyControls.GooeyTree.GooeyTreeTagData();
			System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode25,
            treeNode26});
			GooeyControls.GooeyTree.GooeyTreeTagData gooeyTreeTagData11 = new GooeyControls.GooeyTree.GooeyTreeTagData();
			System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("Node5");
			GooeyControls.GooeyTree.GooeyTreeTagData gooeyTreeTagData12 = new GooeyControls.GooeyTree.GooeyTreeTagData();
			System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("Node6");
			GooeyControls.GooeyTree.GooeyTreeTagData gooeyTreeTagData13 = new GooeyControls.GooeyTree.GooeyTreeTagData();
			System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("Node4", new System.Windows.Forms.TreeNode[] {
            treeNode28,
            treeNode29});
			GooeyControls.GooeyTree.GooeyTreeTagData gooeyTreeTagData14 = new GooeyControls.GooeyTree.GooeyTreeTagData();
			System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("Node3", new System.Windows.Forms.TreeNode[] {
            treeNode30});
			GooeyControls.GooeyTree.GooeyTreeTagData gooeyTreeTagData15 = new GooeyControls.GooeyTree.GooeyTreeTagData();
			System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("Node7");
			GooeyControls.GooeyTree.GooeyTreeTagData gooeyTreeTagData16 = new GooeyControls.GooeyTree.GooeyTreeTagData();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.btnToggleEnabledG = new System.Windows.Forms.Button();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.gooeyTree1 = new GooeyControls.GooeyTree();
			this.btnToggleEnabledR = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(372, 42);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(438, 550);
			this.textBox1.TabIndex = 1;
			// 
			// btnToggleEnabledG
			// 
			this.btnToggleEnabledG.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnToggleEnabledG.Location = new System.Drawing.Point(373, 13);
			this.btnToggleEnabledG.Name = "btnToggleEnabledG";
			this.btnToggleEnabledG.Size = new System.Drawing.Size(162, 23);
			this.btnToggleEnabledG.TabIndex = 2;
			this.btnToggleEnabledG.Text = "Toggle GooeyTree enabled";
			this.btnToggleEnabledG.UseVisualStyleBackColor = true;
			this.btnToggleEnabledG.Click += new System.EventHandler(this.btnToggleEnabledG_Click);
			// 
			// treeView1
			// 
			this.treeView1.CheckBoxes = true;
			this.treeView1.Location = new System.Drawing.Point(12, 281);
			this.treeView1.Name = "treeView1";
			treeNode17.Name = "Node1";
			treeNode17.Text = "Node1";
			treeNode18.Name = "Node2";
			treeNode18.Text = "Node2";
			treeNode19.Name = "Node0";
			treeNode19.Text = "Node0";
			treeNode20.Name = "Node5";
			treeNode20.Text = "Node5";
			treeNode21.Name = "Node6";
			treeNode21.Text = "Node6";
			treeNode22.Name = "Node4";
			treeNode22.Text = "Node4";
			treeNode23.Name = "Node3";
			treeNode23.Text = "Node3";
			treeNode24.Name = "Node7";
			treeNode24.Text = "Node7";
			this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode19,
            treeNode23,
            treeNode24});
			this.treeView1.Size = new System.Drawing.Size(354, 311);
			this.treeView1.TabIndex = 3;
			this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(60, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "GooeyTree";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 265);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(92, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Regular TreeView";
			// 
			// gooeyTree1
			// 
			this.gooeyTree1.Location = new System.Drawing.Point(12, 29);
			this.gooeyTree1.Name = "gooeyTree1";
			treeNode25.Name = "Node1";
			treeNode25.StateImageIndex = 0;
			gooeyTreeTagData9.CheckState = GooeyControls.GooeyTree.CheckState.Unchecked;
			gooeyTreeTagData9.OtherTagInfo = null;
			treeNode25.Tag = gooeyTreeTagData9;
			treeNode25.Text = "Node1";
			treeNode26.Name = "Node2";
			treeNode26.StateImageIndex = 0;
			gooeyTreeTagData10.CheckState = GooeyControls.GooeyTree.CheckState.Unchecked;
			gooeyTreeTagData10.OtherTagInfo = null;
			treeNode26.Tag = gooeyTreeTagData10;
			treeNode26.Text = "Node2";
			treeNode27.Name = "Node0";
			treeNode27.StateImageIndex = 0;
			gooeyTreeTagData11.CheckState = GooeyControls.GooeyTree.CheckState.Unchecked;
			gooeyTreeTagData11.OtherTagInfo = null;
			treeNode27.Tag = gooeyTreeTagData11;
			treeNode27.Text = "Node0";
			treeNode28.Name = "Node5";
			treeNode28.StateImageIndex = 0;
			gooeyTreeTagData12.CheckState = GooeyControls.GooeyTree.CheckState.Unchecked;
			gooeyTreeTagData12.OtherTagInfo = null;
			treeNode28.Tag = gooeyTreeTagData12;
			treeNode28.Text = "Node5";
			treeNode29.Name = "Node6";
			treeNode29.StateImageIndex = 0;
			gooeyTreeTagData13.CheckState = GooeyControls.GooeyTree.CheckState.Unchecked;
			gooeyTreeTagData13.OtherTagInfo = null;
			treeNode29.Tag = gooeyTreeTagData13;
			treeNode29.Text = "Node6";
			treeNode30.Name = "Node4";
			treeNode30.StateImageIndex = 0;
			gooeyTreeTagData14.CheckState = GooeyControls.GooeyTree.CheckState.Unchecked;
			gooeyTreeTagData14.OtherTagInfo = null;
			treeNode30.Tag = gooeyTreeTagData14;
			treeNode30.Text = "Node4";
			treeNode31.Name = "Node3";
			treeNode31.StateImageIndex = 0;
			gooeyTreeTagData15.CheckState = GooeyControls.GooeyTree.CheckState.Unchecked;
			gooeyTreeTagData15.OtherTagInfo = null;
			treeNode31.Tag = gooeyTreeTagData15;
			treeNode31.Text = "Node3";
			treeNode32.Name = "Node7";
			treeNode32.StateImageIndex = 0;
			gooeyTreeTagData16.CheckState = GooeyControls.GooeyTree.CheckState.Unchecked;
			gooeyTreeTagData16.OtherTagInfo = null;
			treeNode32.Tag = gooeyTreeTagData16;
			treeNode32.Text = "Node7";
			this.gooeyTree1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode27,
            treeNode31,
            treeNode32});
			this.gooeyTree1.Size = new System.Drawing.Size(354, 214);
			this.gooeyTree1.TabIndex = 0;
			this.gooeyTree1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.gooeyTree1_AfterCheck);
			this.gooeyTree1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.gooeyTree1_AfterSelect);
			// 
			// btnToggleEnabledR
			// 
			this.btnToggleEnabledR.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnToggleEnabledR.Location = new System.Drawing.Point(541, 13);
			this.btnToggleEnabledR.Name = "btnToggleEnabledR";
			this.btnToggleEnabledR.Size = new System.Drawing.Size(162, 23);
			this.btnToggleEnabledR.TabIndex = 6;
			this.btnToggleEnabledR.Text = "Toggle RegularTree enabled";
			this.btnToggleEnabledR.UseVisualStyleBackColor = true;
			this.btnToggleEnabledR.Click += new System.EventHandler(this.btnToggleEnabledR_Click);
			// 
			// frmGooeyTree
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(822, 604);
			this.Controls.Add(this.btnToggleEnabledR);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.treeView1);
			this.Controls.Add(this.btnToggleEnabledG);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.gooeyTree1);
			this.Name = "frmGooeyTree";
			this.Text = "frmGooeyTree";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private GooeyControls.GooeyTree gooeyTree1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button btnToggleEnabledG;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnToggleEnabledR;
	}
}