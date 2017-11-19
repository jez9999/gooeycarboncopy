using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;

namespace CarbonCopy {
	/// <summary>
	/// Class to hold a DirectoryInfo object, but return the FullName of that DirectoryInfo's path when .ToString()'d (DirectoryInfo itself only returns Name).
	/// </summary>
	public class CCODirinfoHolder {
		#region Constructors

		public CCODirinfoHolder(DirectoryInfo dirInfo) {
			this.DirInfo = dirInfo;
		}

		#endregion

		#region Public vars

		public DirectoryInfo DirInfo;

		#endregion

		#region Public methods

		public override string ToString() {
			if (DirInfo == null) { return ""; }
			else { return DirInfo.FullName; }
		}

		#endregion
	}

	/// <summary>
	/// Structure containing definitions for Carbon Copy's colours.
	/// </summary>
	public class CCOColours {
		public Color Black = System.Drawing.Color.FromArgb(0, 0, 0);
		public Color Green = System.Drawing.Color.FromArgb(0, 120, 0);
		public Color Red = System.Drawing.Color.FromArgb(255, 0, 0);
		public Color Blue = System.Drawing.Color.FromArgb(0, 0, 255);
	}

	/// <summary>
	/// Contains members to hold the various options relating to a Carbon Copy backup.
	/// </summary>
	public class CCO {
		#region Public vars
		public List<DirectoryInfo> SourceDirs = new List<DirectoryInfo>();
		public DirectoryInfo DestDir;
		public CCOTypeOfBackup Type = CCOTypeOfBackup.None;
		public VerbosityLevel OutputDetail = VerbosityLevel.Normal;
		public bool IsDryRun;
		#endregion
	}

	/// <summary>
	/// Class that deals with various functions pertaining to Carbon Copy options, including saving to and loading from permenant storage.
	/// </summary>
	public class CCOFunctions {
		#region Public methods
		public bool CheckDirValidity(string inputPath, ref DirectoryInfo outputPath, out string errorHolder) {
			string fixedPath = "";

			// Is path string valid?
			try {
				fixedPath = Path.GetFullPath(inputPath);
			}
			catch (Exception) {
				string errorPath;
				if (fixedPath != "") { errorPath = fixedPath; }
				else { errorPath = inputPath; }
				errorHolder = "Directory path '" + errorPath + "' is invalid.";
				return false;
			}

			// Does specified path exist?
			if (!System.IO.Directory.Exists(fixedPath)) {
				errorHolder = "Directory '" + fixedPath + "' does not exist.";
				return false;
			}

			// Now get the correct capitalization for the specified dir path, and
			// use it to create a DirectoryInfo for the specified directory.  This means
			// outputPath will be set to the directory specified by the inputPath,
			// and will have the correct capitalization.  Any drive letters at the
			// beginning of standard Windows paths will be capitalized.
			fixedPath = Regex.Replace(fixedPath, @"^([A-Za-z]+)(\:)", new MatchEvaluator(replaceDrivenameCapitalized));

			outputPath = new DirectoryInfo(fixedPath);
			fixCapitalization(ref outputPath);
			fixedPath = outputPath.FullName;

			// Ensure that path ends in a backslash (directory separator)
			// Note that we need to do this, although it may not seem correct at first;
			// if the user wants to backup a root directory, always removing the
			// trailing slash would result in eg. C: instead of eg. C:\
			// At least this way we are consistent and put a slash at the end of ALL
			// paths.
			if ((outputPath.FullName.Substring(outputPath.FullName.Length-1) != "\\") && (outputPath.FullName.Substring(outputPath.FullName.Length-1) != "/")) {
				fixedPath = outputPath.FullName + "\\";
				outputPath = new DirectoryInfo(fixedPath);
			}

			// All OK...
			errorHolder = "";
			return true;
		}

