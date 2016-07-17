using Kumano.Core.Constractures;
using Kumano.Core.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kumano.Core
{
	public static class ApplicationContext
	{
		#region フィールド

		static IApplicationContext Instance;

		#endregion フィールド

		#region プロパティ

		/// <summary>
		/// 
		/// </summary>
		public static IApplicationContextEvent Event { get { return (IApplicationContextEvent)Instance; } }

		/// <summary>
		/// アプリケーションのアセンブリ情報
		/// </summary>
		public static System.Diagnostics.FileVersionInfo ApplicationFileVersionInfo { get { return Instance.ApplicationFileVersionInfo; } }

		/// <summary>
		/// アプリケーション設定情報
		/// </summary>
		public static ApplicationSettingInfo ApplicationSetting { get { return Instance.ApplicationSetting; } }

		/// <summary>
		/// コンフィグファイルのパス
		/// </summary>
		public static string ConfigDirectoryPath { get { return Instance.ConfigDirectoryPath; } }

		public static Window MainWindow { get { return Instance.MainWindow; } }

		public static IWorkspaceViewModel Workspace { get { return Instance.Workspace; } }

		#endregion プロパティ


		#region メソッド

		public static void InitializeApplication()
		{
			Instance.InitializeApplication();
		}

		public static void SaveApplicationSettingFile()
		{
			Instance.SaveApplicationSettingFile();
		}

		public static void SetupApplicationContextImpl(IApplicationContext applicationContextImpl)
		{
			ApplicationContext.Instance = applicationContextImpl;
		}

		/// <summary>
		/// 経過時間を含めたログの出力
		/// </summary>
		/// <param name="message"></param>
		public static void Watch(string message)
		{
			Instance.Watch(message);
		}

		#endregion メソッド
	}
}
