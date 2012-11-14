using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GooeyUtilities.General {
	public static class CustomAttributeProviderExtensions {
		/// <summary>
		/// Gets a list of all custom attributes of the specified attribute type that have been applied.
		/// This list should therefore never contain more than one item, as that would imply that the same attribute had been applied twice.
		/// A 'custom attribute' is any applied attribute that has been implemented as a "FooAttribute" class, and isn't one of the 'standard'
		/// attributes like the ones you can get from, say, typeof(string).Attributes.
		/// </summary>
		/// <typeparam name="T">The type of the custom attribute(s) to get.</typeparam>
		/// <param name="provider">The object that implements the ICustomAttributeProvider interface.</param>
		/// <returns>The custom attribute(s) found.</returns>
		public static List<T> GetCustomAttributes<T>(this ICustomAttributeProvider provider) where T : Attribute {
			List<T> attrs = new List<T>();

			foreach (object attr in provider.GetCustomAttributes(typeof(T), false)) {
				if (attr is T) {
					attrs.Add(attr as T);
				}
			}

			return attrs;
		}
	}
}
