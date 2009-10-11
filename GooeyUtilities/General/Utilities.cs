using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace Gooey {
	/// <summary>
	/// Because Microsoft bizarrely elected not to allow you to disable the close button from their Form class, we have to implement the ability to do it here by using the Win32 API.
	/// </summary>
	public class CloseButtonDisabler {
		#region Private vars
		
		private bool initialized = false;
		private Form frmDisableMyButton = null;
		private bool? btnDisabled = null;
		
		#endregion
		
		#region Private members
		
		private void doEnableDisable(bool disable) {
			int setting;
			
			if (disable) {
				// Disabling
				setting = MF_BYCOMMAND | MF_GREYED;
				this.btnDisabled = true;
			}
			else {
				// Enabling
				setting = MF_BYCOMMAND | MF_ENABLED;
				this.btnDisabled = false;
			}
			
			// Update close box, and internal flag state
			EnableMenuItem(GetSystemMenu(this.frmDisableMyButton.Handle, 0), SC_CLOSE, setting);
		}
		
		#endregion
		
		#region Public members
		
		/// <summary>
		/// This must be called before any other functionality of the class is used, so it knows which form it's dealing with.
		/// </summary>
		/// <param name="frmDisableMyButton"></param>
		public void InitValues(Form frmDisableMyButton) {
			this.frmDisableMyButton = frmDisableMyButton;
			
			initialized = true;
		}
		
		/// <summary>
		/// When a Form is resized, its close button's status will be reset.  You should register a .SizeChanged event handler and make it call this method, in order to maintain the state of the close button whether enabled OR disabled).
		/// </summary>
		public void EventSizeChanged() {
			doEnableDisable((bool)this.btnDisabled);
		}
		
		/// <summary>
		/// Set this property to true to disable the close button, or false to enable it.  The property value is a boolean indicating whether the close button is currently disabled or not.
		/// </summary>
		public bool? ButtonDisabled {
			get {
				if (!initialized) { throw new Exception("InitValues must be called before other functionality of this class may be used."); }
				
				return this.btnDisabled;
			}
			set {
				if (!initialized) { throw new Exception("InitValues must be called before other functionality of this class may be used."); }
				
				if (value != null) { doEnableDisable((bool)value); }
				else { this.btnDisabled = null; }
			}
		}
		
		#endregion
		
		#region Win32 API imports and constants
		
		[DllImport("User32.dll", CharSet=CharSet.Auto, EntryPoint="GetSystemMenu")]
		private static extern IntPtr GetSystemMenu(IntPtr hWnd, int revert);
		
		[DllImport("User32.dll", CharSet=CharSet.Auto, EntryPoint="EnableMenuItem")]
		private static extern int EnableMenuItem(IntPtr hWndMenu, int itemID, int enable);
		
		private const int SC_CLOSE = 0xF060;
		private const int MF_ENABLED = 0;
		private const int MF_GREYED = 1;
		private const int MF_BYCOMMAND = 0;
		
		#endregion
	}
	
	/// <summary>
	/// Used to specify the type of version string that should be generated.
	/// </summary>
	public enum VersionStringType {
		/// <summary>
		/// A full Assembly version string, including major, minor, build, and revision.
		/// </summary>
		FullString
	}
	
	/// <summary>
	/// Gooey Software's general utility functions class
	/// </summary>
	public class Utilities {
		#region Public methods
		
		/// <summary>
		/// Returns a string indicating the version of the Assenbly supplied.
		/// </summary>
		/// <param name="getVersionFor">The Assembly to get the version string for.</param>
		/// <param name="versionStrType">The format of the version string to return.</param>
		/// <returns>The version string for the Assembly supplied.</returns>
		public static string GetVersionString(Assembly getVersionFor, VersionStringType versionStrType) {
			string retVal;
			Version ver = getVersionFor.GetName().Version;
			
			switch (versionStrType) {
				case VersionStringType.FullString:
				retVal = ver.Major.ToString() + "." + ver.Minor.ToString() + "." + ver.Build.ToString() + "." + ver.Revision.ToString();
				break;
				
				default:
				retVal = ("(no version type recognized!)");
				break;
			}
			
			return retVal;
		}
		
		/// <summary>
		/// Captures and returns the 'drive name' part of a codebase string.
		/// </summary>
		/// <param name="codeBase">The codebase string, eg. file://C:\path\to\codebase.exe</param>
		/// <returns>The drive part, eg. 'C', or an empty string if no drive part was found.</returns>
		public string DriveNameFromCodebase(string codeBase) {
			Regex reParseFileURI = new Regex(@"\/([^\/]*?)\:");
			Match matchDriveName = reParseFileURI.Match(codeBase);
			if (matchDriveName.Groups.Count >= 2) { return matchDriveName.Groups[1].Value; }
			else { return ""; }
		}
		
		/// <summary>
		/// Shows an error message dialog with generic error title, OK button, and error message icon.
		/// </summary>
		/// <param name="errorMsg">The string of the error message to display.</param>
		public void ShowError(string errorMsg) {
			System.Windows.Forms.MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
		
		/// <summary>
		/// Shows a warning message dialog with generic error title, OK button, and error message icon.
		/// </summary>
		/// <param name="warningMsg">The string of the warning message to display.</param>
		public void ShowWarning(string warningMsg) {
			System.Windows.Forms.MessageBox.Show(warningMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}
		
		/// <summary>
		/// Shows an information message dialog with generic information title, OK button, and information icon.
		/// </summary>
		/// <param name="infoMsg">The string of the information message to display.</param>
		public void ShowInfo(string infoMsg) {
			System.Windows.Forms.MessageBox.Show(infoMsg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		
		/// <summary>
		/// Shows an OK/Cancel dialog with specified title, message, OK/Cancel buttons, and Question icon.
		/// </summary>
		/// <param name="title">The title/caption for the dialog.</param>
		/// <param name="okCancelMsg">The string of the OK/cancel message to display.</param>
		/// <returns>The result of the user's interaction with the dialog.</returns>
		public DialogResult ShowOkCancel(string title, string okCancelMsg) {
			return System.Windows.Forms.MessageBox.Show(okCancelMsg, title, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
		}
		
		/// <summary>
		/// Shows an Yes/No dialog with specified title, message, Yes/No buttons, and Question icon.
		/// </summary>
		/// <param name="title">The title/caption for the dialog.</param>
		/// <param name="yesNoMsg">The string of the Yes/No message to display.</param>
		/// <returns>The result of the user's interaction with the dialog.</returns>
		public DialogResult ShowYesNo(string title, string yesNoMsg) {
			return System.Windows.Forms.MessageBox.Show(yesNoMsg, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
		}
		
		/// <summary>
		/// Shows a Yes/No/Cancel dialog with specified title, message, Yes/No/Cancel buttons, and Question icon.
		/// </summary>
		/// <param name="title">The title/caption for the dialog.</param>
		/// <param name="yesNoCancelMsg">The string of the Yes/No/Cancel message to display.</param>
		/// <returns>The result of the user's interaction with the dialog.</returns>
		public DialogResult ShowYesNoCancel(string title, string yesNoCancelMsg) {
			return System.Windows.Forms.MessageBox.Show(yesNoCancelMsg, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
		}
		
		/// <summary>
		/// Scrolls the textbox to the end/bottom.  USES THE WIN32 API.
		/// </summary>
		/// <param name="rtb">The RichTextBox object to scroll to the end/bottom.</param>
		public void ScrollTextBoxEnd(RichTextBox rtb) {
			const int WM_VSCROLL = 277;
			const int SB_BOTTOM = 7;
			
			IntPtr ptrWparam = new IntPtr(SB_BOTTOM);
			IntPtr ptrLparam = new IntPtr(0);
			SendMessage(rtb.Handle, WM_VSCROLL, ptrWparam, ptrLparam);
		}
		
		[DllImport("User32.dll", CharSet=CharSet.Auto, EntryPoint="SendMessage")]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
		
		#endregion
	}
	
	/// <summary>
	/// Handles the calling of the Form.Invoke() method in a safe way; that is to say that it will, by default, drop exceptions; it will also return immediately when Invoke is called and setup a new Thread to deal with it.  This may be needed if Invoke is called at a time when the Form may be closing or have closed, so an ObjectDisposedException or InvalidOperationException (which we can safely ignore) may arise.
	/// </summary>
	public class SafeInvoker {
		#region Private vars
		
		Form invokeWith;
		private bool dropExceptions;
		
		private class doInvocationData {
			public Delegate method;
			public object[] args;
		}
		
		#endregion
		
		#region Constructors
		
		public SafeInvoker(Form invokeWith, bool dontdropExceptions) {
			this.invokeWith = invokeWith;
			this.dropExceptions = !dontdropExceptions;
		}
		
		public SafeInvoker(Form invokeWith): this(invokeWith, false) {
			// We want to drop the exception raised if Invoke fails, by default
		}
		
		#endregion
		
		#region Private methods
		
//		private void doInvocation(Object threadData) {
//			// We need threadData to be a doInvocationData class as that contains the
//			// necessary arguments.
//			doInvocationData did = (doInvocationData)threadData;
//			
//			// Try the invocation, maybe drop exceptions
//			try {
//				invokeWith.Invoke(did.method, did.args);
//			}
//			catch (Exception ex) {
//				if (!dropExceptions) {
//					throw ex;
//				}
//			}
//		}
		
		#endregion
		
		#region Public methods
		
		public void Invoke(Delegate method) {
			this.Invoke(method, new object[] { });
		}
		
		public void Invoke(Delegate method, params object[] args) {
			invokeWith.Invoke(method, args);
//			// Invoke the specified method with the specified args in a new thread
//			doInvocationData did = new doInvocationData();
//			did.method = method;
//			did.args = args;
//			
//			Thread invoker = new Thread(new ParameterizedThreadStart(doInvocation));
//			invoker.Start(did);
		}
		
		#endregion
	}
}
