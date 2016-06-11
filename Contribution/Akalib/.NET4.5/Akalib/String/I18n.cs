using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalib.String
{
	public class I18n
	{
		private bool bBOM;

		public bool IsJis(byte[] bytes)        // Check for JIS (ISO-2022-JP)
		{
			int len = bytes.Length;
			byte b1, b2, b3, b4, b5, b6;

			for (int i = 0; i < len; i++)
			{
				b1 = bytes[i];

				if (b1 > 0x7F)
				{
					return false;   // ISO-2022-JP (0x00～0x7F) の 範囲外
				}
				else if (i < len - 2)
				{
					b2 = bytes[i + 1]; b3 = bytes[i + 2];

					if (b1 == 0x1B && b2 == 0x28 && b3 == 0x42)
						return true;    // ESC ( B  : JIS ASCII

					else if (b1 == 0x1B && b2 == 0x28 && b3 == 0x4A)
						return true;    // ESC ( J  : JIS X 0201-1976 Roman Set

					else if (b1 == 0x1B && b2 == 0x28 && b3 == 0x49)
						return true;    // ESC ( I  : JIS X 0201-1976 片仮名

					else if (b1 == 0x1B && b2 == 0x24 && b3 == 0x40)
						return true;    // ESC $ @  : JIS X 0208-1978(通称：旧JIS)

					else if (b1 == 0x1B && b2 == 0x24 && b3 == 0x42)
						return true;    // ESC $ B  : JIS X 0208-1983(通称：新JIS)
				}
				else if (i < len - 3)
				{
					b2 = bytes[i + 1]; b3 = bytes[i + 2]; b4 = bytes[i + 3];

					if (b1 == 0x1B && b2 == 0x24 && b3 == 0x28 && b4 == 0x44)
						return true;    // ESC $ ( D  : JIS X 0212-1990（JIS補助漢字）
				}
				else if (i < len - 5)
				{
					b2 = bytes[i + 1]; b3 = bytes[i + 2];
					b4 = bytes[i + 3]; b5 = bytes[i + 4]; b6 = bytes[i + 5];

					if (b1 == 0x1B && b2 == 0x26 && b3 == 0x40 &&
						 b4 == 0x1B && b5 == 0x24 && b6 == 0x42)
					{
						return true;    // ESC & @ ESC $ B  : JIS X 0208-1990
					}
				}
			}

			return false;
		}

		public bool IsAscii(byte[] bytes)      // Check for Ascii
		{
			int len = bytes.Length;

			for (int i = 0; i < len; i++)
			{
				if (bytes[i] <= 0x7F)
				{
					// ASCII : 0x00～0x7F
					;
				}
				else
				{
					return false;
				}
			}

			return true;
		}

		public bool IsShiftJis(byte[] bytes)   // Check for Shift-JIS
		{
			int len = bytes.Length;
			byte b1, b2;

			for (int i = 0; i < len; i++)
			{
				b1 = bytes[i];

				if ((b1 <= 0x7F) || (b1 >= 0xA1 && b1 <= 0xDF))
				{
					// ASCII        : 0x00～0x7F
					// 半角カタカナ : 0xA1～0xDF
					;
				}
				else if (i < len - 1)
				{
					b2 = bytes[i + 1];

					if (
						((b1 >= 0x81 && b1 <= 0x9F) || (b1 >= 0xE0 && b1 <= 0xFC)) &&
						((b2 >= 0x40 && b2 <= 0x7E) || (b2 >= 0x80 && b2 <= 0xFC))
						)
					{
						// 漢字 : 第1バイト: 0x81～0x9F、0xE0～0xFC
						//        第2バイト: 0x40～0x7E、0x80～0xFC
						i++;
					}
					else
					{
						return false;
					}
				}
			}

			return true;
		}

		public bool IsEUC(byte[] bytes)        // Check for euc-jp 
		{
			int len = bytes.Length;
			byte b1, b2, b3;

			for (int i = 0; i < len; i++)
			{
				b1 = bytes[i];

				if (b1 <= 0x7F)
				{   //  ASCII : 0x00～0x7F
					;
				}
				else if (i < len - 1)
				{
					b2 = bytes[i + 1];

					if ((b1 >= 0xA1 && b1 <= 0xFE) && (b2 >= 0xA1 && b2 <= 0xFE))
					{ // 漢字 : 第1バイト・第2バイトとも0xA1～0xFE
						i++;
					}
					else if ((b1 == 0x8E) && (b2 >= 0xA1 && b2 <= 0xDF))
					{ // 半角カタカナ : 第1バイト 0x8E, 第2バイト 0xA1～0xDF
						i++;
					}
					else if (i < len - 2)
					{
						b3 = bytes[i + 2];

						if ((b1 == 0x8F) &&
							(b2 >= 0xA1 && b2 <= 0xFE) && (b3 >= 0xA1 && b3 <= 0xFE))
						{ // 補助漢字 : 第1バイト 0x8F, 第2バイト・第3バイトとも 0xA1～0xFE
							i += 2;
						}
						else
						{
							return false;
						}
					}
					else
					{
						return false;
					}
				}
			}

			return true;
		}

		public bool IsUTF8(byte[] bytes)       // Check for UTF-8
		{
			int len = bytes.Length;
			byte b1, b2, b3, b4;

			for (int i = 0; i < len; i++)
			{
				b1 = bytes[i];

				if (b1 <= 0x7F)
				{ //  ASCII : 0x00～0x7F
					;
				}
				else if (i < len - 1)
				{
					b2 = bytes[i + 1];

					if ((b1 >= 0xC0 && b1 <= 0xDF) &&
						(b2 >= 0x80 && b2 <= 0xBF))
					{ // 2 バイト 文字
						i += 1;
					}
					else if (i < len - 2)
					{
						b3 = bytes[i + 2];

						if (b1 == 0xEF && b2 == 0xBB && b3 == 0xBF)
						{ // BOM : 0xEF 0xBB 0xBF
							bBOM = true;
							i += 2;
						}
						else if ((b1 >= 0xE0 && b1 <= 0xEF) &&
							(b2 >= 0x80 && b2 <= 0xBF) &&
							(b3 >= 0x80 && b3 <= 0xBF))
						{ // 3 バイト 文字
							i += 2;
						}

						else if (i < len - 3)
						{
							b4 = bytes[i + 3];

							if ((b1 >= 0xF0 && b1 <= 0xF7) &&
								(b2 >= 0x80 && b2 <= 0xBF) &&
								(b3 >= 0x80 && b3 <= 0xBF) &&
								(b4 >= 0x80 && b4 <= 0xBF))
							{ // 4 バイト 文字
								i += 3;
							}
						}
						else
						{
							return false;
						}
					}
				}
			}

			return true;
		}
	}
}
