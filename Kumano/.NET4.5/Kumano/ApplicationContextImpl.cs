using Kumano.Core.Constractures;
using Kumano.Core.Infrastructures;
using Kumano.Data.ViewModel;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kumano
{
	public class ApplicationContextImpl : IApplicationContext
	{
		#region フィールド

		static ApplicationContextImpl instance;

		// singleton instance.
		static ILog LOG = LogManager.GetLogger(typeof(ApplicationContextImpl));

		/// <summary>
		/// 
		/// </summary>
		readonly string _ApplicationDirectoryPath;

		/// <summary>
		/// Disposeが実行済みの場合Trueを設定します
		/// </summary>
		bool _alreadyDisposed = false;

		/// <summary>
		/// WPFアプリケーションのコンテキスト
		/// </summary>
		Application _Application;

		/// <summary>
		/// 
		/// </summary>
		Window _MainWindow;

		/// <summary>
		/// アプリケーションの処理計測を行うためのタイマー
		/// </summary>
		Stopwatch _Stopwatch = new Stopwatch();

		/// <summary>
		/// 
		/// </summary>
		WorkspaceViewModel _Workspace;

		#endregion フィールド


		#region コンストラクタ

		private ApplicationContextImpl(Application application)
		{
			this._Stopwatch = new Stopwatch();
			this._Stopwatch.Start();

			this.ApplicationSetting = new ApplicationSettingInfo();
			this.ApplicationSetting.Reset();

			_Application = application;

			string personalDirectoryPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
#if DEBUG
			_ApplicationDirectoryPath = personalDirectoryPath + @"\Alcedo_dev";
#else
			_ApplicationDirectoryPath = personalDirectoryPath + @"\Alcedo";
#endif
			AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

			this.ApplicationFileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
		}

		#endregion コンストラクタ


		#region Destructors

		~ApplicationContextImpl()
		{
			// ファイナライザでは、アンマネージドリソースのみを破棄するようにDispose()を呼び出す。
			Dispose(false);
		}

		#endregion Destructors


		#region プロパティ

		public string ApplicationDirectoryPath
		{
			get { return _ApplicationDirectoryPath; }
		}

		public System.Diagnostics.FileVersionInfo ApplicationFileVersionInfo
		{
			get;
			private set;
		}

		public ApplicationSettingInfo ApplicationSetting
		{
			get;
			private set;
		}

		public string ApplicationSettingFilePath
		{
			get { return ConfigDirectoryPath + @"\alcedo.conf"; }
		}

		/// <summary>
		/// コンフィグファイルなどを設置するディレクトリ
		/// </summary>
		public string ConfigDirectoryPath
		{
			get
			{
				return ApplicationDirectoryPath + @"\config";
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Window MainWindow
		{
			get { return _MainWindow; }
			set { _MainWindow = value; }
		}

		/// <summary>
		/// 作業用ディレクトリ
		/// </summary>
		public string TemporaryDirectoryPath
		{
			get
			{
				return ApplicationDirectoryPath + @"\temporary";
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public IWorkspaceViewModel Workspace
		{
			get { return _Workspace; }
			set { _Workspace = (WorkspaceViewModel)value; }
		}

		#endregion プロパティ

		#region メソッド

		/// <summary>
		/// アプリケーションコンテキストクラスを新規作成します。
		/// このメソッドはアプリケーション内で１度だけ呼び出してください。
		/// </summary>
		/// <param name="application">WPFフレームワークのアプリケーションクラス</param>
		/// <returns></returns>
		public static IApplicationContext CreateInstance(Application application)
		{

			if (ApplicationContextImpl.instance != null) throw new ApplicationException("アプリケーションコンテキストはすでにインスタンス化しています");
			ApplicationContextImpl.instance = new ApplicationContextImpl(application);

			return ApplicationContextImpl.instance;
		}

		public static void Dispose()
		{
			ApplicationContextImpl.instance.Dispose(true);
			GC.SuppressFinalize(ApplicationContextImpl.instance);
		}

		public static IApplicationContext GetInstance()
		{
			return ApplicationContextImpl.instance;
		}

		/// <summary>
		/// アプリケーションを終了します
		/// </summary>
		public void ApplicationExit()
		{
			_Application.Shutdown(1);
		}

		/// <summary>
		/// 
		/// </summary>
		public void InitializeApplication()
		{
			Watch("InitializeApplicationの実行を開始します");
			Watch("テンポラリファイルの削除");
			RemoveTemporaryFiles();
		}

		/// <summary>
		/// アプリケーション設定ファイルからアプリケーション設定を読み込む
		/// </summary>
		public void InitializeApplicationFiles()
		{
			// アプリケーションが使用する各種ディレクトリの作成
			System.IO.Directory.CreateDirectory(ApplicationDirectoryPath);
			System.IO.Directory.CreateDirectory(TemporaryDirectoryPath);
			System.IO.Directory.CreateDirectory(ConfigDirectoryPath);

			if (ApplicationSetting == null)
			{
				ApplicationSetting = new ApplicationSettingInfo();
			}

			if (File.Exists(ApplicationSettingFilePath))
			{

				using (StreamReader sr = new StreamReader(ApplicationSettingFilePath, Encoding.GetEncoding("utf-8")))
				{
					ApplicationSetting.Load(sr);
				}
			}
			else
			{
				// ファイルが存在しない場合、デフォルト設定で設定情報を作成し、ファイルに出力する。
				ApplicationSetting.Reset();

				File.Create(ApplicationSettingFilePath).Close();
				using (var sw = new StreamWriter(ApplicationSettingFilePath))
				{
					ApplicationSetting.Save(sw);
				}
			}
		}

		/// <summary>
		/// アプリケーション設定を保存します
		/// </summary>
		public void SaveApplicationSettingFile()
		{
			if (this.ApplicationSetting == null) return;

			if (!File.Exists(ApplicationSettingFilePath))
			{
				File.Create(ApplicationSettingFilePath).Close();
			}

			using (var sw = new StreamWriter(ApplicationSettingFilePath))
			{
				this.ApplicationSetting.Save(sw);
			}
		}

		/// <summary>
		/// アプリケーション終了時に行う処理を記述します
		/// </summary>
		public void ShutdownProcess()
		{
			SaveApplicationSettingFile();
			RemoveTemporaryFiles();
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
		/// テンポラリディレクトリ内のファイルを削除する
		/// </summary>
		private void RemoveTemporaryFiles()
		{
			var dir = new DirectoryInfo(this.TemporaryDirectoryPath);

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
