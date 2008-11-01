using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Carbon_Copy {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			// TODO: Need to implement commandline arguments here to allow for batch mode
			Application.Run(new frmMain());
		}
	}
}