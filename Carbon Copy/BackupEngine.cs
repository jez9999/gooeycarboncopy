using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;

namespace Carbon_Copy {	
	#region Backup engine
	
	/// <summary>
	/// A class that implements a backup engine, which will backup certain directories that are to be specified by the calling code, either by synchronizing them with another set in another location, or by performing an incremental backup in another location.
	/// </summary>
	public class BackupEngine {
		#region Private vars
		
		private Thread backupWorker;
		private bool stopBackup = false;
		private Object endBackupCleanupLock = new Object();
		private Object stopBackupLock = new Object();
		private CCO options;
		private Queue<MsgDisplayInfo> messages = new Queue<MsgDisplayInfo>();
		
		#endregion
		
		#region Public vars
		
		public event MsgFunctionDelegate CbMsg;
		public event MsgFunctionDelegate CbCommentMsg;
		public event MsgFunctionDelegate CbErrorMsg;
		public event MsgFunctionDelegate CbVerboseMsg;
		public event DisplayNextMsgCallbackInvoker CbDisplayNextMessage;
		public event BackupFinishedCallbackInvoker CbBackupFinished;
		
		public bool IsRunningBackup {
			get {
				return (backupWorker != null);
			}		
		}
		
		#endregion
		
		#region Constructors
		
		public BackupEngine(CCO options) {
			this.options = options;
		}
		
		#endregion
		
		#region Public methods
		
		public MsgDisplayInfo GetNextMsg() {
			return messages.Dequeue();
		}
		
		public void StartBackup() {
			// Only do this if we don't already have a backup worker thread going
			if (!IsRunningBackup) {
				checkNecessarySettings();
				
				// OK; setup new thread to do stuff, then return
				backupWorker = new Thread(new ThreadStart(backupWorkerGo));
				backupWorker.Start();
			}
			else {
				throw new BackupEngineException("Can't start backup; a backup is already in progress.");
			}
			return;
		}
		
		public void StopBackup() {
			stopBackup = true;
		}
		
		#endregion
		
		#region Private methods
		
		private void checkNecessarySettings() {
			// Ensure that callbacks allowing us to communicate with the calling
			// code have been set
			if (
				this.CbMsg == null ||
				this.CbCommentMsg == null ||
				this.CbErrorMsg == null ||
				this.CbVerboseMsg == null ||
				this.CbDisplayNextMessage == null ||
				this.CbBackupFinished == null
			) {
				throw new BackupEngineException("All callbacks/delegates must be set before backup can begin.");
			}
		}
		
		private void endBackupCleanup() {
			lock(endBackupCleanupLock) {
				if (backupWorker != null) {
					stopBackup = false;
					
					// Setting this to null should be the LAST thing we do in the cleanup.  Only
					// after this may a caller begin another backup, so all cleanup must be done
					// before this.
					backupWorker = null;
				}
			}
			
			if (CbBackupFinished != null) { CbBackupFinished(); }
		}
		
