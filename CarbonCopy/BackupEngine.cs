using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using JunctionPoint;
using GooeyUtilities.General;

namespace CarbonCopy {	
	#region Backup engine
	/// <summary>
	/// A class that implements a backup engine, which will backup certain directories that are to be specified by the calling code, either by synchronizing them with another set in another location, or by performing an incremental backup in another location.
	/// </summary>
	public class BackupEngine {
		#region Private vars
		private Thread backupWorker;
		private bool stopBackup = false;
		private string currentlyProcessing = "(initializing)";
		private Object endBackupCleanupLock = new Object();
		private Object stopBackupLock = new Object();
		private CCO options;
		private Queue<MsgDisplayInfo> messages = new Queue<MsgDisplayInfo>();
		private long fileCopyCount = 0;
		private long dirCopyCount = 0;
		private List<MsgData> recentMessages = new List<MsgData>();
		#endregion

		#region Public vars
		public event MsgFunctionDelegate CbDebugMsg;
		public event MsgFunctionDelegate CbInfoMsg;
		public event MsgFunctionDelegate CbErrorMsg;
		public event MsgFunctionDelegate CbVerboseMsg;
		public event DisplayNextMsgCallbackInvoker CbDisplayNextMessage;
		public event BackupFinishedCallbackInvoker CbBackupFinished;

		public bool IsRunningBackup {
			get {
				return (backupWorker != null);
			}		
		}
		public string CurrentlyProcessing {
			get {
				return currentlyProcessing;
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
				this.CbDebugMsg == null ||
				this.CbInfoMsg == null ||
				this.CbErrorMsg == null ||
				this.CbVerboseMsg == null ||
				this.CbDisplayNextMessage == null ||
				this.CbBackupFinished == null
			) {
				throw new BackupEngineException("All callbacks/delegates must be set before backup can begin.");
			}
		}

		private void endBackupCleanup() {
			lock (endBackupCleanupLock) {
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

			// List directory and configuration information
			foreach (DirectoryInfo di in options.SourceDirs) {
				addMsg(VerbosityLevel.Verbose, "Found source backup directory: " + di.FullName);
			}
			addMsg(VerbosityLevel.Verbose, "Found destination backup directory: " + options.DestDir.FullName);
			addMsg(VerbosityLevel.Verbose, "Type of backup: " + options.Type.ToString());
			addMsg(VerbosityLevel.Verbose, "Verbosity level: " + options.OutputDetail.ToString());

			// Ensure that all source backup dirs are valid and 'touch them up'
			// (setting them to fixedPath fixes their capitalization and terminates
			// them all with a back or forward slash)
			for (int i=0; i < options.SourceDirs.Count; i++) {
				if (!optFunc.CheckDirValidity(options.SourceDirs[i].FullName, ref fixedPath, out errorHolder)) {
					addMsg(VerbosityLevel.Error, errorHolder);
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
							addMsg(VerbosityLevel.Error, "Directory '" + di1.FullName + "' is duplicated in the source directories list.");
							endBackupCleanup();
							return;
						}
					}
				}
			}

			// Check that destination backup dir is valid and 'touch it up'
			if (!optFunc.CheckDirValidity(options.DestDir.FullName, ref fixedPath, out errorHolder)) {
				addMsg(VerbosityLevel.Error, errorHolder);
				endBackupCleanup();
				return;
			}
			else {
				options.DestDir = fixedPath;
			}

			if (!options.IsDryRun) {
				addMsg(VerbosityLevel.Info, "Starting backup (type " + (options.Type == CCOTypeOfBackup.Incremental ? "incremental" : options.Type == CCOTypeOfBackup.CarbonCopy ? "carbon copy" : "???") + ").");
			}
			else {
				addMsg(VerbosityLevel.Info, "Starting 'dry run' backup (type " + (options.Type == CCOTypeOfBackup.Incremental ? "incremental" : options.Type == CCOTypeOfBackup.CarbonCopy ? "carbon copy" : "???") + ").");
			}

			foreach (DirectoryInfo sourceDir in options.SourceDirs) {
				if (stopBackup) {
					endBackupCleanup();
					return;
				}

				// Backup this source directory tree
				try { currentlyProcessing = sourceDir.FullName; }
				catch (Exception) { }
				if (!options.IsDryRun) {
					addMsg(VerbosityLevel.Info, "Synchronizing base source directory " + sourceDir.FullName);
				}
				else {
					addMsg(VerbosityLevel.Info, "Would synchronize base source directory " + sourceDir.FullName);
				}

				fileCopyCount = dirCopyCount = 0;
				try { traverseDir(sourceDir, options.DestDir); }
				catch (StopBackupException) {
					endBackupCleanup();
					return;
				}
				catch (Exception ex) {
					endBackupCleanup();
					// In case of fatal error, first put out messages from the 'recent messages' buffer
					emitRecentMessages("BACKUP HALTED... ");
					addMsg(VerbosityLevel.Error, "BACKUP HALTED... Misc. error occurred: " + ex.GetFormattedExceptionMessages());
					return;
				}
				finally {
					addMsg(VerbosityLevel.Info, "Finished; " + (options.IsDryRun ? "would've " : "") + "had to copy " + fileCopyCount + " file" + (fileCopyCount == 1 ? "" : "s") + " and " + dirCopyCount + " director" + (dirCopyCount == 1 ? "y" : "ies") + ".");
				}
			}

