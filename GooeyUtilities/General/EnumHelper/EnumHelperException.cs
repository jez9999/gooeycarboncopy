using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GooeyUtilities.General.EnumHelper {
	public class EnumHelperException : Exception {
		#region Private vars
		private const string defaultMsg = "There was a problem parsing the enumeration.";
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

		public EnumHelperException(): base(defaultMsg) {
			// No further implementation
		}

		public EnumHelperException(string message): base(message) {
			// No further implementation
		}

		public EnumHelperException(string message, Exception innerException): base(message, innerException) {
			// No further implementation
		}

		protected EnumHelperException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context) {
			// No further implementation
		}
		#endregion
	}
}
