using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Akalib
{
	public class OsVersion
	{

		public OsVersion()
		{
			string result = ERR;
			OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX();
			OperatingSystem osInfo = Environment.OSVersion;

			osVersionInfo.dwOSVersionInfoSize =
				   Marshal.SizeOf(typeof(OSVERSIONINFOEX));
			if (!GetVersionEx(ref osVersionInfo))
			{
				this.OsVersionText = ERR;
			}
			else
			{

				bool is64 = Is64();

				log.AppendLine("osInfo.Platform=" + osInfo.Platform);
				log.AppendLine("osInfo.Version.Major=" + osInfo.Version.Major);
				log.AppendLine("osInfo.Version.Minor=" + osInfo.Version.Minor);
				log.AppendLine("osVersionInfo.wProductType=" + osVersionInfo.wProductType);
				log.AppendLine("osVersionInfo.wSuiteMask=" + osVersionInfo.wSuiteMask);
				log.AppendLine("IntPtr.Size=" + IntPtr.Size);
				log.AppendLine("64 bit OS=" + Convert.ToString(is64));

				if (osInfo.Platform == PlatformID.Win32NT)
				{
					if (osInfo.Version.Major > 4)
					{
						result = "Microsoft ";
					}

					if (osInfo.Version.Major == 6)
					{
						if (osInfo.Version.Minor == 0)
						{
							if (osVersionInfo.wProductType == VER_NT_WORKSTATION)
							{
								result += "Windows Vista";
							}
							else
							{
								result += "Windows Server 2008";
							}
						}
						else if (osInfo.Version.Minor == 1)
						{
							if (osVersionInfo.wProductType == VER_NT_WORKSTATION)
							{
								result += "Windows 7";
							}
							else
							{
								result += "Windows Server 2008 R2";
							}
						}
						else if (osInfo.Version.Minor == 2)
						{
							if (osVersionInfo.wProductType == VER_NT_WORKSTATION)
							{
								result += "Windows 8";
							}
							else
							{
								result += "Windows Server 2012";
							}
						}
					}
					else if (osInfo.Version.Major == 5)
					{
						if (osInfo.Version.Minor == 2)
						{
							if (GetSystemMetrics(SM_SERVERR2) != 0)
							{
								result += "Windows Server 2003 R2";
							}
							else if ((osVersionInfo.wSuiteMask & VER_SUITE_STORAGE_SERVER) > 0)
							{
								result += "Windows Storage Server 2003";
							}
							else if ((osVersionInfo.wSuiteMask & VER_SUITE_WH_SERVER) > 0)
							{
								result += "Windows Home Server";
							}
							else if (osVersionInfo.wProductType == VER_NT_WORKSTATION && is64)
							{
								result += "Windows XP Professional x64 Edition";
							}
							else
							{
								result += "Windows Server 2003";
							}

							// エディション取得処理は省略
						}
						else if (osInfo.Version.Minor == 1)
						{
							result += "Windows XP";
						}
						else if (osInfo.Version.Minor == 0)
						{
							result += "Windows 2000";
						}
					}

					// Include service pack (if any)
					if (osVersionInfo.szCSDVersion.Length > 0)
					{
						result += " " + osVersionInfo.szCSDVersion;
					}

					// Architecture
					if (is64 == false)
					{
						result += ", 32-bit";
					}
					else
					{
						result += ", 64-bit";
					}
				}
				else if (osInfo.Platform == PlatformID.Win32Windows)
				{
					result = "This sample does not support this version of Windows.";
				}
			}

			this.OsVersionText = result;
		}

		/// <summary>
		/// 稼働環境の詳細を含めた文字列を取得します。
		/// </summary>
		/// <returns></returns>
		public string GetEnvironmentString()
		{
			return log.ToString();
		}

		public string GetLog()
		{
			return log.ToString();
		}

		/// <summary>
		/// OSバージョン名を取得する。
		/// </summary>
		/// <returns>OSバージョン名</returns>
		public string GetOsDisplayString()
		{
			return this.OsVersionText;
		}
		private StringBuilder log = new StringBuilder();
		private string OsVersionText = "";

		private bool Is64()
		{
			bool is64;

			if (IntPtr.Size == 4)
			{
				bool wow64 = false;
				IntPtr address = GetProcAddress(GetModuleHandle("Kernel32.dll"), "IsWow64Process");
				if (address != IntPtr.Zero)
				{
					if (IsWow64Process(Process.GetCurrentProcess().Handle, out wow64) == false)
					{
						wow64 = false;
					}
				}

				if (wow64)
				{
					is64 = true;
				}
				else
				{
					is64 = false;
				}
			}
			else
			{
				is64 = true;
			}

			return is64;
		}

		#region 構造体、定数、DLL定義

		// エラー
		private const string ERR = "取得失敗";

		// GetSystemMetrics
		private const Int32 SM_SERVERR2 = 89;
		private const Int32 VER_NT_DOMAIN_CONTROLLER = 2;
		private const Int32 VER_NT_SERVER = 3;

		// GetVersionEx
		private const Int32 VER_NT_WORKSTATION = 1;
		private const Int32 VER_SUITE_BLADE = 1024;
		private const Int32 VER_SUITE_DATACENTER = 128;
		private const Int32 VER_SUITE_ENTERPRISE = 2;
		private const Int32 VER_SUITE_PERSONAL = 512;
		private const Int32 VER_SUITE_SINGLEUSERTS = 256;
		private const Int32 VER_SUITE_SMALLBUSINESS = 1;
		private const Int32 VER_SUITE_STORAGE_SERVER = 8192; //0x00002000;
		private const Int32 VER_SUITE_TERMINAL = 16;
		private const Int32 VER_SUITE_WH_SERVER = 32768; //0x00008000;

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true)]
		private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		[DllImport("user32.dll")]
		private static extern int GetSystemMetrics(Int32 nIndex);

		[DllImport("kernel32.dll")]
		private static extern bool GetVersionEx(ref OSVERSIONINFOEX osVersionInfo);

		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lpSystemInfo);

		[StructLayout(LayoutKind.Sequential)]
		private struct OSVERSIONINFOEX
		{
			public int dwOSVersionInfoSize;
			public int dwMajorVersion;
			public int dwMinorVersion;
			public int dwBuildNumber;
			public int dwPlatformId;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string szCSDVersion;
			public short wServicePackMajor;
			public short wServicePackMinor;
			public short wSuiteMask;
			public byte wProductType;
			public byte wReserved;
		}

		#endregion

	}
}