		/// <summary>
		/// Initial backup process thread method; begins the actual process of backing up specified files to specified backup directory.
		/// </summary>
		private void backupWorkerGo() {
			DirectoryInfo fixedPath = null;
			string errorHolder;
			CCOFunctions optFunc = new CCOFunctions();
			
			if (stopBackup) {
				endBackupCleanup();
				return;
			}
			
			// Check that we have valid options before starting the backup
			if (
				options.Type == CCOTypeOfBackup.None ||
				options.SourceDirs.Count == 0 ||
				options.DestDir == null
			) {
				AddMsg(new MsgDisplayInfo(CbErrorMsg, "Not all necessary options passed!"));
				endBackupCleanup();
				return;
			}
			
			// List directory and configuration information
			foreach (DirectoryInfo di in options.SourceDirs) {
				AddMsg(new MsgDisplayInfo(CbVerboseMsg, "Found source backup directory: " + di.FullName));
			}
			AddMsg(new MsgDisplayInfo(CbVerboseMsg, "Found destination backup directory: " + options.DestDir.FullName));
			AddMsg(new MsgDisplayInfo(CbVerboseMsg, "Type of backup: " + options.Type.ToString()));
			AddMsg(new MsgDisplayInfo(CbVerboseMsg, "What to display: " + options.ToDisplay.ToString()));
			
			// Ensure that all source backup dirs are valid and 'touch them up'
			// (setting them to fixedPath fixes their capitalization and terminates
			// them all with a back or forward slash)
			for (int i=0; i < options.SourceDirs.Count; i++) {
				if (!optFunc.CheckDirValidity(options.SourceDirs[i].FullName, ref fixedPath, out errorHolder)) {
					AddMsg(new MsgDisplayInfo(CbErrorMsg, errorHolder));
					endBackupCleanup();
					return;
				}
				else {
					options.SourceDirs[i] = fixedPath;
				}
			}
			
			// Ensure that there are no source backup dir dupes
			foreach (DirectoryInfo di1 in options.SourceDirs) {
				// Is it a dupe?
				int dupeCount = 0;
				foreach (DirectoryInfo di2 in options.SourceDirs) {
					if (di1.FullName == di2.FullName) {
						dupeCount++;
						if (dupeCount > 1) {
							AddMsg(new MsgDisplayInfo(CbErrorMsg, "Directory '" + di1.FullName + "' is duplicated in the source directories list."));
							endBackupCleanup();
							return;
						}
					}
				}
			}
			
			// Check that destination backup dir is valid
			if (!optFunc.CheckDirValidity(options.DestDir.FullName, ref fixedPath, out errorHolder)) {
				AddMsg(new MsgDisplayInfo(CbErrorMsg, errorHolder));
				endBackupCleanup();
				return;
			}
			else {
				options.DestDir = fixedPath;
			}
			
			AddMsg(new MsgDisplayInfo(CbCommentMsg, "Starting backup."));
			
			foreach (DirectoryInfo sourceDir in options.SourceDirs) {
				if (stopBackup) {
					endBackupCleanup();
					return;
				}
				
				// Backup this source directory tree
				AddMsg(new MsgDisplayInfo(CbCommentMsg, "Synchronizing base source directory " + sourceDir.FullName));
				
				// TODO: Make this so we just call a func to backup S to S1
				try {
//					// Ensure that the base backup dir path exists
//					string destDirBaseSourceDirName = Regex.Replace(sourceDir.FullName, @"^\\\\", @"\\_unc_\\");
//					destDirBaseSourceDirName = options.DestDir.FullName + Regex.Replace(destDirBaseSourceDirName, @"\:", @"");
//					DirectoryInfo destDirBaseSourceDir = null;
//					try {
//						// TODO: Fix this created date/time
//						destDirBaseSourceDir = Directory.CreateDirectory(destDirBaseSourceDirName);
//					}
//					catch (Exception ex) {
//						endBackupCleanup();
//						AddMsg(new MsgDisplayInfo(CbErrorMsg, "Error creating base backup directory: " + ex.Message.ToString()));
//						return;
//					}
					
					traverseDir(sourceDir, options.DestDir);
				}
				catch (StopBackupException) {
					endBackupCleanup();
					return;
				}
				catch (Exception ex) {
					endBackupCleanup();
					AddMsg(new MsgDisplayInfo(CbErrorMsg, "BACKUP HALTED... Misc. error occurred: " + ex.Message.ToString()));
					return;
				}
			}
			
			// We finished!
			AddMsg(new MsgDisplayInfo(CbCommentMsg, "Backup finished successfully."));
			endBackupCleanup();
			return;
		}
		
