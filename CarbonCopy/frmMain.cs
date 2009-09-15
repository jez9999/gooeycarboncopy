using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using Jez;

namespace Carbon_Copy {
	public partial class frmMain : Form {
		#region Private vars
		
		private Jez.Utilities utils;
		private CCOFunctions optFunc;
		
		#endregion
		
		#region Constructors
		
		public frmMain() {
			InitializeComponent();
			
			// TODO: Make the quickToolTip popup and disappear much faster via the
			// designer property settings, once we've figured out how to fix its
			// bug (see http://social.msdn.microsoft.com/Forums/en-US/netfxbcl/thread/a08a23c4-88b5-464d-903b-87555475cb88)
			
			// Setup tooltip(s)
			ttpQuick.SetToolTip(lblVerboseWarning, "'Verbose' can result in a very large amount of output for large backups; don't enable it unless you really want very detailed information!");
			
			this.utils = new Jez.Utilities();
			this.optFunc = new CCOFunctions();
		}
		
		#endregion
		
		#region Private methods
		
		private void btnSourceBrowse_Click(object sender, EventArgs e) {
			folderBrowserDialog1.Description = "Select a source directory to backup...";
			string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
			string driveName = utils.DriveNameFromCodebase(codeBase);
			folderBrowserDialog1.SelectedPath = driveName + ":\\";
			folderBrowserDialog1.ShowNewFolderButton = false;
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
				txtSourceDir.Text = folderBrowserDialog1.SelectedPath;
			}
		}
		
		private void frmMain_Load(object sender, EventArgs e) {
			// Set title to version number, etc.
			this.Text = "Carbon Copy v" + Jez.Utilities.GetVersionString(System.Reflection.Assembly.GetExecutingAssembly(), VersionStringType.FullString);
			
			// Automatic label height, but force width to that of the backup dirs list
			lblDestDir.MinimumSize = new Size(lstSourceDirs.Width, 0);
			lblDestDir.MaximumSize = new Size(lstSourceDirs.Width, 0);
			lblDestDir.Text = "";
			
			// Initialize options information/default selections
			radCarbon.Checked = true;
			chkComments.Checked = true;
		}
		
		private void btnAddDir_Click(object sender, EventArgs e) {
			DirectoryInfo fixedPath = null;
			string errorHolder;
			
			// Is path string valid?
			if (!optFunc.CheckDirValidity(txtSourceDir.Text, ref fixedPath, out errorHolder)) {
				utils.ShowError(errorHolder);
				return;
			}
			
			// Is it a dupe?
			foreach (CCODirinfoHolder dirPath1 in lstSourceDirs.Items) {
				DirectoryInfo dirPath2 = fixedPath;
				if (dirPath1.ToString().Equals(dirPath2.FullName, StringComparison.CurrentCultureIgnoreCase)) {
					utils.ShowError("Directory '" + dirPath2.FullName + "' is already in the list.");
					return;
				}
			}
			
			lstSourceDirs.Items.Add(new CCODirinfoHolder(fixedPath));
			txtSourceDir.Text = "";
		}
		
		private void btnDestSet_Click(object sender, EventArgs e) {
			DirectoryInfo fixedPath = null;
			string errorHolder;
			
			// Is path string valid?
			if (!optFunc.CheckDirValidity(txtDestDir.Text, ref fixedPath, out errorHolder)) {
				utils.ShowError(errorHolder);
				return;
			}
			
			lblDestDir.Text = fixedPath.FullName;
			lblDestDir.Tag = fixedPath;
			txtDestDir.Text = "";
		}
		
		private void btnRemDir_Click(object sender, EventArgs e) {
			if (lstSourceDirs.SelectedIndex >= 0) {
				int oldIndex = lstSourceDirs.SelectedIndex;
				lstSourceDirs.Items.RemoveAt(lstSourceDirs.SelectedIndex);
				if (lstSourceDirs.Items.Count-1 >= oldIndex) {
					lstSourceDirs.SelectedIndex = oldIndex;
				}
				else {
					if (lstSourceDirs.Items.Count > 0) {
						lstSourceDirs.SelectedIndex = lstSourceDirs.Items.Count-1;
					}
				}
			}
			else {
				System.Media.SystemSounds.Beep.Play();
			}
		}
		
