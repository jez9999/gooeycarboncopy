using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Gooey;
using System.Reflection;

namespace GooeyUtilitiesTester
{
	public partial class frmMain : Form {
		private Icon icoTray = new Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("GooeyUtilitiesTester.ccApp.ico"));
		private ContextMenu iconMenu;
		private bool popTrayMenu = true;
		
		public frmMain() {
			InitializeComponent();
			
			// Create and populate icon menu
			iconMenu = new ContextMenu();
			this.notifyIcon1.Icon = this.icoTray;
			this.notifyIcon1.Visible = true;
			this.notifyIcon1.ContextMenu = this.iconMenu;
			iconMenu.Popup += new EventHandler(iconMenu_Popup);
		}
		
		private void button1_Click(object sender, EventArgs e) {
			// Show modal dialog
			Utilities utils = new Utilities();
			utils.ShowInfo("Modal dialog box...");
		}
		
		private void iconMenu_Popup(object sender, EventArgs e) {
			this.iconMenu.MenuItems.Clear();
			if (this.popTrayMenu) {
				this.iconMenu.MenuItems.Add(new MenuItem("Test1", new EventHandler(iconMenuExit_Click)));
				this.iconMenu.MenuItems.Add(new MenuItem("Test2", new EventHandler(iconMenuExit_Click)));
				this.iconMenu.MenuItems.Add(new MenuItem("Test3", new EventHandler(iconMenuExit_Click)));
			}
		}
		
		private void iconMenuExit_Click(object sender, EventArgs ea) {
			// Exit app by closing main form
			this.Close();
		}
		