		private void traverseDir(DirectoryInfo sourceDir, DirectoryInfo baseDestDir) {
			// Synchronize current source directory
			
			// The plan of action is:
			// 1. Get full dest dir path from base dest dir & source dir path
			// 2. Synchronize THE source dir to dest dir.  Not its child objects, no
			//    recursion yet; we're JUST creating the source dir in the dest dir.
			// 3. Do the synchronization of the child dir to the dest dir proper; delete
			//    objects in the dest dir that don't exist in the source dir, then
			//    synchronize (modify/create) objects, that do exist in the source dir,
			//    to the dest dir.
			// 4. Set the attributes of the dest dir, as we won't be modifying anything
			//    underneath it now (so its modified date/time won't change after this).
			
			// 1. Figure out destination dir path
			string destDirPath = Regex.Replace(sourceDir.FullName, @"^\\\\", @"\\_unc_\\");
			destDirPath = baseDestDir.FullName + Regex.Replace(destDirPath, @"\:", @"");
			
			AddMsg(new MsgDisplayInfo(CbMsg, "Synchronizing " + sourceDir.FullName + " to " + destDirPath));
			
			// Remove last dir off end; we want to synchronize TO this one
			// eg. 'X:\backuptest\C\testBackupDir\' becomes 'X:\backuptest\C\'
			Match destDirTrimmed = Regex.Match(destDirPath, @"^(.*\\).*\\");
			destDirPath = destDirTrimmed.Groups[1].Value;
			
			// Get DirectoryInfo for destination dir; create dir if necessary
			DirectoryInfo destDir = null;
			try {
				destDir = Directory.CreateDirectory(destDirPath);
			}
			catch (Exception ex) {
				AddMsg(new MsgDisplayInfo(CbErrorMsg, "Problem creating base backup directory: " + ex.Message.ToString()));
				throw new StopBackupException();
			}
			
			// 2. Synchronize this directory...
			List<FileSystemInfo> sourceDirInList = new List<FileSystemInfo>();
			sourceDirInList.Add(sourceDir);
			
			try {
				List<FileSystemInfo> syncedDirInList = synchronizeObjs(sourceDirInList, destDir, false);
				// We know the synchronized dest dir is the first entry in the list as it's
				// the only object we passed to be synchronized!
				DirectoryInfo syncedDir = ((DirectoryInfo)syncedDirInList[0]);
				syncedDir = slashTerm(syncedDir);
				
				// 3. Synchronize source directory's child nodes
				List<DirectoryInfo> childDirs = synchronizeDir(sourceDir, syncedDir);
				syncedDir = null;
				
				// Now synchronize source directory's child dirs recursively...
				// a bunch of calls to traverseDir, methinks
				foreach (DirectoryInfo childDir in childDirs) {
					traverseDir(childDir, baseDestDir);
				}
				
				// 4. Finally, set this directory's attributes and datetimes correctly; we
				// didn't do this at the beginning, as the modified datetime was going to
				// change when we synchronized its contents; therefore, it must be done last.
				synchronizeObjs(sourceDirInList, destDir, true);
			}
			catch (SynchronizeObjsException ex) {
				// Oh dear... just output error and move on.
				AddMsg(new MsgDisplayInfo(CbErrorMsg, ex.Message));
			}
			catch (SynchronizeDirException ex) {
				// Oh dear... just output error and move on.
				AddMsg(new MsgDisplayInfo(CbErrorMsg, ex.Message));
			}
			
			// TODO - I think we can get rid of the below comment, and code below the
			// return; now.  :-)  It's just left in case we discover we need some of it
			// for some reason.
			// jez marker - WE WANT TO REMOVE THE CODE BELOW, AND SYNC CHILD DIRS ABOVE
			return;
			
			
			
//			destDirPath = Regex.m
//			synchronizeObjs
			
			
			
//			AddMsg(new MsgDisplayInfo(CbMsg, "Synchronizing " + sourceDir.FullName + " to " + destDir.FullName));
//			
//			List<DirectoryInfo> childDirs = synchronizeDir(sourceDir, destDir);
//			if (stopBackup) {
//				throw new StopBackupException();
//			}
			
//			// Now synchronize its children recursively
//			AddMsg(new MsgDisplayInfo(CbVerboseMsg, "Synchronizing children of " + sourceDir.FullName));
//			foreach (DirectoryInfo childDir in childDirs) {
//				if (stopBackup) {
//					throw new StopBackupException();
//				}
//				
//				DirectoryInfo cd = slashTerm(childDir);
//				DirectoryInfo dd = new DirectoryInfo(destDir.FullName + childDir.Name);
//				// .net doesn't, by default, put a backslash on the .FullName of a
//				// DirectoryInfo when you get a list of them using something like
//				// .GetDirectories(); however, if you manually create a DirectoryInfo
//				// passing a path ending with a backslash as its path constructor,
//				// the .FullName DOES have the backslash at the end.
//				// We need to have a backslash at the end of all directory paths
//				// (see CCOptions.cs for reasoning as to why), so we must manually
//				// recreate the DirectoryInfo with the constructor string having the
//				// backslash at the end.
//				
//				AddMsg(new MsgDisplayInfo(CbVerboseMsg, "Child found: " + cd.FullName));
//				
//				traverseDir(cd, dd);
//			}
		}
		
		private DirectoryInfo slashTerm(DirectoryInfo inputDir) {
			// Take the input directory's DirectoryInfo, and return one with the same
			// path, but whose .FullName is guaranteed to be terminated with a slash.
			
			if ((inputDir.FullName.Substring(inputDir.FullName.Length-1) != "\\") && (inputDir.FullName.Substring(inputDir.FullName.Length-1) != "/")) {
				return new DirectoryInfo(inputDir.FullName + "\\");
			}
			else { return inputDir; }
		}
		
