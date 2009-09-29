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
			System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node1");
			System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node2");
			System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
			System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Node5");
			System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Node6");
			System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Node4", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5});
			System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Node3", new System.Windows.Forms.TreeNode[] {
            treeNode6});
			System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Node7");
			System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Node1");
			GooeyControls.GooeyTree.GooeyTreeTagData gooeyTreeTagData1 = new GooeyControls.GooeyTree.GooeyTreeTagData();
			System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Node2");
			GooeyControls.GooeyTree.GooeyTreeTagData gooeyTreeTagData2 = new GooeyControls.GooeyTree.GooeyTreeTagData();
			System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode9,
            treeNode10});
			GooeyControls.GooeyTree.GooeyTreeTagData gooeyTreeTagData3 = new GooeyControls.GooeyTree.GooeyTreeTagData();
			System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Node5");
			GooeyControls.GooeyTree.GooeyTreeTagData gooeyTreeTagData4 = new GooeyControls.GooeyTree.GooeyTreeTagData();
			System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Node6");
			GooeyControls.GooeyTree.GooeyTreeTagData gooeyTreeTagData5 = new GooeyControls.GooeyTree.GooeyTreeTagData();
			System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Node4", new System.Windows.Forms.TreeNode[] {
            treeNode12,
            treeNode13});
			GooeyControls.GooeyTree.GooeyTreeTagData gooeyTreeTagData6 = new GooeyControls.GooeyTree.GooeyTreeTagData();
			System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Node3", new System.Windows.Forms.TreeNode[] {
            treeNode14});
			GooeyControls.GooeyTree.GooeyTreeTagData gooeyTreeTagData7 = new GooeyControls.GooeyTree.GooeyTreeTagData();
			System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Node7");
			GooeyControls.GooeyTree.GooeyTreeTagData gooeyTreeTagData8 = new GooeyControls.GooeyTree.GooeyTreeTagData();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.btnToggleEnabledG = new System.Windows.Forms.Button();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnToggleEnabledR = new System.Windows.Forms.Button();
			this.gooeyTree1 = new GooeyControls.GooeyTree();
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
			treeNode1.Name = "Node1";
			treeNode1.Text = "Node1";
			treeNode2.Name = "Node2";
			treeNode2.Text = "Node2";
			treeNode3.Name = "Node0";
			treeNode3.Text = "Node0";
			treeNode4.Name = "Node5";
			treeNode4.Text = "Node5";
			treeNode5.Name = "Node6";
			treeNode5.Text = "Node6";
			treeNode6.Name = "Node4";
			treeNode6.Text = "Node4";
			treeNode7.Name = "Node3";
			treeNode7.Text = "Node3";
			treeNode8.Name = "Node7";
			treeNode8.Text = "Node7";
			this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode7,
            treeNode8});
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
			// gooeyTree1
			// 
			this.gooeyTree1.Location = new System.Drawing.Point(12, 29);
			this.gooeyTree1.Name = "gooeyTree1";
			treeNode9.Name = "Node1";
			treeNode9.StateImageIndex = 0;
			gooeyTreeTagData1.CheckState = GooeyControls.GooeyTree.CheckState.Unchecked;
			gooeyTreeTagData1.OtherTagInfo = null;
			treeNode9.Tag = gooeyTreeTagData1;
			treeNode9.Text = "Node1";
			treeNode10.Name = "Node2";
			treeNode10.StateImageIndex = 0;
			gooeyTreeTagData2.CheckState = GooeyControls.GooeyTree.CheckState.Unchecked;
			gooeyTreeTagData2.OtherTagInfo = null;
			treeNode10.Tag = gooeyTreeTagData2;
			treeNode10.Text = "Node2";
			treeNode11.Name = "Node0";
			treeNode11.StateImageIndex = 0;
			gooeyTreeTagData3.CheckState = GooeyControls.GooeyTree.CheckState.Unchecked;
			gooeyTreeTagData3.OtherTagInfo = null;
			treeNode11.Tag = gooeyTreeTagData3;
			treeNode11.Text = "Node0";
			treeNode12.Name = "Node5";
			treeNode12.StateImageIndex = 0;
			gooeyTreeTagData4.CheckState = GooeyControls.GooeyTree.CheckState.Unchecked;
			gooeyTreeTagData4.OtherTagInfo = null;
			treeNode12.Tag = gooeyTreeTagData4;
			treeNode12.Text = "Node5";
			treeNode13.Name = "Node6";
			treeNode13.StateImageIndex = 0;
			gooeyTreeTagData5.CheckState = GooeyControls.GooeyTree.CheckState.Unchecked;
			gooeyTreeTagData5.OtherTagInfo = null;
			treeNode13.Tag = gooeyTreeTagData5;
			treeNode13.Text = "Node6";
			treeNode14.Name = "Node4";
			treeNode14.StateImageIndex = 0;
			gooeyTreeTagData6.CheckState = GooeyControls.GooeyTree.CheckState.Unchecked;
			gooeyTreeTagData6.OtherTagInfo = null;
			treeNode14.Tag = gooeyTreeTagData6;
			treeNode14.Text = "Node4";
			treeNode15.Name = "Node3";
			treeNode15.StateImageIndex = 0;
			gooeyTreeTagData7.CheckState = GooeyControls.GooeyTree.CheckState.Unchecked;
			gooeyTreeTagData7.OtherTagInfo = null;
			treeNode15.Tag = gooeyTreeTagData7;
			treeNode15.Text = "Node3";
			treeNode16.Name = "Node7";
			treeNode16.StateImageIndex = 0;
			gooeyTreeTagData8.CheckState = GooeyControls.GooeyTree.CheckState.Unchecked;
			gooeyTreeTagData8.OtherTagInfo = null;
			treeNode16.Tag = gooeyTreeTagData8;
			treeNode16.Text = "Node7";
			this.gooeyTree1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode11,
            treeNode15,
            treeNode16});
			this.gooeyTree1.Size = new System.Drawing.Size(354, 214);
			this.gooeyTree1.TabIndex = 0;
			this.gooeyTree1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.gooeyTree1_AfterCheck);
			this.gooeyTree1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.gooeyTree1_AfterSelect);
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