using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CarbonCopy
{
	public static class Utils
	{
		public static Stream GetEmbeddedImageStream(string imgName) {
			Assembly asm = Assembly.GetExecutingAssembly();
			return asm.GetManifestResourceStream("CarbonCopy.Resources." + imgName);
		}
	}
}
