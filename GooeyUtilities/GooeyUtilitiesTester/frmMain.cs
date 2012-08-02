using System;
using System.Xml;
using System.Xml.XPath;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using Gooey;

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
			
			tbOutput.Text = "";

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

		private void btnTestGetFragmentFromFileUrl_Click(object sender, EventArgs e)
		{
			Utilities utils = new Utilities();

			string urlWithDrivename = @"file://C:";
			string urlWithNoExt = @"file://C:\path\to\codebase";
			string urlWithFullPath = @"file://C:\path\to\codebase.exe";
			string urlWithFullPathMultipleExt = @"file://C:\path\to\codebase.exe.exe.exe";

			tbOutput.Text = "";

			// Test with drivename only
			tbOutput.Text += "Try to get fragments from " + urlWithDrivename + "\r\n";
			tbOutput.Text += "Drivename: " + utils.GetFragmentFromFileUrl(urlWithDrivename, Utilities.FileUrlFragmentPart.DriveName) + "\r\n";
			tbOutput.Text += "Path: " + utils.GetFragmentFromFileUrl(urlWithDrivename, Utilities.FileUrlFragmentPart.Path) + "\r\n";
			tbOutput.Text += "Filename: " + utils.GetFragmentFromFileUrl(urlWithDrivename, Utilities.FileUrlFragmentPart.FileName) + "\r\n";
			tbOutput.Text += "Filebody: " + utils.GetFragmentFromFileUrl(urlWithDrivename, Utilities.FileUrlFragmentPart.FileBody) + "\r\n";
			tbOutput.Text += "Fileext: " + utils.GetFragmentFromFileUrl(urlWithDrivename, Utilities.FileUrlFragmentPart.FileExt) + "\r\n";

			// Test with no extension
			tbOutput.Text += "\r\n";
			tbOutput.Text += "Try to get fragments from " + urlWithNoExt + "\r\n";
			tbOutput.Text += "Drivename: " + utils.GetFragmentFromFileUrl(urlWithNoExt, Utilities.FileUrlFragmentPart.DriveName) + "\r\n";
			tbOutput.Text += "Path: " + utils.GetFragmentFromFileUrl(urlWithNoExt, Utilities.FileUrlFragmentPart.Path) + "\r\n";
			tbOutput.Text += "Filename: " + utils.GetFragmentFromFileUrl(urlWithNoExt, Utilities.FileUrlFragmentPart.FileName) + "\r\n";
			tbOutput.Text += "Filebody: " + utils.GetFragmentFromFileUrl(urlWithNoExt, Utilities.FileUrlFragmentPart.FileBody) + "\r\n";
			tbOutput.Text += "Fileext: " + utils.GetFragmentFromFileUrl(urlWithNoExt, Utilities.FileUrlFragmentPart.FileExt) + "\r\n";

			// Test with full URL
			tbOutput.Text += "\r\n";
			tbOutput.Text += "Try to get fragments from " + urlWithFullPath + "\r\n";
			tbOutput.Text += "Drivename: " + utils.GetFragmentFromFileUrl(urlWithFullPath, Utilities.FileUrlFragmentPart.DriveName) + "\r\n";
			tbOutput.Text += "Path: " + utils.GetFragmentFromFileUrl(urlWithFullPath, Utilities.FileUrlFragmentPart.Path) + "\r\n";
			tbOutput.Text += "Filename: " + utils.GetFragmentFromFileUrl(urlWithFullPath, Utilities.FileUrlFragmentPart.FileName) + "\r\n";
			tbOutput.Text += "Filebody: " + utils.GetFragmentFromFileUrl(urlWithFullPath, Utilities.FileUrlFragmentPart.FileBody) + "\r\n";
			tbOutput.Text += "Fileext: " + utils.GetFragmentFromFileUrl(urlWithFullPath, Utilities.FileUrlFragmentPart.FileExt) + "\r\n";

			// Test with full URL with multiple extensions
			tbOutput.Text += "\r\n";
			tbOutput.Text += "Try to get fragments from " + urlWithFullPathMultipleExt + "\r\n";
			tbOutput.Text += "Drivename: " + utils.GetFragmentFromFileUrl(urlWithFullPathMultipleExt, Utilities.FileUrlFragmentPart.DriveName) + "\r\n";
			tbOutput.Text += "Path: " + utils.GetFragmentFromFileUrl(urlWithFullPathMultipleExt, Utilities.FileUrlFragmentPart.Path) + "\r\n";
			tbOutput.Text += "Filename: " + utils.GetFragmentFromFileUrl(urlWithFullPathMultipleExt, Utilities.FileUrlFragmentPart.FileName) + "\r\n";
			tbOutput.Text += "Filebody: " + utils.GetFragmentFromFileUrl(urlWithFullPathMultipleExt, Utilities.FileUrlFragmentPart.FileBody) + "\r\n";
			tbOutput.Text += "Fileext: " + utils.GetFragmentFromFileUrl(urlWithFullPathMultipleExt, Utilities.FileUrlFragmentPart.FileExt) + "\r\n";
		}

		private void btnXpathNav_Click(object sender, EventArgs e) {
			tbOutput.Text = "";

			// Setup example XML document
			string xmlDocText =
@"<?xml version=""1.0"" encoding=""utf-16""?>
<people>
	<person>
		<name>Michael Motor</name>
		<age>35</age>
		<hobbies>
			<hobby>Motorsport</hobby>
			<hobby>Formula One</hobby>
			<hobby>Watching Top Gear</hobby>
		</hobbies>
	</person>
	<person>
		<name>Charlie Computer</name>
		<age>23</age>
		<hobbies>
			<hobby>Video games</hobby>
			<hobby>Programming</hobby>
		</hobbies>
	</person>
	<person>
		<name>Betty Boring</name>
		<age>65</age>
		<hobbies />
	</person>
	<person>
		<name>Angie Active</name>
		<age>27</age>
		<hobbies>
			<hobby>Jogging</hobby>
			<hobby>Playing tennis</hobby>
			<hobby>Gym</hobby>
		</hobbies>
	</person>
	<person>
		<name>Freddie Film</name>
		<age>31</age>
		<hobbies>
			<hobby>Cinema</hobby>
		</hobbies>
	</person>
</people>";

			// First, create our XmlDocument
			XmlDocument xmlDoc = new XmlDocument();

			// Create the TextReader that will read the XML document from memory
			StringReader sr = new StringReader(xmlDocText);

			// Read and load it
			xmlDoc.Load(sr);

			// Create an XPathNavigator so we can easily query values from the XmlDocument
			XPathNavigator nav = xmlDoc.CreateNavigator();

			// Display sample doc
			tbOutput.Text += "Sample XML document:\r\n" + xmlDocText + "\r\n\r\n==========\r\n";

			// Demo of how to use XPathNavigator; first, query the people
			XPathNodeIterator peopleIter = nav.Select("/people/person");

			while (peopleIter.MoveNext()) {
				// This is a 'person' in our XML doc.
				tbOutput.Text += "\r\nPerson\r\n";

				// To do almost anything with this node, we want to get the iterator's underlying XPathNavigator.
				XPathNavigator nodeNav = peopleIter.Current;

				// Name.  Note that this is relative, so the XPath expression should NOT begin with a slash.
				tbOutput.Text += "- Name: " + nodeNav.SelectSingleNode("name").Value + "\r\n";
				// Age.
				tbOutput.Text += "- Age: " + nodeNav.SelectSingleNode("age").ValueAsInt.ToString() + "\r\n";
				// Hobbies.  We're iterating through these as it should have children.
				tbOutput.Text += "- Hobbies:";
				XPathNodeIterator hobbiesIter = nodeNav.Select("hobbies/hobby");
				bool hasHobbies = false;
				while (hobbiesIter.MoveNext()) {
					hasHobbies = true;
					tbOutput.Text += "\r\n  - " + hobbiesIter.Current.Value;
				}
				if (!hasHobbies) {
					tbOutput.Text += " (none)";
				}
				tbOutput.Text += "\r\n";

				// End of record, onto the next...
			}
		}

		private void btnTestHexStringConverter_Click(object sender, EventArgs e) {
			Utilities utils = new Utilities();

			string hexString1 = "C370200";
			string hexString2 = "C37020X0";
			string hexString3 = "C3702000";
			string errMsg;
			tbOutput.Text += "Testing converter with invalid hex string (length) - " + hexString1 + "... ";
			errMsg = null;
			try {
				utils.ConvertHexStringToBytes(hexString1);
			}
			catch (Exception ex) {
				errMsg = ex.Message;
			}
			if (errMsg == null) {
				tbOutput.Text += "OK, but should have failed!\r\n";
			}
			else {
				tbOutput.Text += "Failed; \"" + errMsg + "\".\r\n";
			}

			tbOutput.Text += "Testing converter with invalid hex string (contents) - " + hexString2 + "... ";
			errMsg = null;
			try {
				utils.ConvertHexStringToBytes(hexString2);
			}
			catch (Exception ex) {
				errMsg = ex.Message;
			}
			if (errMsg == null) {
				tbOutput.Text += "OK, but should have failed!\r\n";
			}
			else {
				tbOutput.Text += "Failed; \"" + errMsg + "\".\r\n";
			}

			tbOutput.Text += "Testing converter with valid hex string - " + hexString3 + "... ";
			errMsg = null;
			byte[] hexBytes = null;
			try {
				hexBytes = utils.ConvertHexStringToBytes(hexString3);
			}
			catch (Exception ex) {
				errMsg = ex.Message;
			}
			if (errMsg != null) {
				tbOutput.Text += "Failed, but should have succeeded; \"" + errMsg + "\".\r\n";
			}
			else {
				tbOutput.Text += "OK.  Converted to a byte array containing:\r\n";
				foreach (byte hexByte in hexBytes) {
					tbOutput.Text += string.Format("  {0:X2} ({0:d})\r\n", hexByte);
				}
			}
		}
	}
}
