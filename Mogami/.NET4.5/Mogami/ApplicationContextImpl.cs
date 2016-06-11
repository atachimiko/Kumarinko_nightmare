using Akalib;
using log4net;
using Mogami.Core.Constructions;
using Mogami.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mogami
{
	public class ApplicationContextImpl : IApplicationContext
	{


		#region フィールド

		private static ApplicationContextImpl instance;

		private static ILog LOG = LogManager.GetLogger(typeof(ApplicationContextImpl));

		/// <summary>
		/// アプリケーションが使用するディレクトリ
		/// </summary>
		private readonly string _ApplicationDirectoryPath;

		/// <summary>
		/// Disposeが実行済みの場合Trueを設定します
		/// </summary>
		private bool _alreadyDisposed = false;

		/// <summary>
		/// .NETアプリケーションのコンテキスト
		/// </summary>
		private Application _Application;

		/// <summary>
		/// アプリケーションの処理計測を行うためのタイマー
		/// </summary>
		private Stopwatch _Stopwatch = new Stopwatch();

		#endregion フィールド

		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="application"></param>
		/// <param name="applicationConfig"></param>
		private ApplicationContextImpl(Application application, IBuildAssemblyParameter applicationConfig)
		{
			this._Stopwatch = new Stopwatch();
			this._Stopwatch.Start();

			_ApplicationDirectoryPath = "";
			_Application = application;

			IBuildAssemblyParameter buildParam;
			if (applicationConfig != null) buildParam = applicationConfig;
			else buildParam = new BuildAssemblyParameter();

			string personalDirectoryPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			_ApplicationDirectoryPath = personalDirectoryPath + buildParam.Params["ApplicationDirectoryPath"];

			AppDomain.CurrentDomain.SetPrincipalPolicy(System.Security.Principal.PrincipalPolicy.WindowsPrincipal);

			this.ApplicationFileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
		}

		#endregion コンストラクタ

		#region プロパティ

		/// <summary>
		/// アプリケーションが使用するディレクトrいを取得します。
		/// </summary>
		public string ApplicationDirectoryPath
		{
			get { return _ApplicationDirectoryPath; }
		}

		/// <summary>
		/// 
		/// </summary>
		public System.Diagnostics.FileVersionInfo ApplicationFileVersionInfo
		{
			get;
			private set;
		}

		/// <summary>
		/// アプリケーション設定ファイルのパスを取得します。
		/// </summary>
		public string ApplicationSettingFilePath
		{
			get { return ConfigDirectoryPath + @"\halcyon.conf"; }
		}

		/// <summary>
		/// アプリケーションの設定情報の保存に使用するディレクトリを取得します。
		/// </summary>
		public string ConfigDirectoryPath
		{
			get { return ApplicationDirectoryPath + @"\config"; }
		}

		/// <summary>
		/// アプリケーションが使用するデータベースファイルを保存するディレクトリを取得します。
		/// </summary>
		public string DatabaseDirectoryPath
		{
			get { return ApplicationDirectoryPath + @"\db"; }
		}

		/// <summary>
		/// アプリケーションが一時ファイルを保存するディレクトリを取得します。
		/// </summary>
		public string TemporaryDirectoryPath
		{
			get { return ApplicationDirectoryPath + @"\temporary"; }
		}

		/// <summary>
		/// アプリケーションが使用するサムネイルを保存するディレクトリを取得します。
		/// </summary>
		public string ThumbnailDirectoryPath
		{
			get { return ApplicationDirectoryPath + @"\thumb"; }
		}

		#endregion プロパティ

		#region メソッド

		/// <summary>
		/// アプリケーションコンテキストクラスを新規作成します。
		/// このメソッドはアプリケーション内で１度だけ呼び出してください。
		/// </summary>
		/// <param name="application">WPFフレームワークのアプリケーションクラス</param>
		/// <returns></returns>
		public static IApplicationContext CreateInstance(Application application, IBuildAssemblyParameter applicationConfig = null)
		{
			if (ApplicationContextImpl.instance != null) throw new ApplicationException("アプリケーションコンテキストはすでにインスタンス化しています");
			ApplicationContextImpl.instance = new ApplicationContextImpl(application, applicationConfig);
			return ApplicationContextImpl.instance;
		}

		public static void Dispose()
		{
			ApplicationContextImpl.instance.Dispose(true);
			GC.SuppressFinalize(ApplicationContextImpl.instance);

			ApplicationContextImpl.instance = null;
		}

		public static IApplicationContext GetInstance()
		{
			return ApplicationContextImpl.instance;
		}
		/// <summary>
		/// アプリケーションの初期化を行います
		/// </summary>
		/// <param name="param">実行する初期化シーケンスの種類</param>
		public void Initialize(InitializeParamType param)
		{
			switch (param)
			{
				case InitializeParamType.DIRECTORY:
					InitializeDirectory();
					break;

				case InitializeParamType.DATABASE:
					InitializeDatabase();
					break;
			}
		}

		/// <summary>
		/// アプリケーション起動処理を実行します
		/// </summary>
		public void InitializeApplication()
		{
			Watch("InitializeApplicationの実行を開始します");

			Watch("テンポラリファイルの削除");
			RemoveTemporaryFiles();
			Initialize(InitializeParamType.DIRECTORY);
			Initialize(InitializeParamType.DATABASE);
		}

		/// <summary>
		/// アプリケーション全体での経過時間をロギングする
		/// </summary>
		/// <param name="message"></param>
		public void Watch(string message)
		{
			LOG.InfoFormat("[{0}] {1}", _Stopwatch.Elapsed, message);
		}

		protected virtual void Dispose(bool isDisposing)
		{
			if (_alreadyDisposed)
				return;

			if (isDisposing)
			{
			}

			_alreadyDisposed = true;
		}

		/// <summary>
		/// データベースに関する初期化処理
		/// </summary>
		private void InitializeDatabase()
		{
		}

		/// <summary>
		/// 必要なディレクトリを作成する初期化処理
		/// </summary>
		private void InitializeDirectory()
		{
			// アプリケーションが使用する各種ディレクトリの作成
			System.IO.Directory.CreateDirectory(ApplicationDirectoryPath);
			System.IO.Directory.CreateDirectory(DatabaseDirectoryPath);
			System.IO.Directory.CreateDirectory(TemporaryDirectoryPath);
			System.IO.Directory.CreateDirectory(ThumbnailDirectoryPath);
			System.IO.Directory.CreateDirectory(ConfigDirectoryPath);
		}

		/// <summary>
		/// テンポラリディレクトリ内のファイルを削除する
		/// </summary>
		private void RemoveTemporaryFiles()
		{
			var dir = new DirectoryInfo(this.TemporaryDirectoryPath);
			if (!dir.Exists) return;

			foreach (var file in dir.GetFiles())
			{
				// ファイルの削除に失敗しても処理は継続
				try
				{
					file.IsReadOnly = false;
					file.Delete();
				}
				catch (Exception)
				{
				}
			}
		}

		#endregion メソッド

	}
}