		private void synchronizeObjs(List<FileSystemInfo> sourceObjs, DirectoryInfo destDir) {
			// By default, DO synchronize directory attributes
			synchronizeObjs(sourceObjs, destDir, true);
		}
		/// <summary>
		/// Synchronizes the FileSystemInfo objects passed into the destination directory specified by the DirectoryInfo passed.  This ensures that all source FileSystemInfo objects passed will exist in the destination directory passed.  It DOES NOT delete entries in the destination dir that have not been specified in the source FileSystemInfo objects list; that must be done elsewhere.
		/// </summary>
		/// <param name="sourceObjs">The list of source FileSystemInfo objects to be synchronized to the destination dir.</param>
		/// <param name="destDir">The destination dir DirectoryInfo object.</param>
		/// <param name="syncDirAttributes">A boolean specifying whether the destination directories that are being synchronized should have their attributes set to match those of the corresponding source directories, or not.</param>
		/// <returns>A list of FileSystemInfo objects which are descriptors of the DESTINATION DIR objects that have been synchronized (ie. they'll have the .FullName set to the DESTINATION DIR's path to that FileSystemInfo object).</returns>
		private List<FileSystemInfo> synchronizeObjs(List<FileSystemInfo> sourceObjs, DirectoryInfo destDir, bool syncDirAttributes) {
			// Synchronize objects in source objects list into given destination dir
			// Returns a FileSystemInfo list containing info on each object (file or
			// directory) that was synchronized.
			
			List<FileSystemInfo> retVal = new List<FileSystemInfo>();
			
			// 6 basic operations may need to be performed as part of synchronization:
			// - Delete a destination object as it's not in the source dir
			//   NOTE: We're fixing capitalization here, too; when comparing dest
			//   and source file and directory names, we do a case-sensitive match.
			//   If the cases are different, the dest file will be removed and replaced.
			// - Replace a destination file as its attributes don't match those
			//   of its namesake in the source dir
			// - Update a destination directory as its attributes don't match those
			//   of its namesake in the source dir
			// - Replace a destination object as its type (file/directory) doesn't
			//   match that of its namesake in the source dir (this is also reflected
			//   in that the object's attributes don't match; one attribute is whether
			//   the object is a directory or not.)
			// - Add a source object to the destination dir as it doesn't exist in the
			//   destination dir
			// - Leave a destination object alone
			
			if (stopBackup) {
				throw new StopBackupException();
			}
			
			// Make array list of dest dir's objects; they seem to be in
			// alphabetical order already...
			FileInfo[] destFilesTemp;
			DirectoryInfo[] destDirsTemp;
			
			try {
				destFilesTemp = destDir.GetFiles();
				destDirsTemp = destDir.GetDirectories();
			}
			catch (Exception ex) {
				throw new SynchronizeObjsException("Couldn't get file or directory list - " + ex.Message);
			}
			
			List<FileSystemInfo> destObjs = new List<FileSystemInfo>();
			destObjs.AddRange(destFilesTemp);
			destObjs.AddRange(destDirsTemp);
			
			List<FileSystemInfo> newDestObjs = new List<FileSystemInfo>();
			
			// First, sync objects that already exist in the destination dir and that
			// are specified in the source objects list.
			foreach (FileSystemInfo obj in destObjs) {
				if (stopBackup) {
					throw new StopBackupException();
				}
				
				bool objectsIdentical;
				int foundIndex;
				// If destination object has the same name as an object in the
				// source objects list, synchronize it
				if ((foundIndex = indexNameXinY(obj, sourceObjs)) >= 0) {
					// Synchronize
					
					objectsIdentical = true;
					if (
						obj.Attributes != sourceObjs[foundIndex].Attributes &&
						!(obj is DirectoryInfo && sourceObjs[foundIndex] is DirectoryInfo)
					) {
						// Attributes (including whether the object is a directory)
						// different.  If they're both directories we can leave the dest
						// dir object and simply change its attributes later.  Otherwise,
						// we have to delete the dest dir object and copy it across
						// later...
						
						AddMsg(new MsgDisplayInfo(CbVerboseMsg, "Deleting " + obj.FullName + " - attributes or type different from that in source dir."));
						forciblyKillObject(obj);
						objectsIdentical = false;
					}
					else if (
						obj is FileInfo &&
						(
							sourceObjs[foundIndex].CreationTimeUtc != obj.CreationTimeUtc ||
							sourceObjs[foundIndex].LastWriteTimeUtc != obj.LastWriteTimeUtc
						)
					) {
						// Delete dest file - last creation or write time different.
						// If dest object is a directory, we don't need to do this as
						// we can simply change its created/modified times later.
						
						AddMsg(new MsgDisplayInfo(CbVerboseMsg, "Deleting " + obj.FullName + " - last creation or write time different from that in source dir."));
						forciblyKillObject(obj);
						objectsIdentical = false;
					}
					else if (
						obj is FileInfo &&
						// It's a file, and we know that the types of the dest and src
						// objs are the same.  Check their lengths to make sure they match.
						( ((FileInfo)obj).Length != ((FileInfo)sourceObjs[foundIndex]).Length )
					) {
						// Delete dest file - sizes differ
						AddMsg(new MsgDisplayInfo(CbVerboseMsg, "Deleting " + obj.FullName + " - size of file different from that of file in source dir."));
						forciblyKillObject(obj);
						objectsIdentical = false;
					}
					
					try {
						// We may need to change attributes and/or datetimes to make
						// dirs with the same name identical.
						if (
							obj is DirectoryInfo &&
							sourceObjs[foundIndex] is DirectoryInfo &&
							syncDirAttributes
						) {
							if (obj.Attributes != sourceObjs[foundIndex].Attributes) {
								AddMsg(new MsgDisplayInfo(CbVerboseMsg, "Setting attributes for directory " + obj.FullName + " - dest dir attributes different from source dir attributes."));
								obj.Attributes = sourceObjs[foundIndex].Attributes;
							}
							if (
								obj.CreationTimeUtc != sourceObjs[foundIndex].CreationTimeUtc ||
								obj.LastWriteTimeUtc != sourceObjs[foundIndex].LastWriteTimeUtc
							) {
								AddMsg(new MsgDisplayInfo(CbVerboseMsg, "Setting created/modified datetimes for directory " + obj.FullName + " - dest dir datetimes different from source dir datetimes."));
								obj.CreationTimeUtc = sourceObjs[foundIndex].CreationTimeUtc;
								obj.LastWriteTimeUtc = sourceObjs[foundIndex].LastWriteTimeUtc;
							}
						}
					}
					catch (Exception ex) {
						AddMsg(new MsgDisplayInfo(CbErrorMsg, "Problem setting dest dir attributes: " + ex.Message.ToString()));
					}
					
					if (objectsIdentical) {
						// Objects are identical according to all the above tests; don't
						// delete the dest object, and record its continued existance in
						// our new list.
						newDestObjs.Add(obj);
						
						// Add to list of objects synchronized
						retVal.Add(obj);
					}
				}
			}
			
			destObjs = newDestObjs;
			
			// Now, copy over source objects that need to be copied.
			foreach (FileSystemInfo obj in sourceObjs) {
				if (stopBackup) {
					throw new StopBackupException();
				}
				
				int foundIndex;
				
				// FILE
				if (obj is FileInfo) {
					if ((foundIndex = indexNameXinY(obj, destObjs)) < 0) {
						// Need to copy
						try {
							string copyToPath = destDir.FullName + obj.Name;
							// The 'Copying' verbose message tends to result in ENORMOUS
							// output when dealing with large directories.  This warrants
							// a warnings in the GUI somewhere around the 'display
							// verbose messages' checkbox.
							AddMsg(new MsgDisplayInfo(CbVerboseMsg, "Copying " + obj.FullName + " to " + copyToPath));
							((FileInfo)obj).CopyTo(copyToPath);
							FileInfo justCopied = new FileInfo(copyToPath);
							justCopied.Attributes &= ~FileAttributes.ReadOnly;
							justCopied.CreationTimeUtc = obj.CreationTimeUtc;
							justCopied.LastWriteTimeUtc = obj.LastWriteTimeUtc;
							justCopied.Attributes = obj.Attributes;
							
							// Add to list of objects synchronized
							retVal.Add(justCopied);
						}
						catch (Exception ex) {
							AddMsg(new MsgDisplayInfo(CbErrorMsg, "Couldn't copy file " + obj.FullName + " - " + ex.Message));
						}
					}
				}
				// DIR
				else if (obj is DirectoryInfo) {
					if ((foundIndex = indexNameXinY(obj, destObjs)) < 0) {
						// Need to copy
						try {
							string createPath = destDir.FullName + obj.Name + "\\";
							AddMsg(new MsgDisplayInfo(CbVerboseMsg, "Creating directory " + createPath));
							Directory.CreateDirectory(createPath);
							DirectoryInfo justCreated = new DirectoryInfo(createPath);
							justCreated.CreationTimeUtc = obj.CreationTimeUtc;
							justCreated.LastWriteTimeUtc = obj.LastWriteTimeUtc;
							justCreated.Attributes = obj.Attributes;
							
							// Add to list of objects synchronized
							retVal.Add(justCreated);
						}
						catch (Exception ex) {
							AddMsg(new MsgDisplayInfo(CbVerboseMsg, "Couldn't create directory " + obj.FullName + " - " + ex.Message));
						}
					}
				}
			}
			
			return retVal;
		}
		
