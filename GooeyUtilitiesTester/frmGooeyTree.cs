using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GooeyControls;

namespace GooeyUtilitiesTester
{
	public partial class frmGooeyTree : Form {
		public frmGooeyTree() {
			InitializeComponent();
		}
		
		private void gooeyTree1_AfterSelect(object sender, TreeViewEventArgs ea) {
//			textBox1.Text += "GooeyTree AfterSelect...\r\n";
		}
		
		private void gooeyTree1_AfterCheck(object sender, TreeViewEventArgs ea) {
			textBox1.Text = "";
			
			textBox1.Text += "GooeyTree node status:\r\n\r\n";
			foreach (TreeNode node in gooeyTree1.Nodes) {
				addNodeText(node);
			}
			textBox1.Text += "\r\n";
			
			textBox1.Text += "Last update triggered by AfterCheck event for:\r\nNode: " + ea.Node.Text + "\r\nAction: " + ea.Action.ToString();
			textBox1.Text += "\r\n";
			
			textBox1.Text += "Bounds of last node (x, y, w, h): " + ea.Node.Bounds.X + ", " + ea.Node.Bounds.Y + ", " + ea.Node.Bounds.Width + ", " + ea.Node.Bounds.Height;
		}
		
		private void addNodeText(TreeNode node) {
			textBox1.Text += node.Text + ": " + ((GooeyTree.GooeyTreeTagData)node.Tag).CheckState.ToString() + "\r\n";
			foreach (TreeNode child in node.Nodes) {
				addNodeText(child);
			}
		}
		
		private void treeView1_AfterSelect(object sender, TreeViewEventArgs ea) {
//			textBox1.Text += "Regular TreeView AfterSelect...\r\n";
		}
		
		private void treeView1_AfterCheck(object sender, TreeViewEventArgs ea) {
//			textBox1.Text += "Regular TreeView AfterCheck...\r\n";
		}
		
		private void btnToggleEnabledG_Click(object sender, EventArgs e) {
			gooeyTree1.Enabled = !gooeyTree1.Enabled;
		}
		
		private void btnToggleEnabledR_Click(object sender, EventArgs e) {
			treeView1.Enabled = !treeView1.Enabled;
		}
	}
}
