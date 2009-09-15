using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Gooey;

namespace Carbon_Copy {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			// If a file path has been specified, assume it's the path to an XML file
			// containing settings for a backup profile, which we want to run in batch
			// mode.
			Gooey.Utilities utils = new Gooey.Utilities();
			string[] cmdLineArgs = Environment.GetCommandLineArgs();
			if (cmdLineArgs.Length > 2) {
				utils.ShowError("Usage: CarbCopy.exe \"C:\\path\\to\\backup\\profile.xml\"");
				Application.Exit();
			}
			else if (cmdLineArgs.Length == 2) {
				// Run in batch mode - try to load options, then run straight from
				// backup form
				
				// Load profile settings from selected XML file
				CCOSaveLoad saveloader = new CCOSaveLoad();
				CCO options;
				string errorTxt;
				string loadFromPath = cmdLineArgs[1];
				if (!saveloader.Load(out options, loadFromPath, out errorTxt)) {
					utils.ShowError("Error loading settings from '" + loadFromPath + "': " + errorTxt);
					Application.Exit();
				}
				else {
					Application.Run(new frmBackup(options));
				}
			}
			else {
				// Run in interactive mode
				Application.Run(new frmMain());
			}
		}
	}
}
