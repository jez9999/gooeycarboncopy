using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Gooey;

namespace Carbon_Copy {
	public partial class frmBackup : Form {
		#region Private vars
		
		private Gooey.Utilities utils;
		private CCOFunctions optFunc;
		private CCOColours ccColour;
		private BackupEngine bkpEngine;
		private CCO backupOptions;
		private int widthDiff = 0;
		private int heightDiff = 0;
		private int cancelLeftDiff = 0;
		private int cancelTopDiff = 0;
		private int closeLeftDiff = 0;
		private int closeTopDiff = 0;
		private bool closeOnStop = false;
		private bool closeBtnClicked = false;
		
		#endregion
		
		#region Constructors
		
		public frmBackup(CCO passedOptions) {
			InitializeComponent();
			
			// Carbon Copy backup options must be passed to this form upon initialization
			this.backupOptions = passedOptions;
			
			this.utils = new Gooey.Utilities();
			this.optFunc = new CCOFunctions();
			this.ccColour = new CCOColours();
			
			this.bkpEngine = new BackupEngine(this.backupOptions);
			bkpEngine.CbBackupFinished += backupFinishedInvoker;
			bkpEngine.CbMsg += addMsg;
			bkpEngine.CbCommentMsg += addCommentMsg;
			bkpEngine.CbErrorMsg += addErrorMsg;
			bkpEngine.CbVerboseMsg += addVerboseMsg;
			bkpEngine.CbDisplayNextMessage += displayNextMsgInvoker;
		}
		
		#endregion
		
		#region Private methods
		
		private static Gooey.CloseButtonDisabler cbdFrmBackup = new CloseButtonDisabler();
		
		private static void handleSizeChanged(object sender, EventArgs e) {
			cbdFrmBackup.EventSizeChanged();
		}
		
		private void frmBackup_Load(object sender, EventArgs e) {
			// Set title to version number, etc.
			this.Text = "Carbon Copy v" + Gooey.Utilities.GetVersionString(System.Reflection.Assembly.GetExecutingAssembly(), VersionStringType.FullString) + " - Backing up...";
			
			// Disable form's close button, and make sure it stays disabled when
			// form is resized.
			cbdFrmBackup.InitValues(this);
			this.SizeChanged += new EventHandler(handleSizeChanged);
			cbdFrmBackup.ButtonDisabled = true;
			
			// Move form into position
			this.Left = Screen.GetWorkingArea(this).Width / 20;
			this.Top = Screen.GetWorkingArea(this).Height / 20;
			
			// Record positions of controls on form that we'll need later
			widthDiff = this.Width - txtBackupOutput.Width;
			heightDiff = this.Height - txtBackupOutput.Height;
			cancelLeftDiff = this.Width - btnCancel.Left;
			cancelTopDiff = this.Height - btnCancel.Top;
			closeLeftDiff = this.Width - btnClose.Left;
			closeTopDiff = this.Height - btnClose.Top;
			
			// Position form controls correctly
			positionFormControls(this, null);
			this.SizeChanged += new EventHandler(positionFormControls);
			
			// TODO: yes, this will cause the window BG to go grey.  Find a 3rd
			// party control (or create one!) that lets us set it to a colour of our
			// choice when it's disabled.  For now, we put up with grey.
			txtBackupOutput.Enabled = false;
			
			// Prevent text wrapping
			// TODO: This doesn't work.  Maybe replacing this richtextbox with our new
			// listbox control will fix things anyway, but in the meantime maybe we can
			// find a way to prevent text wrapping in the richtextbox?
			txtBackupOutput.RightMargin = System.Convert.ToInt32(System.Math.Pow(2, 31)-1);
			
			startBackup();
		}
		
		private void positionFormControls(object sender, EventArgs e) {
			txtBackupOutput.Width = this.Width - this.widthDiff;
			txtBackupOutput.Height = this.Height - this.heightDiff;
			btnCancel.Left = this.Width - this.cancelLeftDiff;
			btnCancel.Top = this.Height - this.cancelTopDiff;
			btnClose.Left = this.Width - this.closeLeftDiff;
			btnClose.Top = this.Height - this.closeTopDiff;
		}
		