			// We finished!
			addMsg(VerbosityLevel.Info, "Backup finished successfully.");
			endBackupCleanup();
			return;
		}

		private void traverseDir(DirectoryInfo sourceDir, DirectoryInfo baseDestDir) {
			// Synchronize current source directory
			try { currentlyProcessing = sourceDir.FullName; }
			catch (Exception) { }

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

			addMsg(VerbosityLevel.Debug, "Synchronizing " + sourceDir.FullName + " to " + destDirPath);

			// Remove last dir off end; we want to synchronize TO this one
			// eg. 'X:\backuptest\C\testBackupDir\' becomes 'X:\backuptest\C\'
			Match destDirTrimmed = Regex.Match(destDirPath, @"^(.*\\).*\\");
			destDirPath = destDirTrimmed.Groups[1].Value;

			// Get DirectoryInfo for destination dir; create dir if necessary
			DirectoryInfo destDir = null;
			try {
				if (options.IsDryRun) {
					addMsg(VerbosityLevel.Verbose, "Would ensure that destination dir existed: " + destDirPath);
					destDir = new DirectoryInfo(destDirPath);
				}
				else {
					destDir = Directory.CreateDirectory(destDirPath);
				}
			}
			catch (Exception ex) {
				addMsg(VerbosityLevel.Error, "Problem creating base backup directory: " + unwrapExceptionMessages(ex));
				throw new StopBackupException();
			}

			// 2. Synchronize this directory...
			List<FileSystemInfo> sourceDirInList = new List<FileSystemInfo>();
			sourceDirInList.Add(sourceDir);

			try {
				List<FileSystemInfo> syncedDirInList = synchronizeObjs(sourceDirInList, destDir, false, true);
				// We know the synchronized dest dir is the first entry in the list as it's
				// the only object we passed to be synchronized!
				if (syncedDirInList.Count == 0) {
					throw new SynchronizeDirException("Unable to synchronize dir: " + sourceDir.FullName);
				}
				DirectoryInfo syncedDir = ((DirectoryInfo)syncedDirInList[0]);
				syncedDir = slashTerm(syncedDir);

				if (!(
					(sourceDir.Attributes & FileAttributes.ReparsePoint) > 0 &&
					JP.IsJunctionPoint(sourceDir.FullName)
				))
				{
					// 3. Not a junction point, so synchronize source directory's child nodes (files AND dirs, but
					//    no need to create dirs because we'll do that later when we traverse them as parent dirs via
					//    this traverseDir method)
					List<DirectoryInfo> childDirs = synchronizeDir(sourceDir, syncedDir, true);
					syncedDir = null;

					// Now synchronize source directory's child dirs recursively...
					foreach (DirectoryInfo childDir in childDirs) {
						traverseDir(childDir, baseDestDir);
					}

					// 4. Finally, set this directory's attributes and datetimes correctly; we
					// didn't do this at the beginning, as the modified datetime was going to
					// change when we synchronized its contents; therefore, it must be done last.
					if (!options.IsDryRun) {
						synchronizeObjs(sourceDirInList, destDir, true, true);
					}
				}
			}
			catch (SynchronizeObjsException ex) {
				// Oh dear... just output error and move on.
				addMsg(VerbosityLevel.Error, unwrapExceptionMessages(ex));
			}
			catch (SynchronizeDirException ex) {
				// Oh dear... just output error and move on.
				addMsg(VerbosityLevel.Error, unwrapExceptionMessages(ex));
			}
			catch (SlashTerminateException ex) {
				// Oh dear... just output error and move on.
				addMsg(VerbosityLevel.Error, unwrapExceptionMessages(ex));
			}
		}

		private DirectoryInfo slashTerm(DirectoryInfo inputDir) {
			// Take the input directory's DirectoryInfo, and return one with the same
			// path, but whose .FullName is guaranteed to be terminated with a slash.

			if ((inputDir.FullName.Substring(inputDir.FullName.Length-1) != "\\") && (inputDir.FullName.Substring(inputDir.FullName.Length-1) != "/")) {
				try {
					return new DirectoryInfo(inputDir.FullName + "\\");
				}
				catch (PathTooLongException ex) {
					throw new SlashTerminateException("Error slash-terminating path '" + inputDir.FullName + "': " + ex.Message);
				}
			}
			else { return inputDir; }
		}