		/// <summary>
		/// Synchronizes two specified directories by first deleting objects in the destination dir that don't exist in the source dir, then synchronizing the two dirs using synchronizeObjs.
		/// </summary>
		/// <param name="sourceDir">The dir to be synchronized from.</param>
		/// <param name="destDir">The dir to be synchronized to.</param>
		/// <returns>A list of DirectoryInfo objects containing the child directories of the given source directory.</returns>
		private List<DirectoryInfo> synchronizeDir(DirectoryInfo sourceDir, DirectoryInfo destDir) {
			if (stopBackup) {
				throw new StopBackupException();
			}
			
			// Make array list of source and dest dirs' objects; they seem to be in
			// alphabetical order already...
			
			List<DirectoryInfo> childDirs = new List<DirectoryInfo>();
			
			FileInfo[] srcFilesTemp;
			FileInfo[] destFilesTemp;
			DirectoryInfo[] srcDirsTemp;
			DirectoryInfo[] destDirsTemp;
			
			try {
				srcFilesTemp = sourceDir.GetFiles();
				destFilesTemp = destDir.GetFiles();
				srcDirsTemp = sourceDir.GetDirectories();
				destDirsTemp = destDir.GetDirectories();
			}
			catch (Exception ex) {
				throw new SynchronizeDirException("Couldn't get file or directory list - " + ex.Message);
			}
			foreach (DirectoryInfo di in srcDirsTemp) {
				childDirs.Add(slashTerm(di));
			}
			
			List<FileSystemInfo> srcObjs = new List<FileSystemInfo>();
			List<FileSystemInfo> destObjs = new List<FileSystemInfo>();
			
			srcObjs.AddRange(srcFilesTemp);
			srcObjs.AddRange(srcDirsTemp);
			destObjs.AddRange(destFilesTemp);
			destObjs.AddRange(destDirsTemp);
			
			// synchronizeObjs will synchronize objects to the destination dir listed
			// in the 'source objects' FileSystemInfo list passed, but will not remove
			// objects in the destination dir that don't exist in the source dir.  We
			// must do that here.
			
			List<FileSystemInfo> newDestObjs = new List<FileSystemInfo>();
			// 6 basic operations may need to be performed as part of synchronization:
			// - Delete a destination object as it's not in the source dir
			//   NOTE: We're fixing capitalization here, too; when comparing dest
			//   and source file and directory names, we do a case-sensitive match.
			//   If the cases are different, the dest file will be removed and replaced.
			// - Replace a destination file as its attributes don't match those
			//   of its namesake in the source dir
			// - Update a destination directory as its attributes don't match those
			//   of its namesake in the source dir
			// - Replace a destination object as its type (file/directory) doesn't
			//   match that of its namesake in the source dir (this is also reflected
			//   in that the object's attributes don't match; one attribute is whether
			//   the object is a directory or not.)
			// - Add a source object to the destination dir as it doesn't exist in the
			//   destination dir
			// - Leave a destination object alone
			foreach (FileSystemInfo obj in destObjs) {
				if (stopBackup) {
					throw new StopBackupException();
				}
				
				// Delete obj if it doesn't exist in source directory
				int foundIndex;
				if ((foundIndex = indexNameXinY(obj, srcObjs)) < 0) {
					// Delete dest object - not found
					AddMsg(new MsgDisplayInfo(CbVerboseMsg, "Deleting " + obj.FullName + " - not found in source dir."));
					forciblyKillObject(obj);
				}
				else {
					// Objects are identical according to all the above tests; don't
					// delete the dest object, and record its continued existance in
					// our new list.
					newDestObjs.Add(obj);
				}
			}
			
			destObjs = newDestObjs;
			
			// Invalid destination objects have been deleted.  Now synchronize source
			// objects.
			synchronizeObjs(srcObjs, destDir);
			
			return childDirs;
		}
		