		// Methods to add text to backup richtextbox
		private void displayNextMsg(GetNextMessageCallback getNextMsgCb) {
			MsgDisplayInfo di = getNextMsgCb();
			di.MsgFunction(di.MsgText);
		}
		private void displayNextMsgInvoker(GetNextMessageCallback getNextMsgCb) {
			DisplayNextMsgCallback cb = new DisplayNextMsgCallback(displayNextMsg);
			SafeInvoker si = new SafeInvoker(this);
			si.Invoke(cb, new object[] { getNextMsgCb });
		}
		
		private void addMsg(string msg) {
			// TODO - when we get a textbox class that doesn't suck and allows us to set
			// foreground and background colour even after we've disabled the textbox.
			// we don't need to specify ccColour.Black here, and can go back to the old
			// code, specifying msg,null,false.
			// addTxtboxMsg(msg, new Color(), false);  // <-- probably dont want this
			// addTxtboxMsg(msg, null, false);         // <-- probably want this
			// ^ actually, we should be able to use null for the middle argument, the
			// one above that is what we used to use but new Color() probably has a nasty
			// overhead.
			addTxtboxMsg(msg, ccColour.Black, true);
		}
		
		private void addCommentMsg(string msg) {
			if ((backupOptions.ToDisplay & CCOWhatToDisplay.Comments) != 0) {
				addTxtboxMsg(msg, ccColour.Green, true);
			}
		}
		
		private void addErrorMsg(string msg) {
			if ((backupOptions.ToDisplay & CCOWhatToDisplay.Errors) != 0) {
				addTxtboxMsg("Error: " + msg, ccColour.Red, true);
			}
		}
		
		private void addVerboseMsg(string msg) {
			if ((backupOptions.ToDisplay & CCOWhatToDisplay.Verbose) != 0) {
				addTxtboxMsg(msg, ccColour.Blue, true);
			}
		}
		
		private void addTxtboxMsg(string msg, Color ccColor, bool useCcColor) {
			// TODO: When we get a new, better output control than this, that should take
			// care of the flicker/wobbling scrollbar problem.
			
			// Try and make this an atomic operation to reduce (somewhat) scrollbar
			// flicker/'wobbling'
			lock(txtBackupOutput) {
				// Get caret to end of text
				txtBackupOutput.Select(txtBackupOutput.Text.Length, 0);
				
				// Append message text
				Color backupColor = txtBackupOutput.SelectionColor;
				if (useCcColor) { txtBackupOutput.SelectionColor = ccColor; }
				txtBackupOutput.SelectedText = msg + "\r\n";
				if (useCcColor) { txtBackupOutput.SelectionColor = backupColor; }
				txtBackupOutput.ScrollToCaret();
				txtBackupOutput.Update();
			}
		}
		
		private void btnCancel_Click(object sender, EventArgs e) {
			bkpEngine.StopBackup();
		}
		
		private void btnClose_Click(object sender, EventArgs e) {
			closeBtnClicked = true;
			this.Close();
		}
		
		private void startBackup() {
			string errors;
			if (!optFunc.SanityCheck(this.backupOptions, out errors)) {
				addErrorMsg("Error(s) found in backup profile:\r\n" + errors);
				backupFinished();
				return;
			}
			
			// We assume all options have now been checked for validity; start the
			// backup process IF the backup engine exists and is not currently running
			// a backup.
			
			if (!bkpEngine.IsRunningBackup) {
				btnCancel.Enabled = true;
				bkpEngine.StartBackup();
			}
		}
		
		private void backupFinished() {
			txtBackupOutput.Enabled = true;
			btnCancel.Enabled = false;
			btnClose.Enabled = true;
			
			txtBackupOutput.TabStop = true;
			btnCancel.TabStop = true;
			btnClose.TabStop = true;
			
			if (closeOnStop) {
				this.Close();
			}
		}
		private void backupFinishedInvoker() {
			BackupFinishedCallback cb = new BackupFinishedCallback(backupFinished);
			SafeInvoker si = new SafeInvoker(this);
			si.Invoke(cb, new object[] {  });
		}
		
		private void frmBackup_FormClosing(object sender, FormClosingEventArgs e) {
			// We're not closing this form except via our close button...
			if (!closeBtnClicked) {
				e.Cancel = true;
				return;
			}
			
			// Stop backup if it's in progress
			if (bkpEngine.IsRunningBackup) {
				bkpEngine.StopBackup();
				e.Cancel = true;
				closeOnStop = true;
				return;
			}
		}
		
		#endregion
	}
}