		private void btnDestBrowse_Click(object sender, EventArgs e) {
			folderBrowserDialog1.Description = "Select a destination directory to backup to...";
			string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
			string driveName = utils.DriveNameFromCodebase(codeBase);
			if (Directory.Exists(txtDestDir.Text)) { folderBrowserDialog1.SelectedPath = txtDestDir.Text; }
			else { folderBrowserDialog1.SelectedPath = driveName + ":\\"; }
			folderBrowserDialog1.ShowNewFolderButton = false;
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
				txtDestDir.Text = folderBrowserDialog1.SelectedPath;
			}
		}
		
		private void grpDest_Paint(object sender, PaintEventArgs e) {
			// Paint bevel around 'destination directory' text
			System.Drawing.Pen greyPen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0x80, 0x80, 0x80));
			System.Drawing.Pen whitePen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0xFF, 0xFF, 0xFF));
			System.Drawing.Graphics grpDestGraphics = grpDest.CreateGraphics();
			Point ptTopLeft = new Point(lblDestDir.Left-1, lblDestDir.Top-1);
			Point ptTopRight = new Point(lblDestDir.Left+lblDestDir.Width, lblDestDir.Top-1);
			Point ptBottomLeft = new Point(lblDestDir.Left-1, lblDestDir.Top+lblDestDir.Height);
			Point ptBottomRight = new Point(lblDestDir.Left+lblDestDir.Width, lblDestDir.Top+lblDestDir.Height);
			
			grpDestGraphics.DrawLine(greyPen, ptTopLeft, ptTopRight);
			grpDestGraphics.DrawLine(greyPen, ptTopLeft, ptBottomLeft);
			grpDestGraphics.DrawLine(whitePen, ptBottomLeft, ptBottomRight);
			grpDestGraphics.DrawLine(whitePen, ptTopRight, ptBottomRight);
			
			grpDestGraphics.Dispose();
			greyPen.Dispose();
			whitePen.Dispose();
		}
		
		private void btnStart_Click(object sender, EventArgs e) {
			CCO passingOptions = genPassingOptions();
			
			string errors;
			if (!optFunc.SanityCheck(passingOptions, out errors)) {
				utils.ShowError("Error(s) found in backup profile:\r\n" + errors);
			}
			else {
				frmBackup backupForm = new frmBackup(passingOptions);
				
				backupForm.ShowDialog();
			}
		}
		
		private CCO genPassingOptions() {
			CCO options = new CCO();
			
			foreach (CCODirinfoHolder dirInfoHolder in lstSourceDirs.Items) {
				options.SourceDirs.Add(dirInfoHolder.DirInfo);
			}
			
			options.DestDir = (DirectoryInfo)lblDestDir.Tag;
			
			if (radCarbon.Checked) { options.Type = CCOTypeOfBackup.CarbonCopy; }
			else if (radIncremental.Checked) { options.Type = CCOTypeOfBackup.Incremental; }
			
			options.ToDisplay =
				(chkComments.Checked ? CCOWhatToDisplay.Comments : 0) |
				(CCOWhatToDisplay.Errors) |   // Errors always displayed
				(chkVerbose.Checked ? CCOWhatToDisplay.Verbose : 0);
			
			return options;
		}
		
		private void setupFromOptions(CCO options) {
			// This method assumes 'options' has already been sanity checked (dupes, etc.)
			// Setup source dir(s)
			lstSourceDirs.Items.Clear();
			foreach (DirectoryInfo di in options.SourceDirs) {
				lstSourceDirs.Items.Add(new CCODirinfoHolder(di));
			}
			
			// Setup dest dir
			lblDestDir.Text = "";
			lblDestDir.Tag = null;
			lblDestDir.Text = options.DestDir.FullName;
			lblDestDir.Tag = options.DestDir;
			
			// Setup type of backup
			if (options.Type == CCOTypeOfBackup.CarbonCopy) {
				radCarbon.Checked = true;
			}
			else if (options.Type == CCOTypeOfBackup.Incremental) {
				radIncremental.Checked = true;
			}
			
			// Setup what to display
			if ((options.ToDisplay & CCOWhatToDisplay.Comments) > 0) {
				chkComments.Checked = true;
			}
			else {
				chkComments.Checked = false;
			}
			
			if ((options.ToDisplay & CCOWhatToDisplay.Verbose) > 0) {
				chkVerbose.Checked = true;
			}
			else {
				chkVerbose.Checked = false;
			}
			// (Currently, we always display errors.  We may make this configurable in
			// future.)
		}
		
		private void txtSourceDir_KeyPress(object sender, KeyPressEventArgs e) {
			if ((int)e.KeyChar == 13) {
				// Default action: Add source directory
				btnAddDir_Click(sender, e);
				e.Handled = true;
			}
		}
		
		private void txtDestDir_KeyPress(object sender, KeyPressEventArgs e) {
			if ((int)e.KeyChar == 13) {
				// Default action: Set destination directory
				btnDestSet_Click(sender, e);
				e.Handled = true;
			}
		}
		
		private void btnAbout_Click(object sender, EventArgs e) {
			utils.ShowInfo("Carbon copy version: " + Jez.Utilities.GetVersionString(System.Reflection.Assembly.GetExecutingAssembly(), VersionStringType.FullString));
		}
		
		private void lstSourceDirs_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Delete) {
				btnRemDir.PerformClick();
			}
		}
		
		private void frmMain_Paint(object sender, PaintEventArgs e) {
			// Draw separators
			System.Drawing.Pen blackPen = new System.Drawing.Pen(System.Drawing.Color.FromArgb(0x00, 0x00, 0x00));
			System.Drawing.Graphics formGraphics = this.CreateGraphics();
			Point ptTop1 = new Point(btnStart.Right+8, btnStart.Top+1);
			Point ptBtm1 = new Point(btnStart.Right+8, btnStart.Bottom-1);
			Point ptTop2 = new Point(btnLoad.Right+8, btnLoad.Top+1);
			Point ptBtm2 = new Point(btnLoad.Right+8, btnLoad.Bottom-1);
			
			formGraphics.DrawLine(blackPen, ptTop1, ptBtm1);
			formGraphics.DrawLine(blackPen, ptTop2, ptBtm2);
			
			formGraphics.Dispose();
			blackPen.Dispose();
		}
		
		private void btnExit_Click(object sender, EventArgs e) {
			Application.Exit();
		}
		
		private void btnSave_Click(object sender, EventArgs e) {
			// Save current profile settings to XML file
			CCOSaveLoad saveloader = new CCOSaveLoad();
			
			// Dialog for selecting file path to save to
			saveFileDialog1.Title = "Select a file to save backup profile to...";
			saveFileDialog1.FileName = "";
			// We purposely don't set the .InitialDirectory property.  When left unset,
			// the dialog helpfully remembers the last directory the user was in, which
			// is desired behaviour.
			saveFileDialog1.Filter = "XML file (.xml)|*.xml";
			saveFileDialog1.FilterIndex = 1;
			if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
				string errorTxt;
				string saveToPath = saveFileDialog1.FileName;
				if (!saveloader.Save(genPassingOptions(), saveToPath, out errorTxt)) {
					utils.ShowError("Error saving settings to '" + saveToPath + "': " + errorTxt);
				}
				else {
					utils.ShowInfo("Settings saved to '" + saveToPath + "'.");
				}
			}
		}
		
		private void btnLoad_Click(object sender, EventArgs e) {
			// Load profile settings from selected XML file
			CCOSaveLoad saveloader = new CCOSaveLoad();
			
			// Dialog for selecting file path to load from
			openFileDialog1.Title = "Select a file to load backup profile from...";
			openFileDialog1.FileName = "";
			// We purposely don't set the .InitialDirectory property.  When left unset,
			// the dialog helpfully remembers the last directory the user was in, which
			// is desired behaviour.
			openFileDialog1.Filter = "XML files (.xml)|*.xml|All files (*.*)|*.*";
			openFileDialog1.FilterIndex = 1;
			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				CCO options;
				string errorTxt;
				string loadFromPath = openFileDialog1.FileName;
				if (!saveloader.Load(out options, loadFromPath, out errorTxt)) {
					utils.ShowError("Error loading settings from '" + loadFromPath + "': " + errorTxt);
				}
				else {
					setupFromOptions(options);
					utils.ShowInfo("Settings loaded from '" + loadFromPath + "'.");
				}
			}
		}
		
		#endregion
	}
}