		/// <summary>
		/// Case-sensitive method to check whether one file system object has the same name (not FULLY QUALIFIED name, just short name) as another in a provided list.
		/// </summary>
		/// <param name="XObj">The object to check the name of.</param>
		/// <param name="YObjs">The list in which to check for the object provided, to see whether it matches the name of a member of this list.</param>
		/// <returns>The zero-indexed index in the list provided of the matching object if the name of one matches (case-sensitive); otherwise -1.</returns>
		private int indexNameXinY(FileSystemInfo XObj, List<FileSystemInfo> YObjs) {
			for (int index=0; index < YObjs.Count; index++) {
				if (YObjs[index].Name == XObj.Name) { return index; }
			}
			
			return -1;
		}
		private int indexNameXinY(FileInfo XObj, List<FileInfo> YObjs) {
			for (int index=0; index < YObjs.Count; index++) {
				if (YObjs[index].Name == XObj.Name) { return index; }
			}
			
			return -1;
		}
		private int indexNameXinY(FileInfo XObj, List<DirectoryInfo> YObjs) {
			for (int index=0; index < YObjs.Count; index++) {
				if (YObjs[index].Name == XObj.Name) { return index; }
			}
			
			return -1;
		}
		private int indexNameXinY(DirectoryInfo XObj, List<FileInfo> YObjs) {
			for (int index=0; index < YObjs.Count; index++) {
				if (YObjs[index].Name == XObj.Name) { return index; }
			}
			
			return -1;
		}
		private int indexNameXinY(DirectoryInfo XObj, List<DirectoryInfo> YObjs) {
			for (int index=0; index < YObjs.Count; index++) {
				if (YObjs[index].Name == XObj.Name) { return index; }
			}
			
			return -1;
		}
		