		/// <summary>
		/// Synchronizes the specified FileSystemInfo objects into the specified destination directory.
		/// This ensures that all source FileSystemInfo objects passed will exist in the destination directory.
		/// It DOES NOT delete entries in the destination dir that have not been specified in the source
		/// FileSystemInfo objects list; that must be done elsewhere.
		/// </summary>
		/// <param name="sourceObjs">The list of source FileSystemInfo objects to be synchronized to the destination dir.</param>
		/// <param name="destDir">The destination dir DirectoryInfo object.</param>
		/// <param name="syncDirAttributes">Specifies whether the destination directories that are being synchronized should have their attributes set to match those of the corresponding source directories.</param>
		/// <param name="outputDryRunDirCreation">Specifies whether, during a dry run, we should output a message when we would create a directory.</param>
		/// <param name="dontCreateJunctionPoints">If true, doesn't create new junction points.</param>
		/// <param name="dontCreateDirs">If true, doesn't create new directories.</param>
		/// <returns>A list of FileSystemInfo objects which are descriptors of the DESTINATION DIR objects that have been synchronized (ie. they'll have the .FullName set to the DESTINATION DIR's path to that FileSystemInfo object).</returns>
		private List<FileSystemInfo> synchronizeObjs(List<FileSystemInfo> sourceObjs, DirectoryInfo destDir, bool syncDirAttributes, bool outputDryRunDirCreation, bool dontCreateJunctionPoints = false, bool dontCreateDirs = false) {
			// Synchronize objects in source objects list into given destination dir
			// Returns a FileSystemInfo list containing info on each object (file or
			// directory) that was synchronized.

			List<FileSystemInfo> retVal = new List<FileSystemInfo>();

			if (stopBackup) {
				throw new StopBackupException();
			}

			// Make array list of dest dir's objects; they seem to be in
			// alphabetical order already...
			FileInfo[] destFilesTemp = new FileInfo[0];
			DirectoryInfo[] destDirsTemp = new DirectoryInfo[0];

			try {
				destFilesTemp = destDir.GetFiles();
				destDirsTemp = destDir.GetDirectories();
			}
			catch (Exception ex) {
				if (!options.IsDryRun) {
					throw new SynchronizeObjsException("Couldn't get file or directory list - " + unwrapExceptionMessages(ex));
				}
			}

			List<FileSystemInfo> destObjs = new List<FileSystemInfo>();
			destObjs.AddRange(destFilesTemp);
			destObjs.AddRange(destDirsTemp);

			List<FileSystemInfo> newDestObjs = new List<FileSystemInfo>();

			// First, sync objects that already exist in the destination dir and that
			// are specified in the source objects list.
			foreach (FileSystemInfo destObj in destObjs) {
				try { currentlyProcessing = destObj.FullName; }
				catch (Exception) { }
				if (stopBackup) {
					throw new StopBackupException();
				}

				bool objectsIdentical;
				int foundIndex;
				// If destination object has the same name as an object in the
				// source objects list, synchronize it
				if ((foundIndex = indexNameXinY(destObj, sourceObjs)) >= 0) {
					// Synchronize

					// TODO: refactor this code so that we're caching .Attributes first, and using that cached
					// version; see, sometimes we can't get the file's attributes even if it's in the dir
					// listing (symlinks on a Samba share when symlinks aren't being followed by Samba), and in
					// that case we want to output an error and move on, not abort the whole backup.
					objectsIdentical = true;
					bool destObjIsJunctionPoint = destObj is DirectoryInfo && isReparsePoint((DirectoryInfo)destObj) && JP.IsJunctionPoint(((DirectoryInfo)destObj).FullName);
					if (
						(destObj.Attributes != sourceObjs[foundIndex].Attributes && !isSymlinkToRealDir(sourceObjs[foundIndex], destObj)) &&
						!(destObj is DirectoryInfo && sourceObjs[foundIndex] is DirectoryInfo && notReparsePoint((DirectoryInfo)destObj) && notReparsePoint((DirectoryInfo)sourceObjs[foundIndex]))
					) {
						// Attributes (including whether the object is a directory or reparse point)
						// different.  If they're both directories we can leave the dest dir object
						// and simply change its attributes later.  Otherwise, we have to delete the
						// dest dir object and copy it across later...
						if (!(destObjIsJunctionPoint && dontCreateJunctionPoints) && !(!destObjIsJunctionPoint && destObj is DirectoryInfo && dontCreateDirs)) {
							if (!options.IsDryRun) {
								integrityCheckSourceFile(sourceObjs[foundIndex], destDir);
								addMsg(VerbosityLevel.Verbose, "Deleting " + (destObj is FileInfo ? "file " : destObjIsJunctionPoint ? "junction point " : "dir ") + destObj.FullName + " - attributes or type different from that in source dir.");
								forciblyKillObject(destObj);
							}
							else {
								addMsg(VerbosityLevel.Info, "Would delete " + (destObj is FileInfo ? "file " : destObjIsJunctionPoint ? "junction point " : "dir ") + destObj.FullName + " - attributes or type different from that in source dir.");
							}
						}
						objectsIdentical = false;
					}
					else if (
						(destObj is FileInfo || destObjIsJunctionPoint) &&
						(
							sourceObjs[foundIndex].CreationTimeUtc != destObj.CreationTimeUtc ||
							sourceObjs[foundIndex].LastWriteTimeUtc != destObj.LastWriteTimeUtc
						)
					) {
						// Delete dest file/JP - last creation or write time different.
						// If dest object is a normal directory, we don't need to do this as
						// we can simply change its created/modified times later.

						if (!(destObjIsJunctionPoint && dontCreateJunctionPoints)) {
							if (!options.IsDryRun) {
								integrityCheckSourceFile(sourceObjs[foundIndex], destDir);
								addMsg(VerbosityLevel.Verbose, "Deleting " + (destObjIsJunctionPoint ? "junction point " : "file ") + destObj.FullName + " - last creation or write time different from that in source dir.");
								forciblyKillObject(destObj);
							}
							else {
								addMsg(VerbosityLevel.Info, "Would delete " + (destObjIsJunctionPoint ? "junction point " : "file ") + destObj.FullName + " - last creation or write time different from that in source dir.");
							}
						}
						objectsIdentical = false;
					}
					else if (
						destObj is FileInfo &&
						// It's a file, and we know that the types of the dest and src
						// objs are the same.  Check their lengths to make sure they match.
						( ((FileInfo)destObj).Length != ((FileInfo)sourceObjs[foundIndex]).Length )
					) {
						// Delete dest file - sizes differ
						if (!options.IsDryRun) {
							integrityCheckSourceFile(sourceObjs[foundIndex], destDir);
							addMsg(VerbosityLevel.Verbose, "Deleting file " + destObj.FullName + " - size of file different from that of file in source dir.");
							forciblyKillObject(destObj);
						}
						else {
							addMsg(VerbosityLevel.Info, "Would delete file " + destObj.FullName + " - size of file different from that of file in source dir.");
						}
						objectsIdentical = false;
					}
					else if (
						destObjIsJunctionPoint &&
						JP.IsJunctionPoint(((DirectoryInfo)sourceObjs[foundIndex]).FullName) &&
						JP.GetTarget(((DirectoryInfo)destObj).FullName) != JP.GetTarget(((DirectoryInfo)sourceObjs[foundIndex]).FullName)
					) {
						// Delete dest JP - source and destination objects are junction points but their targets differ
						if (!(destObjIsJunctionPoint && dontCreateJunctionPoints)) {
							if (!options.IsDryRun) {
								addMsg(VerbosityLevel.Verbose, "Deleting junction point " + destObj.FullName + " - targets differ.");
								forciblyKillObject(destObj);
							}
							else {
								addMsg(VerbosityLevel.Info, "Would delete junction point " + destObj.FullName + " - targets differ.");
							}
						}
						objectsIdentical = false;
					}

					try {
						// We may need to change attributes and/or datetimes to make
						// dirs with the same name identical.
						if (
							destObj is DirectoryInfo &&
							sourceObjs[foundIndex] is DirectoryInfo &&
							(notReparsePoint((DirectoryInfo)destObj)) &&
							(notReparsePoint((DirectoryInfo)sourceObjs[foundIndex])) &&
							syncDirAttributes
						) {
							if (destObj.Attributes != sourceObjs[foundIndex].Attributes) {
								if (!options.IsDryRun) {
									addMsg(VerbosityLevel.Verbose, "Setting attributes for directory " + destObj.FullName + " - dest dir attributes different from source dir attributes.");
									destObj.Attributes = sourceObjs[foundIndex].Attributes;
								}
								else {
									addMsg(VerbosityLevel.Info, "Would set attributes for directory " + destObj.FullName + " - dest dir attributes different from source dir attributes.");
								}
							}
							if (
								destObj.CreationTimeUtc != sourceObjs[foundIndex].CreationTimeUtc ||
								destObj.LastWriteTimeUtc != sourceObjs[foundIndex].LastWriteTimeUtc
							) {
								if (!options.IsDryRun) {
									addMsg(VerbosityLevel.Verbose, "Setting created/modified datetimes for directory " + destObj.FullName + " - dest dir datetimes different from source dir datetimes.");
									destObj.CreationTimeUtc = sourceObjs[foundIndex].CreationTimeUtc;
									destObj.LastWriteTimeUtc = sourceObjs[foundIndex].LastWriteTimeUtc;
								}
								else {
									addMsg(VerbosityLevel.Debug, "Would set created/modified datetimes for directory " + destObj.FullName + " - dest dir datetimes different from source dir datetimes.");
								}
							}
						}
					}
					catch (Exception ex) {
						addMsg(VerbosityLevel.Error, "Problem setting dest dir attributes: " + unwrapExceptionMessages(ex));
					}

					if (objectsIdentical) {
						// Objects are identical according to all the above tests; we didn't
						// delete the dest object, so record its continued existance in
						// our new list.
						newDestObjs.Add(destObj);

						// Add to list of objects synchronized
						retVal.Add(destObj);
					}
				}
			}

			destObjs = newDestObjs;

			// Now, copy over source objects that need to be copied.
			foreach (FileSystemInfo obj in sourceObjs) {
				try { currentlyProcessing = obj.FullName; }
				catch (Exception) { }
				if (stopBackup) {
					throw new StopBackupException();
				}

				int foundIndex;
				string copyToPath = "???";
				string createPath = "???";
				// FILE
				if (obj is FileInfo) {
					if ((foundIndex = indexNameXinY(obj, destObjs)) < 0) {
						// Need to copy
						fileCopyCount++;
						try {
							copyToPath = destDir.FullName + obj.Name;
							if (!options.IsDryRun) {
								// The 'Copying' verbose message tends to result in ENORMOUS
								// output when dealing with large directories.  This warrants
								// a warning in the GUI somewhere around the 'display
								// verbose messages' checkbox.
								addMsg(VerbosityLevel.Verbose, "Copying " + obj.FullName + " to " + copyToPath);
								((FileInfo)obj).CopyTo(copyToPath);
								FileInfo justCopied = new FileInfo(copyToPath);
								justCopied.Attributes &= ~FileAttributes.ReadOnly;
								justCopied.CreationTimeUtc = obj.CreationTimeUtc;
								justCopied.LastWriteTimeUtc = obj.LastWriteTimeUtc;
								justCopied.Attributes = obj.Attributes;

								// Add to list of objects synchronized
								retVal.Add(justCopied);
							}
							else {
								addMsg(VerbosityLevel.Info, "Would copy " + obj.FullName + " to " + copyToPath);
								retVal.Add(new FileInfo(copyToPath));
							}
						}
						catch (Exception ex) {
							if (!options.IsDryRun) {
								addMsg(VerbosityLevel.Error, "Couldn't copy file " + copyToPath + " - " + unwrapExceptionMessages(ex));
							}
							else {
								addMsg(VerbosityLevel.Error, "Wouldn't be able to copy file " + copyToPath + " - " + unwrapExceptionMessages(ex));
							}
						}
					}
				}
				// DIR
				else if (obj is DirectoryInfo) {
					if ((foundIndex = indexNameXinY(obj, destObjs)) < 0) {
						// Need to copy
						dirCopyCount++;
						bool isJunctionPoint = false;
						try {
							createPath = destDir.FullName + obj.Name + "\\";
							isJunctionPoint = isReparsePoint((DirectoryInfo)obj) && JP.IsJunctionPoint(((DirectoryInfo)obj).FullName);
							if (!(isJunctionPoint && dontCreateJunctionPoints) && !(!isJunctionPoint && dontCreateDirs)) {
								if (!options.IsDryRun) {
									addMsg(VerbosityLevel.Verbose, "Creating " + (isJunctionPoint ? "junction point " : "directory ") + createPath);
									if (isJunctionPoint) { JP.Create(createPath, JP.GetTarget(((DirectoryInfo)obj).FullName), false); }
									else { Directory.CreateDirectory(createPath); }
									DirectoryInfo justCreated = new DirectoryInfo(createPath);
									if (isJunctionPoint) {
										JP.SetCreateModifyTime(createPath, obj.CreationTimeUtc, obj.LastWriteTimeUtc);
									}
									else {
										justCreated.CreationTimeUtc = obj.CreationTimeUtc;
										justCreated.LastWriteTimeUtc = obj.LastWriteTimeUtc;
									}
									justCreated.Attributes = obj.Attributes;

									// Add to list of objects synchronized
									retVal.Add(justCreated);
								}
								else {
									if (outputDryRunDirCreation) {
										addMsg(VerbosityLevel.Info, "Would create " + (isJunctionPoint ? "junction point " : "directory ") + createPath);
									}
									retVal.Add(new DirectoryInfo(createPath));
								}
							}
						}
						catch (Exception ex) {
							if (!options.IsDryRun) {
								addMsg(VerbosityLevel.Error, "Couldn't create " + (isJunctionPoint ? "junction point " : "directory ") + createPath + " - " + unwrapExceptionMessages(ex));
							}
							else {
								addMsg(VerbosityLevel.Error, "Wouldn't be able to create " + (isJunctionPoint ? "junction point " : "directory ") + createPath + " - " + unwrapExceptionMessages(ex));
							}
						}
					}
				}
			}

			return retVal;
		}

