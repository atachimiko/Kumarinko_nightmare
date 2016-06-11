using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Text.RegularExpressions;

namespace Akalib.String
{
	/// <summary>
	/// １６進数表記で記述されたカラーコードをColorオブジェクトに変換します
	/// </summary>
	public class ColorTextUtil
	{

		/// <summary>
		/// Extract only the hex digits from a string.
		/// </summary>
		public static string ExtractHexDigits(string input)
		{
			// remove any characters that are not digits (like #)
			Regex isHexDigit
			   = new Regex("[abcdefABCDEF\\d]+", RegexOptions.Compiled);
			string newnum = "";
			foreach (char c in input)
			{
				if (isHexDigit.IsMatch(c.ToString()))
					newnum += c.ToString();
			}
			return newnum;
		}

		/// <summary>
		/// アルファ値つきRGB
		/// </summary>
		/// <param name="hexColor"></param>
		/// <returns></returns>
		public static Color HexStringToAColor(string hexColor)
		{
			string hc = ExtractHexDigits(hexColor);
			if (hc.Length != 8)
			{
				// you can choose whether to throw an exception
				//throw new ArgumentException("hexColor is not exactly 6 digits.");
				return Colors.Transparent;
			}
			string a = hc.Substring(0, 2);
			string r = hc.Substring(2, 2);
			string g = hc.Substring(4, 2);
			string b = hc.Substring(6, 2);
			Color color;
			try
			{
				int ai
				   = Int32.Parse(a, System.Globalization.NumberStyles.HexNumber);
				int ri
				   = Int32.Parse(r, System.Globalization.NumberStyles.HexNumber);
				int gi
				   = Int32.Parse(g, System.Globalization.NumberStyles.HexNumber);
				int bi
				   = Int32.Parse(b, System.Globalization.NumberStyles.HexNumber);
				color = Color.FromArgb((byte)ai, (byte)ri, (byte)gi, (byte)bi);
			}
			catch
			{
				// you can choose whether to throw an exception
				//throw new ArgumentException("Conversion failed.");
				return Colors.Transparent;
			}
			return color;
		}
		/// <summary>
		/// Convert a hex string to a .NET Color object.
		/// </summary>
		/// <param name="hexColor">a hex string: "FFFFFF", "#000000"</param>
		public static Color HexStringToColor(string hexColor)
		{
			string hc = ExtractHexDigits(hexColor);
			if (hc.Length != 6)
			{
				// you can choose whether to throw an exception
				//throw new ArgumentException("hexColor is not exactly 6 digits.");
				return Colors.Transparent;
			}
			string r = hc.Substring(0, 2);
			string g = hc.Substring(2, 2);
			string b = hc.Substring(4, 2);
			Color color;
			try
			{
				int ri
				   = Int32.Parse(r, System.Globalization.NumberStyles.HexNumber);
				int gi
				   = Int32.Parse(g, System.Globalization.NumberStyles.HexNumber);
				int bi
				   = Int32.Parse(b, System.Globalization.NumberStyles.HexNumber);
				color = Color.FromRgb((byte)ri, (byte)gi, (byte)bi);
			}
			catch
			{
				// you can choose whether to throw an exception
				//throw new ArgumentException("Conversion failed.");
				return Colors.Transparent;
			}
			return color;
		}
	}
}