		/// <summary>
		/// Check whether this is a file or a dir.  Set the file to not readonly, or if it's a dir, set it and its children recursively to not readonly; then, delete it.
		/// </summary>
		/// <param name="obj">File or dir to delete/kill.</param>
		private void forciblyKillObject(FileSystemInfo obj) {
			if (obj is FileInfo) { obj.Attributes &= ~FileAttributes.ReadOnly; }
			else { notReadOnly((DirectoryInfo)obj); }
			
			if (obj is FileInfo) { ((FileInfo)obj).Delete(); }
			else { ((DirectoryInfo)obj).Delete(true); }
		}
		
		/// <summary>
		/// Recursively set the directory and its contents to NOT readonly.
		/// </summary>
		/// <param name="dir">The directory to perform the operation on.</param>
		private void notReadOnly(DirectoryInfo dir) {
			dir.Attributes &= ~FileAttributes.ReadOnly;
			
			FileInfo[] dirFiles = dir.GetFiles();
			foreach (FileInfo file in dirFiles) {
				file.Attributes &= ~FileAttributes.ReadOnly;
			}
			
			DirectoryInfo[] dirSubdirs = dir.GetDirectories();
			foreach (DirectoryInfo dirSubdir in dirSubdirs) {
				notReadOnly(dirSubdir);
			}
		}
		
		/// <summary>
		/// Add a message to the FIFO message queue, then inform the message handling delegate that there's a new message waiting to be displayed.
		/// </summary>
		/// <param name="msgDisplayInfo">The object that describes various message attributes, and the message itself.</param>
		private void AddMsg(MsgDisplayInfo msgDisplayInfo) {
			messages.Enqueue(msgDisplayInfo);
			
			CbDisplayNextMessage(GetNextMsg);
		}
		
		#endregion
	}
	
	#endregion
	
	#region Delegates and containers
	
	/// <summary>
	/// A class that holds information on a message that is to be supplied to the calling code.  This will be put in FIFO queue, ready to be pulled out by the calling code's message handler.
	/// </summary>
	public class MsgDisplayInfo {
		public MsgFunctionDelegate MsgFunction;
		public string MsgText;
		
		public MsgDisplayInfo(MsgFunctionDelegate msgFunction, string msgText) {
			this.MsgFunction = msgFunction;
			this.MsgText = msgText;
		}
	}
	
	/// <summary>
	/// Delegate for callbacks to message functions - these functions are to deal with the message supplied (in string format) in a way they see fit, and are to be defined by the calling code.
	/// </summary>
	/// <param name="msg">The message to display.</param>
	public delegate void MsgFunctionDelegate(string msg);
	
	/// <summary>
	/// Delegate for callbacks indicating that the GUI thread should retreive the next message from the message queue.
	/// </summary>
	public delegate MsgDisplayInfo GetNextMessageCallback();
	
	// The idea behind the 'invoker' delegate is that the invoker is what is called
	// by the BackupEngine, and the calling code can do something there that allows
	// it to avoid some exception it may get in trying to eg. display message (in
	// the case of a form, an ObjectDisposedException if the form is closed before
	// the message is displayed), and then call the actual callback that the invoker
	// corresponds to.  So, the BackupEngine will only ever call the invoker
	// delegate, but the other delegate is made available for the calling code to
	// use.
	public delegate void DisplayNextMsgCallbackInvoker(GetNextMessageCallback getNextMsgCb);
	public delegate void DisplayNextMsgCallback(GetNextMessageCallback getNextMsgCb);
	
	// Delegates for callbacks indicating that the backup process has finished.
	public delegate void BackupFinishedCallbackInvoker();
	public delegate void BackupFinishedCallback();
	
	#endregion
	
	#region Exceptions
	
	public class BackupEngineException : Exception {
		// General backup engine exception class
		
		#region Private vars
		
		private const string defaultMsg = "There was a miscellaneous error with the backup engine.";
		// ^ As any 'const' is made a compile-time constant, this text will obviously be
		// available to the constructors before the object has been instantiated, as is
		// necessary.
		
		#endregion
		
		#region Constructors
		// Note that the 'public ClassName(...): base() {' notation is explicitly telling
		// the compiler to call this class's base class's empty constructor.  A constructor
		// HAS to call either a base() or this() constructor before its own body, and the
		// '(...): base()' notation (with the colon) is the way to do it explicitly.  If you
		// don't use this notation, base() will be implicitly called.  Therefore, this:
		// public ClassName(...) {...}
		// is identical to this:
		// public ClassName(...): base() {...}
		// 
		// For more information, see:
		// http://msdn2.microsoft.com/en-us/library/aa645603.aspx
		// http://www.jaggersoft.com/csharp_standard/17.10.1.htm
		
