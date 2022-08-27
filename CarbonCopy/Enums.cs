using System;
using System.Collections.Generic;
using System.Text;

namespace CarbonCopy {
	/// <summary>
	/// Enumeration indicating what type of backup to perform.
	/// </summary>
	public enum CCOTypeOfBackup : int {
		None         = 0,
		CarbonCopy   = 1,
		Incremental  = 2,
	}

	/// <summary>
	/// Enumeration of items indicating how detailed the backup's GUI output should be
	/// </summary>
	public enum VerbosityLevel : int {
		Info     = 0,
		Error    = 1,
		Debug    = 2,
		Verbose  = 3,
	}
}
