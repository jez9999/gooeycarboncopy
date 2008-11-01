using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Carbon_Copy {
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
	/// Enumeration indicating what type of backup to perform.
	/// </summary>
	public enum CCOTypeOfBackup {
		None,
		CarbonCopy,
		Incremental,
	}
	
	/// <summary>
	/// Enumeration of flags indicating what type(s) of output to display during backup.
	/// </summary>
	[FlagsAttribute]
	public enum CCOWhatToDisplay {
		None =      0,
		Comments =  1,
		Errors =    2,
		Verbose =   4,
	}
	
	/// <summary>
	/// Contains members to hold the various options relating to a Carbon Copy backup.
	/// </summary>
	public class CCO {
		#region Public vars
		
		public List<DirectoryInfo> SourceDirs = new List<DirectoryInfo>();
		public DirectoryInfo DestDir;
		public CCOTypeOfBackup Type = CCOTypeOfBackup.None;
		public CCOWhatToDisplay ToDisplay = CCOWhatToDisplay.None;
		
		#endregion
	}
	
	/// <summary>
	/// Class that deals with various functions pertaining to Carbon Copy options, including saving to and loading from permenant storage.
	/// </summary>
	public class CCOFunctions {
		#region Public methods
		
		public bool CheckDirValidity(string inputPath, ref DirectoryInfo outputPath, out string errorHolder) {
			string fixedPath = "";
			
			// my $path='c:\\abc\\'; $path =~ s/^([A-Za-z]+)\:/\U$1\E\:/; print $path;
			
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
}