		/// <summary>
		/// Synchronizes two specified directories by first deleting objects in the destination dir that don't exist in
		/// the source dir (UNLESS we're in incremental backup mode), then synchronizing the two dirs using
		/// synchronizeObjs.
		/// </summary>
		/// <param name="sourceDir">The dir to be synchronized from.</param>
		/// <param name="destDir">The dir to be synchronized to.</param>
		/// <param name="dontCreateDirs">If true, doesn't create new directories.</param>
		/// <returns>A list of DirectoryInfo objects containing the child directories of the given source directory.</returns>
		private List<DirectoryInfo> synchronizeDir(DirectoryInfo sourceDir, DirectoryInfo destDir, bool dontCreateDirs = false) {
			if (stopBackup) {
				throw new StopBackupException();
			}

			// Make array list of source and dest dirs' objects; they seem to be in alphabetical order already...

			List<DirectoryInfo> childDirs = new List<DirectoryInfo>();

			FileInfo[] srcFilesTemp = new FileInfo[0];
			FileInfo[] destFilesTemp = new FileInfo[0];
			DirectoryInfo[] srcDirsTemp = new DirectoryInfo[0];
			DirectoryInfo[] destDirsTemp = new DirectoryInfo[0];

			try {
				srcFilesTemp = sourceDir.GetFiles();
				destFilesTemp = destDir.GetFiles();
			}
			catch (Exception ex) {
				if (!options.IsDryRun) {
					throw new SynchronizeDirException("Couldn't get file list - " + unwrapExceptionMessages(ex));
				}
			}
			try {
				srcDirsTemp = sourceDir.GetDirectories();
				destDirsTemp = destDir.GetDirectories();
			}
			catch (Exception ex) {
				if (!options.IsDryRun) {
					throw new SynchronizeDirException("Couldn't get directory list - " + unwrapExceptionMessages(ex));
				}
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
			// We ONLY want to do this if we're in 'carbon copy' mode, not 'incremental'...
			if (options.Type == CCOTypeOfBackup.CarbonCopy) {
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
				foreach (FileSystemInfo destObj in destObjs) {
					if (stopBackup) {
						throw new StopBackupException();
					}

					// Delete obj if it doesn't exist in source directory ('carbon copy' mode)
					int foundIndex;
					if ((foundIndex = indexNameXinY(destObj, srcObjs)) < 0) {
						// Delete dest object - not found
						bool destObjIsJunctionPoint = destObj is DirectoryInfo && isReparsePoint((DirectoryInfo)destObj) && JP.IsJunctionPoint(((DirectoryInfo)destObj).FullName);
						if (!options.IsDryRun) {
							addMsg(VerbosityLevel.Verbose, "Deleting " + (destObj is FileInfo ? "file " : destObjIsJunctionPoint ? "junction point " : "dir ") + destObj.FullName + " - not found in source dir.");
							forciblyKillObject(destObj);
						}
						else {
							addMsg(VerbosityLevel.Info, "Would delete " + (destObj is FileInfo ? "file " : destObjIsJunctionPoint ? "junction point " : "dir ") + destObj.FullName + " - not found in source dir.");
						}
					}
					else {
						// Objects are identical according to all the above tests; don't
						// delete the dest object, and record its continued existance in
						// our new list.
						newDestObjs.Add(destObj);
					}
				}

				destObjs = newDestObjs;
			}

			// Invalid destination objects have been deleted.  Now synchronize source objects, including
			// synchronization of directory attributes.  Because any child directories in this list will later
			// be synchronized again through synchronizeObjs, as parent directories, we don't output 'dry run'
			// info here that we're creating them, because the info would be output twice (once here, and once
			// when the directory is later synchronized as a parent directory).  Nor do we create junction
			// points, which will be created later when traversed as parent directories (they're DirectoryInfo
			// objects even though they're semantically more like files).
			synchronizeObjs(srcObjs, destDir, true, false, true, dontCreateDirs);

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
				if (YObjs[index].Name == XObj.Name) {
					return index;
				}
			}

			return -1;
		}
		private int indexNameXinY(FileInfo XObj, List<FileInfo> YObjs) {
			for (int index=0; index < YObjs.Count; index++) {
				if (YObjs[index].Name == XObj.Name) {
					return index;
				}
			}

			return -1;
		}
		private int indexNameXinY(FileInfo XObj, List<DirectoryInfo> YObjs) {
			for (int index=0; index < YObjs.Count; index++) {
				if (YObjs[index].Name == XObj.Name) {
					return index;
				}
			}

			return -1;
		}
		private int indexNameXinY(DirectoryInfo XObj, List<FileInfo> YObjs) {
			for (int index=0; index < YObjs.Count; index++) {
				if (YObjs[index].Name == XObj.Name) {
					return index;
				}
			}

			return -1;
		}
		private int indexNameXinY(DirectoryInfo XObj, List<DirectoryInfo> YObjs) {
			for (int index=0; index < YObjs.Count; index++) {
				if (YObjs[index].Name == XObj.Name) {
					return index;
				}
			}

			return -1;
		}