		/// <summary>
		/// Performs a 'sanity check' on the passed Carbon Copy Options object.  If crucial value(s) are missing, lists them in the error string.
		/// </summary>
		/// <param name="toCheck">The CCO object to check.</param>
		/// <param name="errors">Holds the error message(s) of the errors found by the sanity check.</param>
		/// <returns>True if there were no errors, otherwise false.</returns>
		public bool SanityCheck(CCO toCheck, out string errors) {
			bool foundErrors = false;

			errors = "";

			if (toCheck == null) {
				errors += "- CCO object is null!\r\n";
				foundErrors = true;
				// Fatal error, so return now
				return false;
			}

			if (toCheck.SourceDirs == null || toCheck.SourceDirs.Count == 0) {
				errors += "- One or more source directories are required.\r\n";
				foundErrors = true;
			}

			if (toCheck.DestDir == null || toCheck.DestDir.FullName.Length == 0) {
				errors += "- Destination directory is required.\r\n";
				foundErrors = true;
			}

			// Check whether there are source backup dir dupes
			foreach (DirectoryInfo di1 in toCheck.SourceDirs) {
				// Is it a dupe?
				int dupeCount = 0;
				foreach (DirectoryInfo di2 in toCheck.SourceDirs) {
					if (di1.FullName == di2.FullName) {
						dupeCount++;
						if (dupeCount > 1) {
							errors += "- Found duplicate source directory in source dirs list: " + di1.FullName + "\r\n";
							foundErrors = true;
						}
					}
				}
			}

			if (toCheck.Type == CCOTypeOfBackup.None) {
				errors += "- Type of backup must be selected.\r\n";
				foundErrors = true;
			}

			// We can allow ToDisplay to be 'None'; it will still display the
			// bog-standard messages, so it's a valid selection.

			if (foundErrors) { return false; }
			else { return true; }
		}
		#endregion

		#region Private methods
		private string replaceDrivenameCapitalized(Match m) {
			// Replace this Regex drivename match with an uppercased version
			// Regex should match a path like this:
			// (AA)(:)/abc/xyz
			return m.Groups[1].Value.ToUpper() + m.Groups[2].Value;
		}

		private void fixCapitalization(ref DirectoryInfo diToFix) {
			// If we're at the root, no need to do anything...
			if (diToFix.Parent != null) {
				// First, fix the capitalization of the parent dir
				DirectoryInfo parentDir = diToFix.Parent;
				fixCapitalization(ref parentDir);

				// Get dirs in parent dir
				DirectoryInfo[] dirInfos = parentDir.GetDirectories();

				// Find our current dir as one of the listed dirs and set di to it,
				// thereby fixing our dir's capitalization
				foreach (DirectoryInfo di in dirInfos) {
					if (di.Name.ToString().Equals(diToFix.Name, StringComparison.CurrentCultureIgnoreCase)) {
						diToFix = di;
						break;
					}
				}
			}
		}
		#endregion
	}

	/// <summary>
	/// Provides utilities for saving CCO's to and loading CCO's from XML files.
	/// </summary>
	public class CCOSaveLoad {
		/// <summary>
		/// Save the specified CCO options to the specified file.
		/// </summary>
		/// <param name="options">The CCO to save.</param>
		/// <param name="filePath">The path of the file to save the XML options data to.</param>
		/// <param name="errorTxt">Holds the error message of the Exception if there was an error during save.</param>
		/// <returns>True if there were no errors, otherwise false.</returns>
		public bool Save(CCO options, string filePath, out string errorTxt) {
			errorTxt = "";

			try {
				// Perform sanity check on CCO object
				CCOFunctions optFunc = new CCOFunctions();
				string sanityErrors;
				if (!optFunc.SanityCheck(options, out sanityErrors)) {
					throw new Exception("Error(s) found in backup profile:\r\n" + sanityErrors);
				}

				// First, build up our XmlDocument and store configuration values
				XmlDocument doc = new XmlDocument();
				XmlElement rootElement;
				doc.AppendChild(
					rootElement = doc.CreateElement("carbonCopyOptions")
				);

				XmlElement typeElement = doc.CreateElement("backupType");
				typeElement.SetAttribute("value", ((int)options.Type).ToString());
				rootElement.AppendChild(typeElement);

				XmlElement dryRunElement = doc.CreateElement("isDryRun");
				dryRunElement.SetAttribute("value", options.IsDryRun.ToString());
				rootElement.AppendChild(dryRunElement);

				XmlElement displayElement = doc.CreateElement("outputDetail");
				displayElement.SetAttribute("value", ((int)options.OutputDetail).ToString());
				rootElement.AppendChild(displayElement);

				XmlElement srcElement = doc.CreateElement("srcDirs");
				foreach (DirectoryInfo di in options.SourceDirs) {
					XmlText dirTxt = doc.CreateTextNode(di.FullName);
					XmlElement srcDir = doc.CreateElement("dir");
					srcDir.AppendChild(dirTxt);
					srcElement.AppendChild(srcDir);
				}
				rootElement.AppendChild(srcElement);

				XmlElement destElement = doc.CreateElement("destDir");
				destElement.SetAttribute("value", options.DestDir.FullName);
				rootElement.AppendChild(destElement);

				// Grab an XPathNavigator to navigate thru our entire XmlDocument tree
				XPathNavigator nav = doc.CreateNavigator();

				// Create the XmlWriter that will write the XML document to disk
				XmlWriterSettings sett = new XmlWriterSettings();
				sett.Indent = true;
				sett.IndentChars = "\t";
				XmlWriter writer = XmlWriter.Create(filePath, sett);
				// Finally write it, and close the writer
				nav.WriteSubtree(writer);
				writer.Close();
			}
			catch (Exception ex) {
				errorTxt = ex.Message;
				return false;
			}

			return true;
		}