		public BackupEngineException(): base(defaultMsg) {
			// No further implementation
		}
		
		public BackupEngineException(string message): base(message) {
			// No further implementation
		}
		
		public BackupEngineException(string message, Exception innerException): base(message, innerException) {
			// No further implementation
		}
		
		protected BackupEngineException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context) {
			// No further implementation
		}
		
		#endregion
	}
	
	public class StopBackupException : Exception {
		// Exception thrown when worker backup thread's code is signaled to stop during
		// a backup operation
		
		#region Private vars
		
		private const string defaultMsg = "Backup stopped.";
		// ^ As any 'const' is made a compile-time constant, this text will obviously be
		// available to the constructors before the object has been instantiated, as is
		// necessary.
		
		#endregion
		
		#region Constructors
		// Note that the 'public ClassName(...): base() {' notation is explicitly telling
		// the compiler to call this class's base class's empty constructor.  A constructor
		// HAS to call either a base() or this() constructor before its own body, and the
		// '(...): base()' notation (with the colon) is the way to do it explicitly.  If you
		// don't use this notation, base() will be implicitly called.  Therefore, this:
		// public ClassName(...) {...}
		// is identical to this:
		// public ClassName(...): base() {...}
		// 
		// For more information, see:
		// http://msdn2.microsoft.com/en-us/library/aa645603.aspx
		// http://www.jaggersoft.com/csharp_standard/17.10.1.htm
		
		public StopBackupException(): base(defaultMsg) {
			// No further implementation
		}
		
		public StopBackupException(string message): base(message) {
			// No further implementation
		}
		
		public StopBackupException(string message, Exception innerException): base(message, innerException) {
			// No further implementation
		}
		
		protected StopBackupException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context) {
			// No further implementation
		}
		
		#endregion
	}
	
	public class SynchronizeDirException : Exception {
		// Exception in synchronizing source dir's directory to dest dir
		
		#region Private vars
		
		private const string defaultMsg = "There was a miscellaneous error synchronizing the source dir's directory contents to the destination dir.";
		// ^ As any 'const' is made a compile-time constant, this text will obviously be
		// available to the constructors before the object has been instantiated, as is
		// necessary.
		
		#endregion
		
		#region Constructors
		// Note that the 'public ClassName(...): base() {' notation is explicitly telling
		// the compiler to call this class's base class's empty constructor.  A constructor
		// HAS to call either a base() or this() constructor before its own body, and the
		// '(...): base()' notation (with the colon) is the way to do it explicitly.  If you
		// don't use this notation, base() will be implicitly called.  Therefore, this:
		// public ClassName(...) {...}
		// is identical to this:
		// public ClassName(...): base() {...}
		// 
		// For more information, see:
		// http://msdn2.microsoft.com/en-us/library/aa645603.aspx
		// http://www.jaggersoft.com/csharp_standard/17.10.1.htm
		
		public SynchronizeDirException(): base(defaultMsg) {
			// No further implementation
		}
		
		public SynchronizeDirException(string message): base(message) {
			// No further implementation
		}
		
		public SynchronizeDirException(string message, Exception innerException): base(message, innerException) {
			// No further implementation
		}
		
		protected SynchronizeDirException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context) {
			// No further implementation
		}
		
		#endregion
	}
	
	public class SynchronizeObjsException : Exception {
		// Exception in synchronizing source dir's objects to dest dir
		
		#region Private vars
		
		private const string defaultMsg = "There was a miscellaneous error synchronizing the source dir's objects to the destination dir.";
		// ^ As any 'const' is made a compile-time constant, this text will obviously be
		// available to the constructors before the object has been instantiated, as is
		// necessary.
		
		#endregion
		
		#region Constructors
		// Note that the 'public ClassName(...): base() {' notation is explicitly telling
		// the compiler to call this class's base class's empty constructor.  A constructor
		// HAS to call either a base() or this() constructor before its own body, and the
		// '(...): base()' notation (with the colon) is the way to do it explicitly.  If you
		// don't use this notation, base() will be implicitly called.  Therefore, this:
		// public ClassName(...) {...}
		// is identical to this:
		// public ClassName(...): base() {...}
		// 
		// For more information, see:
		// http://msdn2.microsoft.com/en-us/library/aa645603.aspx
		// http://www.jaggersoft.com/csharp_standard/17.10.1.htm
		
		public SynchronizeObjsException(): base(defaultMsg) {
			// No further implementation
		}
		
		public SynchronizeObjsException(string message): base(message) {
			// No further implementation
		}
		
		public SynchronizeObjsException(string message, Exception innerException): base(message, innerException) {
			// No further implementation
		}
		
		protected SynchronizeObjsException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context) {
			// No further implementation
		}
		
		#endregion
	}
	
	#endregion
}
