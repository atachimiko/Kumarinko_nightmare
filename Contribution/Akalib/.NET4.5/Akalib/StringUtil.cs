using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using Akalib.String;

namespace Akalib
{
	public static class StringUtil
	{
		/// <summary>
		/// 文字コードの自動判定
		/// </summary>
		/// <param name="bytes">判定したいバイト列</param>
		/// <returns>文字コード</returns>
		public static Encoding GetCode(byte[] bytes)
		{
			var i18n = new I18n();
			if (i18n.IsJis(bytes) == true)
				return Encoding.GetEncoding(50220);
			else if (i18n.IsAscii(bytes) == true)
				return Encoding.ASCII;
			else if (i18n.IsShiftJis(bytes) == true)
				return Encoding.GetEncoding(932);
			else if (i18n.IsEUC(bytes) == true)
				return Encoding.GetEncoding(51932);
			else if (i18n.IsUTF8(bytes) == true)
				return Encoding.UTF8;
			else
				return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		public static string ToUpperFirst(string target)
		{
			return char.ToUpper(target[0]) + target.Substring(1);
		}

		/// <summary>
		/// バイト配列から16進数の文字列を生成します。
		/// </summary>
		/// <param name="bytes">バイト配列</param>
		/// <returns>16進数文字列</returns>
		public static string ToHexString(byte[] bytes)
		{
			StringBuilder sb = new StringBuilder(bytes.Length * 2);
			foreach (byte b in bytes)
			{
				if (b < 16) sb.Append('0'); // 二桁になるよう0を追加
				sb.Append(Convert.ToString(b, 16));
			}
			return sb.ToString();
		}

		/// <summary>
		/// 連続したスペースを1つのスペースに置き換えた文字列を取得します。
		/// </summary>
		/// <param name="str">処理を行いたい文字列</param>
		/// <returns></returns>
		public static string SingleSpace(string str)
		{
			//replaces multiple spaces with one.
			RegexOptions options = RegexOptions.None;
			Regex regex = new Regex(@"[ ]{2,}", options);
			str = regex.Replace(str, @" ");
			return str;
		}

		/// <summary>
		/// 「/」をバックスラッシュ(JIS環境では「\」)に置き換えた文字列を取得します。
		/// </summary>
		/// <param name="source">処理を行いたい文字列</param>
		/// <param name="direction">バックスラッシュに置き換えたい場合は「0」</param>
		/// <returns></returns>
		public static string SlashFlip(string source, int direction)
		{
			string returnVal = null;
			if (direction == 0) //back
			{
				returnVal = source.Replace(@"/", @"\");
			}
			else if (direction == 1) //forward
			{
				returnVal = source.Replace(@"\", @"/");
			}
			return returnVal;
		}





		/// <summary>
		/// 先頭・末尾にあるスラッシュ・バックスラッシュを取り除いた文字列を取得します。
		/// </summary>
		/// <param name="str">処理を行いたい文字列</param>
		/// <returns></returns>
		public static string SlashTrim(string str)
		{
			if (str.IndexOf(@"/") == 0 || str.IndexOf(@"\") == 0) //first char
			{
				str = str.Substring(1, str.Length - 1);
			}
			if (str.IndexOf(@"/") == str.Length || str.IndexOf(@"\") == str.Length) //last char
			{
				str = str.Substring(str.Length, 1);
			}
			return str;
		}



		/// <summary>
		/// 指定した長さより長い部分を切り捨て、「...」を末尾に追加した文字列を取得します。
		/// </summary>
		/// <param name="s">処理を行いたい文字列</param>
		/// <param name="maximumLength">切り捨て文字列数</param>
		/// <returns></returns>
		public static string Truncate(string s, int maximumLength)
		{
			return Truncate(s, maximumLength, "...");
		}


		/// <summary>
		/// 指定した長さより長い部分を切り捨て、任意の文字を末尾に追加した文字列を取得します。
		/// </summary>
		/// <param name="s">処理を行いたい文字列</param>
		/// <param name="maximumLength">切り捨て文字列数</param>
		/// <param name="suffix">末尾に追加する文字</param>
		/// <returns></returns>
		public static string Truncate(string s, int maximumLength, string suffix)
		{
			if (suffix == null)
				throw new ArgumentNullException("suffix");

			if (maximumLength <= 0)
				throw new ArgumentException("Maximum length must be greater than zero.", "maximumLength");

			int subStringLength = maximumLength - suffix.Length;

			if (subStringLength <= 0)
				throw new ArgumentException("Length of suffix string is greater or equal to maximumLength");

			if (s != null && s.Length > maximumLength)
			{
				string truncatedString = s.Substring(0, subStringLength);
				// incase the last character is a space
				truncatedString = truncatedString.Trim();
				truncatedString += suffix;

				return truncatedString;
			}
			else
			{
				return s;
			}
		}


		/// <summary>
		/// Indents the specified string.
		/// </summary>
		/// <param name="s">The string to indent.</param>
		/// <param name="indentation">The number of characters to indent by.</param>
		/// <param name="indentChar">The indent character.</param>
		/// <returns></returns>
		public static string Indent(string s, int indentation, char indentChar)
		{
			if (s == null)
				throw new ArgumentNullException("s");

			if (indentation <= 0)
				throw new ArgumentException("Must be greater than zero.", "indentation");

			StringReader sr = new StringReader(s);
			StringWriter sw = new StringWriter(CultureInfo.InvariantCulture);

			ActionTextReaderLine(sr, sw, delegate(TextWriter tw, string line)
			{
				tw.Write(new string(indentChar, indentation));
				tw.Write(line);
			});

			return sw.ToString();
		}




		/// <summary>
		/// Numbers the lines.
		/// </summary>
		/// <param name="s">The string to number.</param>
		/// <returns></returns>
		public static string NumberLines(string s)
		{
			if (s == null)
				throw new ArgumentNullException("s");

			StringReader sr = new StringReader(s);
			StringWriter sw = new StringWriter(CultureInfo.InvariantCulture);

			int lineNumber = 1;

			ActionTextReaderLine(sr, sw, delegate(TextWriter tw, string line)
			{
				tw.Write(lineNumber.ToString(CultureInfo.InvariantCulture).PadLeft(4));
				tw.Write(". ");
				tw.Write(line);

				lineNumber++;
			});

			return sw.ToString();
		}





		private delegate void ActionLine(TextWriter textWriter, string line);

		/// <summary>
		/// デリゲートを使って行単位での文字列処理を行うためのメソッド
		/// </summary>
		/// <param name="textReader"></param>
		/// <param name="textWriter"></param>
		/// <param name="lineAction">行毎に行いたい処理を実装した関数</param>
		private static void ActionTextReaderLine(TextReader textReader, TextWriter textWriter, ActionLine lineAction)
		{
			string line;
			bool firstLine = true;
			while ((line = textReader.ReadLine()) != null)
			{
				if (!firstLine)
					textWriter.WriteLine();
				else
					firstLine = false;

				lineAction(textWriter, line);
			}
		}
	}
}
