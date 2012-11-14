using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GooeyUtilities.General {
	public static class StringExtensions {
		/// <summary>
		/// Formats a string using the values of the passed params, in the same way as string.Format does.
		/// </summary>
		/// <param name="format">The string to format.</param>
		/// <param name="values">The params whose values to use in the formatted string.</param>
		/// <returns>The formatted string.</returns>
		public static string FormatWith(this string format, params object[] values) {
			return string.Format(format, values);
		}
	}
}