		private void integrityCheckSourceFile(FileSystemInfo sourceObj, DirectoryInfo destDir) {
			string checkFailedMessage = "INTEGRITY CHECK FOR SOURCE FILE FAILED, WILL NOT DELETE DESTINATION FILE!";
			try {
				if (sourceObj is FileInfo fi) {
					using (FileStream fs = fi.OpenRead()) {
						try {
							int bufferSize = 65536; // 64KB read buffer
							int bytesRead;
							byte[] buffer = new byte[bufferSize];
							addMsg(VerbosityLevel.Verbose, $"Integrity check ({fi.Name}); starting read.");
							while ((bytesRead = fs.Read(buffer, 0, bufferSize)) > 0) {
								if (stopBackup) {
									throw new StopBackupException();
								}
							}
							addMsg(VerbosityLevel.Verbose, $"Integrity check ({fi.Name}); file read OK.");
						}
						catch (Exception ex) {
							if (ex is StopBackupException) { throw; }
							throw new Exception($"{checkFailedMessage}  Source file ({fi.FullName}) is probably CORRUPT; you should investigate urgently, and check the destination backup directory to ensure it's consistent with the source directory (some destination files with uncorrupted source files may have been deleted and need manual re-copying to the backup dir): {destDir.FullName}", ex);
						}
					}
				}
			}
			catch (Exception ex) {
				if (ex is StopBackupException || ex.Message.StartsWith(checkFailedMessage)) { throw; }
			}
		}