		/// <summary>
		/// Load the CCO options data from the specified XML file and supply it as a filled CCO object.
		/// </summary>
		/// <param name="options">The CCO options data as a CCO object.</param>
		/// <param name="filePath">The path of the file to load the XML options data from.</param>
		/// <param name="errorTxt">Holds the error message of the Exception if there was an error during load.</param>
		/// <returns>True if there were no errors, otherwise false.</returns>
		public bool Load(out CCO options, string filePath, out string errorTxt) {
			errorTxt = "";
			options = new CCO();

			try {
				CCOFunctions optFunc = new CCOFunctions();

				// First, create our XmlDocument for storing configuration values
				XmlDocument doc = new XmlDocument();

				// Create the XmlReader that will read the XML document from disk
				XmlReader reader = XmlReader.Create(filePath);

				// Read and load it, and close the reader
				doc.Load(reader);
				reader.Close();

				// Create an XPathNavigator so we can easily query values from the XmlDocument
				XPathNavigator nav = doc.CreateNavigator();

				// Now populate the CCO object from our XmlDocument's values
				// Backup type
				XPathNodeIterator typeIter = nav.Select("/carbonCopyOptions/backupType");
				if (!typeIter.MoveNext()) {
					throw new Exception("Couldn't find backupType configuration entry!");
				}
				options.Type = (CCOTypeOfBackup)Convert.ToInt32(typeIter.Current.GetAttribute("value", ""));

				// Is dry run?
				XPathNodeIterator dryRunIter = nav.Select("/carbonCopyOptions/isDryRun");
				if (!dryRunIter.MoveNext()) {
					throw new Exception("Couldn't find isDryRun configuration entry!");
				}
				options.IsDryRun = Convert.ToBoolean(dryRunIter.Current.GetAttribute("value", ""));

				// To display
				XPathNodeIterator displayIter = nav.Select("/carbonCopyOptions/outputDetail");
				if (!displayIter.MoveNext()) {
					throw new Exception("Couldn't find outputDetail configuration entry!");
				}
				options.OutputDetail = (VerbosityLevel)Convert.ToInt32(displayIter.Current.GetAttribute("value", ""));

				// Source dirs
				options.SourceDirs = new List<DirectoryInfo>();
				XPathNodeIterator srcIter = nav.Select("/carbonCopyOptions/srcDirs/dir");
				while (srcIter.MoveNext()) {
					string pathString = srcIter.Current.Value;
					string errorHolder;
					DirectoryInfo fixedPath = null;

					// Is path string valid?
					if (!optFunc.CheckDirValidity(pathString, ref fixedPath, out errorHolder)) {
						throw new Exception(errorHolder);
					}
					options.SourceDirs.Add(fixedPath);
				}

				// Dest dir
				XPathNodeIterator destIter = nav.Select("/carbonCopyOptions/destDir");
				if (!destIter.MoveNext()) {
					throw new Exception("Couldn't find destDir configuration entry!");
				}
				string pathString2 = destIter.Current.GetAttribute("value", "");
				string errorHolder2;
				DirectoryInfo fixedPath2 = null;
				// Is path string valid?
				if (!optFunc.CheckDirValidity(pathString2, ref fixedPath2, out errorHolder2)) {
					throw new Exception(errorHolder2);
				}
				options.DestDir = fixedPath2;

				// Finally, sanity check our populated CCO object
				string sanityErrors;
				if (!optFunc.SanityCheck(options, out sanityErrors)) {
					throw new Exception("Error(s) found in backup profile file:\r\n" + sanityErrors);
				}
			}
			catch (Exception ex) {
				errorTxt = ex.Message;
				return false;
			}

			return true;
		}
	}
}
