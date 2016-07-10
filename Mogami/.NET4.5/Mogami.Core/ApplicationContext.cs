using Mogami.Core.Constructions;
using Mogami.Core.Infrastructure;
using Mogami.Pcm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Core
{
	/// <summary>
	/// アプリケーションコンテキストを呼び出す静的クラス
	/// </summary>
	public static class ApplicationContext
	{


		#region フィールド

		private static IApplicationContext Instance;

		#endregion フィールド

		#region プロパティ

		/// <summary>
		/// アプリケーションコンテキストのApplicationDirectoryPathプロパティの値を取得します。
		/// </summary>
		public static string ApplicationDirectoryPath { get { return Instance.ApplicationDirectoryPath; } }

		/// <summary>
		/// アプリケーションのアセンブリ情報
		/// </summary>
		public static System.Diagnostics.FileVersionInfo ApplicationFileVersionInfo { get { return Instance.ApplicationFileVersionInfo; } }

		/// <summary>
		/// プラグイン管理オブジェクト
		/// </summary>
		public static IPlugInBasedManager PlugInManager { get { return Instance.PlugInManager; } }

		/// <summary>
		/// サムネイルを格納するディレクトリパスを取得します
		/// </summary>
		public static string ThumbnailDirectoryPath { get { return Instance.ThumbnailDirectoryPath; } }

		#endregion プロパティ


		#region メソッド

		public static void Initialize(InitializeParamType param)
		{
			Instance.Initialize(param);
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
