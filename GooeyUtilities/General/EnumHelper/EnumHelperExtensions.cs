using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GooeyUtilities.General.EnumHelper {
	public static class EnumHelperExtensions {
		// NOTE: Unfortunately, in C# you're not allowed to specify "where TEnum : Enum", because that would be ideal.  The best that
		// we can do is specify "where TEnum : struct, IConvertible".  This will mean that the extension methods show up for certain
		// value types other than enums, so it's best only to have a "using" statement for the EnumHelper namespace on files where you
		// really need it so as to avoid unnecessary pollution of the Intellisense popups.

		/// <summary>
		/// Returns the name of this enum constant in its enumeration.
		/// </summary>
		/// <typeparam name="TEnum">The type of the enumeration that this enum instance belongs to.</typeparam>
		/// <param name="thisEnum">The enum instance to get the name for.</param>
		/// <returns>The name of the this enum constant.</returns>
		public static string GetEnumName<TEnum>(this TEnum thisEnum) where TEnum : struct, IConvertible {
			return EnumHelper<TEnum>.GetEnumNameFromEnum(thisEnum);
		}

		/// <summary>
		/// Returns an object which is an instance of the underlying type of this enum and whose value is that defined for this enum constant.
		/// </summary>
		/// <typeparam name="TEnum">The type of the enumeration that this enum instance belongs to.</typeparam>
		/// <param name="thisEnum">The enum instance to get the value for.</param>
		/// <returns>An object which is an instance of the underlying type of this enum and whose value is that defined for this enum constant.</returns>
		public static object GetEnumValue<TEnum>(this TEnum thisEnum) where TEnum : struct, IConvertible {
			return EnumHelper<TEnum>.GetEnumValueFromEnum(thisEnum);
		}
	}
}
