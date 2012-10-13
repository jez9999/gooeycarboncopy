using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GooeyUtilities.General.EnumHelper {
	/// <summary>
	/// A class providing help with enumerations.
	/// </summary>
	/// <typeparam name="TEnum">The type of the enumeration being dealt with.</typeparam>
	public static class EnumHelper<TEnum> {
		#region Constructors
		/// <summary>
		/// Checks to make sure that the generic type given to this class is an enumaration.
		/// </summary>
		static EnumHelper() {
			if (!typeof(TEnum).IsEnum) {
				throw new EnumHelperException("Type '" + typeof(TEnum).FullName + "' is not an enumeration.");
			}
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Returns an enumeration of type TEnum whose value is set to the specified value.
		/// Throws an exception if no entry in TEnum corresponds to the specified value.
		/// </summary>
		/// <param name="enumValue">The value that the returned enumeration should be set to.</param>
		/// <returns>An enumeration of type TEnum whose value is set to the specified value.</returns>
		public static TEnum GetEnumFromEnumValue(object enumValue) {
			Type underlyingType = Enum.GetUnderlyingType(typeof(TEnum));
			if (enumValue.GetType() != underlyingType) {
				throw new EnumHelperException("The type of the value passed (" + enumValue.GetType().FullName + ") is different from the underlying type of the enumeration '" + typeof(TEnum).FullName + "' (" + underlyingType.FullName + ").");
			}

			if (Enum.IsDefined(typeof(TEnum), enumValue)) {
				return (TEnum)enumValue;
			}
			else {
				throw new EnumHelperException("Couldn't find enum value '" + enumValue + "' in enum type '" + typeof(TEnum).FullName + "'.");
			}
		}

		/// <summary>
		/// Returns an enumeration of type TEnum whose value is set to the value associated with the specified entry name.
		/// Throws an exception if no entry with the specified name is defined in the TEnum enumeration.
		/// </summary>
		/// <param name="enumName">The name of the entry whose associated value the returned TEnum should be set to.</param>
		/// <returns>An enumeration of type TEnum whose value is set to the value associated with the specified entry name.</returns>
		public static TEnum GetEnumFromEnumName(string enumName) {
			TEnum parseResult;
			try {
				parseResult = (TEnum)Enum.Parse(typeof(TEnum), enumName);
			}
			catch (Exception) {
				throw new EnumHelperException("Couldn't find enum name '" + enumName + "' in enum type '" + typeof(TEnum).FullName + "'.");
			}

			return parseResult;
		}

		/// <summary>
		/// Returns an object which is an instance of the underlying enum type and whose value is that defined for the
		/// specified enum instance.
		/// </summary>
		/// <param name="enumInstance">The instance of the enumeration whose value to get.</param>
		/// <returns>The object which is an instance of the underlying enum type and whose value is that defined for the specified enum instance.</returns>
		public static object GetEnumValueFromEnum(TEnum enumInstance) {
			Type underlyingType = Enum.GetUnderlyingType(typeof(TEnum));
			return Convert.ChangeType(enumInstance, underlyingType);
		}

		/// <summary>
		/// Returns the name of the specified constant in its enumeration.
		/// </summary>
		/// <param name="enumInstance">The instance of the enum constant whose name to get.</param>
		/// <returns>The name of the specified constant.</returns>
		public static string GetEnumNameFromEnum(TEnum enumInstance) {
			return Enum.GetName(typeof(TEnum), GetEnumValueFromEnum(enumInstance));
		}
		#endregion
	}
}
