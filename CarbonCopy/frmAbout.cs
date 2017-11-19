using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gooey;
using System.Reflection;
using System.IO;

namespace CarbonCopy {
	public partial class frmAbout : Form {
		#region Private members

		private Assembly _thisAssembly;
		private Utilities _utils;

		#endregion

		#region Constructors

		public frmAbout() {
			InitializeComponent();

			_utils = new Utilities();
		}

		#endregion

		#region Private helper methods

		private string getLicenceText() {
			StreamReader sr = new StreamReader(_utils.GetEmbeddedResource(_thisAssembly, "CarbonCopy", "Resources.licence.txt"), Encoding.UTF8);
			return sr.ReadToEnd();
		}

		#endregion

		private void frmAbout_Load(object sender, EventArgs e) {
			_thisAssembly = Assembly.GetExecutingAssembly();

			lblVer.Text = lblVer.Text.Replace("$ver", _utils.GetVersionString(_thisAssembly, VersionStringType.MajorMinor));

			// Prevent tabstop for link to website (property is unavailable in Visual Studio designer!)
			lnkGooeySite.TabStop = false;

			// Load licence text from embedded resource
			txtGplLicence.Text = this.getLicenceText();
		}

		private void lnkGooeySite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs ea) {
			// Navigate to Gooey Software website
			System.Diagnostics.Process.Start("https://www.gooeysoftware.com/");
		}

		private void btnExit_Click(object sender, EventArgs e) {
			this.Close();
		}
	}
}