		private void btntestBtree_Click(object sender, EventArgs e) {
			GooeyBTree<UInt32, string> eventsTree = new GooeyBTree<UInt32, string>(false);
			
			tbOutput.Text += "Smallest node, given that we haven't added anything yet (should be null): " + (eventsTree.GetSmallestNode() == null ? "null" : "not null") + "\r\n";
			tbOutput.Text += "Largest node, given that we haven't added anything yet (should be null): " + (eventsTree.GetLargestNode() == null ? "null" : "not null") + "\r\n";
			
			tbOutput.Text += "\r\n";
			
			eventsTree.AddNode(5, "five");
			eventsTree.AddNode(2, "two");
			eventsTree.AddNode(8, "eight");
			eventsTree.AddNode(9, "nine");
			eventsTree.AddNode(1, "one");
			eventsTree.AddNode(3, "three");
			eventsTree.AddNode(6, "six");
			eventsTree.AddNode(4, "four");
			eventsTree.AddNode(7, "seven");
			eventsTree.AddNode(3, "threeModified");
			
			GooeyBTreeNode<UInt32, string> oldNode;
			GooeyBTreeNode<UInt32, string> node;
			tbOutput.Text += "Starting from smallest node:\r\n";
			node = eventsTree.GetSmallestNode();
			
			oldNode = node;
			while (node != null) {
				oldNode = node;
				tbOutput.Text += node.Key + ": " + node.Value + "\r\n";
				node = eventsTree.GetNextNode(node);
			}
			tbOutput.Text += "... and back to smallest...\r\n";
			node = oldNode;
			while (node != null) {
				oldNode = node;
				tbOutput.Text += node.Key + ": " + node.Value + "\r\n";
				node = eventsTree.GetPreviousNode(node);
			}
			
			tbOutput.Text += "\r\n";
			
			tbOutput.Text += "Starting from largest node:\r\n";
			node = eventsTree.GetLargestNode();
			
			oldNode = node;
			while (node != null) {
				oldNode = node;
				tbOutput.Text += node.Key + ": " + node.Value + "\r\n";
				node = eventsTree.GetPreviousNode(node);
			}
			tbOutput.Text += "... and back to largest...\r\n";
			node = oldNode;
			while (node != null) {
				oldNode = node;
				tbOutput.Text += node.Key + ": " + node.Value + "\r\n";
				node = eventsTree.GetNextNode(node);
			}
			
			tbOutput.Text += "\r\n";
			tbOutput.Text += "And now we should get an exception because of a key clash when we try to set a key that already exists in the tree:\r\n";
			
			bool gotException = false;
			try {
				eventsTree.ExceptionOnNodeKeyClash = true;
				eventsTree.AddNode(3, "threeModifiedAgain");
			}
			catch (GooeyNodeAlreadyExistsException) {
				gotException = true;
				tbOutput.Text += "Caught GooeyNodeAlreadyExistsException!\r\n";
			}
			if (!gotException) {
				tbOutput.Text += "There was a problem - we didn't get an exception!\r\n";
			}
			
			tbOutput.Text += "\r\n";
			
			tbOutput.Text += "Finding node with key 6 (should exist):\r\n";
			node = eventsTree.GetNode(6);
			if (node == null) { tbOutput.Text += "Doesn't exist!\r\n"; }
			else { tbOutput.Text += node.Key + ": " + node.Value + "\r\n"; }
			
			tbOutput.Text += "Finding node with key 11 (shouldn't exist):\r\n";
			node = eventsTree.GetNode(11);
			if (node == null) { tbOutput.Text += "Doesn't exist!\r\n"; }
			else { tbOutput.Text += node.Key + ": " + node.Value + "\r\n"; }
			
			tbOutput.Text += "\r\n";
			
			tbOutput.Text += "Deleting nodes 4, 2, and 15 (15 should fail)...\r\n";
			if (eventsTree.DeleteNode(4)) { tbOutput.Text += "Could delete node 4.\r\n"; }
			else { tbOutput.Text += "Could NOT delete node 4!  Not found!\r\n"; }
			if (eventsTree.DeleteNode(2)) { tbOutput.Text += "Could delete node 2.\r\n"; }
			else { tbOutput.Text += "Could NOT delete node 2!  Not found!\r\n"; }
			if (eventsTree.DeleteNode(15)) { tbOutput.Text += "Could delete node 15.\r\n"; }
			else { tbOutput.Text += "Could NOT delete node 15!  Not found!\r\n"; }
			
			tbOutput.Text += "\r\n";
			
			tbOutput.Text += "Now let's traverse again:\r\n";
			node = eventsTree.GetSmallestNode();
			
			oldNode = node;
			while (node != null) {
				oldNode = node;
				tbOutput.Text += node.Key + ": " + node.Value + "\r\n";
				node = eventsTree.GetNextNode(node);
			}
			tbOutput.Text += "... and back to smallest...\r\n";
			node = oldNode;
			while (node != null) {
				oldNode = node;
				tbOutput.Text += node.Key + ": " + node.Value + "\r\n";
				node = eventsTree.GetPreviousNode(node);
			}
			
			tbOutput.Text += "\r\n";
			
			tbOutput.Text += "Let's try iterating thru with a foreach:\r\n";
			foreach (var eventItem in eventsTree) {
				tbOutput.Text += "Val: " + eventItem.Key + "," + eventItem.Value + "\r\n";
			}
			
			eventsTree[15] = "fifteen";
			eventsTree[12] = "twelve";
			eventsTree.ExceptionOnNodeKeyClash = false;
			eventsTree[15] = "fifteeddn";
			
			tbOutput.Text += "Let's try iterating thru with a foreach2:\r\n";
			foreach (var eventItem in eventsTree) {
				tbOutput.Text += "Val: " + eventItem.Key + "," + eventItem.Value + "\r\n";
			}
			
			tbOutput.Text += "Clearing the collection (size now is " + eventsTree.Count + ")...\r\n";
			eventsTree.Clear();
			tbOutput.Text += "Size now is " + eventsTree.Count + ".\r\n";
			
			tbOutput.Text += "\r\n";
			
			tbOutput.Text += "Let's try iterating thru with a foreach again:\r\n";
			foreach (var eventItem in eventsTree) {
				tbOutput.Text += "Val: " + eventItem.Key + "," + eventItem.Value + "\r\n";
			}
			
			
			
			
			//GooeyBTree<int, string> eventsTree = new GooeyBTree<int, string>();
		}
		
		private void btnTestGooeyTree_Click(object sender, EventArgs e) {
			Form frmGooeyTree = new frmGooeyTree();
			frmGooeyTree.ShowDialog();
		}

		private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
		{

		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (((CheckBox)sender).Checked) {
				this.popTrayMenu = true;
			}
			else {
				this.popTrayMenu = false;
			}
		}
	}
}