		/// <summary>
		/// Check whether this is a file, a dir, or a junction point.  Set the file (or JP) to not readonly, or if it's a dir, set it and its children recursively to not readonly; then, delete it.
		/// </summary>
		/// <param name="obj">File/dir/JP to delete/kill.</param>
		private void forciblyKillObject(FileSystemInfo obj) {
			if (options.IsDryRun) {
				throw new Exception("Engine is in dry run mode but somehow we got to 'forciblyKillObject' - halted.");
			}
			try { currentlyProcessing = obj.FullName; }
			catch (Exception) { }
			if (
				obj is FileInfo ||
				(obj is DirectoryInfo && (((DirectoryInfo)obj).Attributes & FileAttributes.ReparsePoint) > 0 && JP.IsJunctionPoint(((DirectoryInfo)obj).FullName))
			) {
				obj.Attributes &= ~FileAttributes.ReadOnly;
			}
			else { notReadOnly((DirectoryInfo)obj); }

			if (obj is FileInfo) {
				// Delete file
				addMsg(VerbosityLevel.Verbose, $"Deleting file {((FileInfo)obj).Name} ({((FileInfo)obj).FullName})");
				((FileInfo)obj).Delete();
			}
			else {
                // Delete dir (recursively if it's a regular dir, not if it's a reparse point)
                addMsg(VerbosityLevel.Verbose, $"Deleting dir {((DirectoryInfo)obj).Name} ({((DirectoryInfo)obj).FullName})");
				((DirectoryInfo)obj).Delete(notReparsePoint((DirectoryInfo)obj));
			}
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
				if ((dirSubdir.Attributes & FileAttributes.ReparsePoint) > 0) {
					dirSubdir.Attributes &= ~FileAttributes.ReadOnly;
				}
				else {
					notReadOnly(dirSubdir);
				}
			}
		}

		/// <summary>
		/// Checks whether the given directory is not a reparse point (probable NTFS junction point).
		/// </summary>
		/// <param name="di">The directory to check.</param>
		/// <returns>If directory is not a reparse point, true; otherwise false.</returns>
		private bool notReparsePoint(DirectoryInfo di) {
			return (di.Attributes & FileAttributes.ReparsePoint) == 0;
		}

		/// <summary>
		/// Checks whether the given directory is a reparse point (probable NTFS junction point).
		/// </summary>
		/// <param name="di">The directory to check.</param>
		/// <returns>If directory is a reparse point, true; otherwise false.</returns>
		private bool isReparsePoint(DirectoryInfo di) {
			return !notReparsePoint(di);
		}

		/// <summary>
		/// Checks whether the only difference between object attributes is "is reparse point" and the source object
		/// is a symlink (as we backup symlinks to real directories, we want to ignore this specific difference).
		/// </summary>
		/// <param name="sourceObj">The source file system object to check.</param>
		/// <param name="destObj">The destination file system object to check.</param>
		/// <returns>If this is a "symlink to real dir" difference, true; otherwise false.</returns>
		private bool isSymlinkToRealDir(FileSystemInfo sourceObj, FileSystemInfo destObj) {
			return
				sourceObj is DirectoryInfo &&
				destObj is DirectoryInfo &&
				(sourceObj.Attributes & ~FileAttributes.ReparsePoint) == (destObj.Attributes & ~FileAttributes.ReparsePoint) &&
				(sourceObj.Attributes & FileAttributes.ReparsePoint) > 0 &&
				JP.IsSymlink(((DirectoryInfo)sourceObj).FullName);
		}

		/// <summary>
		/// Unwraps all an exception's nested messages and concatenates them in a string.
		/// </summary>
		/// <param name="ex">The exception.</param>
		/// <returns>The concatenated messages.</returns>
		private string unwrapExceptionMessages(Exception ex) {
			StringBuilder sbMessage = new StringBuilder();
			while (ex != null) {
				sbMessage.Append(" ||| " + ex.Message);
				ex = ex.InnerException;
			}

			string strMessage = sbMessage.ToString();
			if (string.IsNullOrEmpty(strMessage) || strMessage.Length < 6) {
				return "(no error message!)";
			}
			return strMessage.Substring(5).TrimEnd();
		}

		/// <summary>
		/// Add a message to the FIFO message queue, then inform the message handling delegate that there's a new message waiting to be displayed.
		/// </summary>
		/// <param name="level">The verbosity level of the message.</param>
		/// <param name="msgText">The message text.</param>
		/// <param name="alwaysDisplay">If true, always display the message no matter what the verbosity level setting.</param>
		/// <param name="logInRecentMessages">If true, logs this message in recent messages buffer.</param>
		private void addMsg(VerbosityLevel level, string msgText, bool alwaysDisplay = false, bool logInRecentMessages = true) {
			if (logInRecentMessages) {
				recentMessages.Add(new MsgData {
					MsgLevel = level,
					MsgText = msgText
				});
				// Cap recent messages count
				if (recentMessages.Count > 20) {
					recentMessages.RemoveRange(0, recentMessages.Count - 20);
				}
			}

			switch (level) {
				case VerbosityLevel.Info:
					messages.Enqueue(new MsgDisplayInfo(CbInfoMsg, msgText, alwaysDisplay));
					break;

				case VerbosityLevel.Error:
					messages.Enqueue(new MsgDisplayInfo(CbErrorMsg, msgText, alwaysDisplay));
					break;

				case VerbosityLevel.Debug:
					messages.Enqueue(new MsgDisplayInfo(CbDebugMsg, msgText, alwaysDisplay));
					break;

				case VerbosityLevel.Verbose:
					messages.Enqueue(new MsgDisplayInfo(CbVerboseMsg, msgText, alwaysDisplay));
					break;
			}

			CbDisplayNextMessage(GetNextMsg);
		}

		private void emitRecentMessages(string initialMsgPrefix = "") {
			addMsg(VerbosityLevel.Info, $"{initialMsgPrefix}Displaying most recent messages before error, maximum verbosity:", logInRecentMessages: false);
			foreach (var msg in recentMessages) {
				addMsg(msg.MsgLevel, msg.MsgText, alwaysDisplay: true, logInRecentMessages: false);
			}
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
		public bool AlwaysDisplay;

		public MsgDisplayInfo(MsgFunctionDelegate msgFunction, string msgText, bool alwaysDisplay) {
			this.MsgFunction = msgFunction;
			this.MsgText = msgText;
			this.AlwaysDisplay = alwaysDisplay;
		}
	}

	public class MsgData {
		public VerbosityLevel MsgLevel { get; set; }
		public string MsgText { get; set; }
	}

	/// <summary>
	/// Delegate for callbacks to message functions - these functions are to deal with the message supplied (in string format) in a way they see fit, and are to be defined by the calling code.
	/// </summary>
	/// <param name="msg">The message to display.</param>
	/// <param name="alwaysDisplay">If true, always display the message no matter what the verbosity level setting.</param>
	public delegate void MsgFunctionDelegate(string msg, bool alwaysDisplay);

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

	public class SlashTerminateException : Exception {
		// Exception in synchronizing source dir's objects to dest dir

		#region Private vars

		private const string defaultMsg = "There was a miscellaneous error slash-terminating the path.";
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

		public SlashTerminateException(): base(defaultMsg) {
			// No further implementation
		}

		public SlashTerminateException(string message): base(message) {
			// No further implementation
		}

		public SlashTerminateException(string message, Exception innerException): base(message, innerException) {
			// No further implementation
		}

		protected SlashTerminateException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context) {
			// No further implementation
		}

		#endregion
	}
	#endregion
}
