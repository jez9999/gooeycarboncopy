using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Gooey;

namespace CarbonCopy {
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
			bkpEngine.CbDebugMsg += addDebugMsg;
			bkpEngine.CbInfoMsg += addInfoMsg;
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
		
		/// <summary>
		/// Add informational message
		/// </summary>
		/// <param name="msg">Message text</param>
		private void addInfoMsg(string msg) {
			bool displayThis = true;
			switch (backupOptions.OutputDetail) {
				case VerbosityLevel.Brief:
				case VerbosityLevel.Normal:
				case VerbosityLevel.Verbose:
				case VerbosityLevel.UltraVerbose:
				default:
					displayThis = true;
					break;
			}
			if (displayThis) {
				addTxtboxMsg("Inf: " + msg, ccColour.Black, true);
			}
		}
		
		/// <summary>
		/// Add error message
		/// </summary>
		/// <param name="msg">Message text</param>
		private void addErrorMsg(string msg) {
			bool displayThis = true;
			switch (backupOptions.OutputDetail) {
				case VerbosityLevel.Brief:
					displayThis = false;
					break;
				
				case VerbosityLevel.Normal:
				case VerbosityLevel.Verbose:
				case VerbosityLevel.UltraVerbose:
				default:
					displayThis = true;
					break;
			}
			if (displayThis) {
				addTxtboxMsg("Err: " + msg, ccColour.Red, true);
			}
		}
		
		/// <summary>
		/// Add debug message
		/// </summary>
		/// <param name="msg">Message text</param>
		private void addDebugMsg(string msg) {
			bool displayThis = true;
			switch (backupOptions.OutputDetail) {
				case VerbosityLevel.Brief:
				case VerbosityLevel.Normal:
					displayThis = false;
					break;
				
				case VerbosityLevel.Verbose:
				case VerbosityLevel.UltraVerbose:
				default:
					displayThis = true;
					break;
			}
			if (displayThis) {
				addTxtboxMsg("Dbg: " + msg, ccColour.Green, true);
			}
		}
		
		/// <summary>
		/// Add verbose message
		/// </summary>
		/// <param name="msg">Message text</param>
		private void addVerboseMsg(string msg) {
			bool displayThis = true;
			switch (backupOptions.OutputDetail) {
				case VerbosityLevel.Brief:
				case VerbosityLevel.Normal:
				case VerbosityLevel.Verbose:
					displayThis = false;
					break;
				
				case VerbosityLevel.UltraVerbose:
				default:
					displayThis = true;
					break;
			}
			if (displayThis) {
				addTxtboxMsg("Ver: " + msg, ccColour.Blue, true);
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