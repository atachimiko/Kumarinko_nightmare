using Akalib;
using Akalib.Entity;
using log4net;
using Mogami.Applus.Manager;
using Mogami.Core.Constructions;
using Mogami.Core.Infrastructure;
using Mogami.Gateway;
using Mogami.Model;
using Mogami.Model.Repository;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Mogami.Pcm;

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
		/// プラグイン管理
		/// </summary>
		private KumarinkoPlugInManager _KumarinkoPlugInManager;

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

			CreateSettingSQLite();
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
		/// プラグイン管理オブジェクト
		/// </summary>
		public IPlugInBasedManager PlugInManager
		{
			get
			{
				return _KumarinkoPlugInManager;
			}
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
					InitializeDatabaseThumbnail();
					break;
				case InitializeParamType.PLUGIN:
					InitializePlugIn();
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
			Initialize(InitializeParamType.PLUGIN);
		}

		/// <summary>
		/// [ユニットテスト] データベースファイルを削除します
		/// </summary>
		/// <remarks>
		/// ※ユニットテストからのみ呼び出してください。
		/// </remarks>
		public void RemoveUnitDbFile()
		{

			LOG.Info("ユニットテストで使用するデータベースファイルを削除します");
			var dir = new DirectoryInfo(this.DatabaseDirectoryPath);
			if (!dir.Exists) return;

			foreach (var file in dir.GetFiles())
			{
				// ファイルの削除に失敗しても処理は継続
				try
				{
					file.IsReadOnly = false;
					file.Delete();
					LOG.InfoFormat("ユニットテストで使用するデータベースファイルを削除しました({0})", file.Name);
				}
				catch (Exception expr)
				{
					LOG.FatalFormat("データベースファイルの削除に失敗しました。({0})", expr.Message);
				}
			}

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
		/// SQLiteを使用するための設定を読み込みます
		/// </summary>
		void CreateSettingSQLite()
		{
			SQLiteConnectionStringBuilder builder_AppDb = new SQLiteConnectionStringBuilder();
			builder_AppDb.DataSource = Path.Combine(DatabaseDirectoryPath, "mogami.db");
			AppDbContext.SDbConnection = builder_AppDb;

			SQLiteConnectionStringBuilder builder_ThumbDb = new SQLiteConnectionStringBuilder();
			builder_ThumbDb.DataSource = Path.Combine(DatabaseDirectoryPath, "thumbnail.db");
			ThumbDbContext.SDbConnection = builder_ThumbDb;
		}

		/// <summary>
		/// データベースに関する初期化処理
		/// </summary>
		private void InitializeDatabase()
		{
			ApMetadata apMetadata = null;
			bool isMigrate = false;

			// 構造の初期化
			using (var @dbc = new AppDbContext())
			{
				bool isInitializeDatabase = false;
				var @repo = new ApMetadataRepository(@dbc);
				try
				{
					apMetadata = @repo.FindBy(p => p.Key == "APVER").FirstOrDefault();
					if (apMetadata == null) isInitializeDatabase = true;
				}
				catch (Exception)
				{
					isInitializeDatabase = true;
				}

				if (isInitializeDatabase)
				{
					// データベースにテーブルなどの構造を初期化する
					string sqltext = "";
					System.Reflection.Assembly assm = System.Reflection.Assembly.GetExecutingAssembly();

					using (var stream = assm.GetManifestResourceStream("Mogami.Assets.Sql.Initialize_sql.txt"))
					{
						using (StreamReader reader = new StreamReader(stream))
						{
							sqltext = reader.ReadToEnd();
						}
					}
					@dbc.Database.ExecuteSqlCommand(sqltext);
					@dbc.SaveChanges();

					apMetadata = @repo.FindBy(p => p.Key == "APVER").FirstOrDefault();
				}

				if (apMetadata == null)
				{
					apMetadata = new ApMetadata { Key = "APVER", Value = "1.0.0" };
					@repo.Add(apMetadata);

					@repo.Save();
				}

				string currentVersion = apMetadata.Value;
				string nextVersion = currentVersion;
				do
				{
					currentVersion = nextVersion;
					nextVersion = UpgradeFromResource(currentVersion, @dbc);
					if (nextVersion != currentVersion) isMigrate = true;
				} while (nextVersion != currentVersion);

				if (isMigrate)
				{
					apMetadata.Value = nextVersion;

					@repo.Save();
				}

				@dbc.SaveChanges();
			}
		}

		/// <summary>
		/// データベースに関する初期化処理
		/// </summary>
		private void InitializeDatabaseThumbnail()
		{
			ApMetadata apMetadata = null;
			bool isMigrate = false;

			// 構造の初期化
			using (var @dbc = new ThumbDbContext())
			{
				bool isInitializeDatabase = false;
				var @repo = new ApMetadataRepository(@dbc);
				try
				{
					apMetadata = @repo.FindBy(p => p.Key == "APVER").FirstOrDefault();
					if (apMetadata == null) isInitializeDatabase = true;
				}
				catch (Exception)
				{
					isInitializeDatabase = true;
				}

				if (isInitializeDatabase)
				{
					// データベースにテーブルなどの構造を初期化する
					string sqltext = "";
					System.Reflection.Assembly assm = System.Reflection.Assembly.GetExecutingAssembly();

					using (var stream = assm.GetManifestResourceStream("Mogami.Assets.Sql.Thumbnail_Initialize_sql.txt"))
					{
						using (StreamReader reader = new StreamReader(stream))
						{
							sqltext = reader.ReadToEnd();
						}
					}
					@dbc.Database.ExecuteSqlCommand(sqltext);
					@dbc.SaveChanges();

					apMetadata = @repo.FindBy(p => p.Key == "APVER").FirstOrDefault();
				}

				if (apMetadata == null)
				{
					apMetadata = new ApMetadata { Key = "APVER", Value = "1.0.0" };
					@repo.Add(apMetadata);

					@repo.Save();
				}

				string currentVersion = apMetadata.Value;
				string nextVersion = currentVersion;
				do
				{
					currentVersion = nextVersion;
					nextVersion = UpgradeFromResource(currentVersion, @dbc);
					if (nextVersion != currentVersion) isMigrate = true;
				} while (nextVersion != currentVersion);

				if (isMigrate)
				{
					apMetadata.Value = nextVersion;

					@repo.Save();
				}

				@dbc.SaveChanges();
			}
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

		private void InitializePlugIn()
		{
			var pluginFolderPath = Path.Combine(ApplicationSettingFilePath, "plugins");
			if (!Directory.Exists(pluginFolderPath))
				System.IO.Directory.CreateDirectory(pluginFolderPath);

			_KumarinkoPlugInManager = new KumarinkoPlugInManager();
			_KumarinkoPlugInManager.PlugInFolder = pluginFolderPath;

			// フォルダ内の、プラグインDLLを読み込む
			_KumarinkoPlugInManager.LoadPlugIns();
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

		/// <summary>
		///
		/// </summary>
		/// <param name="resourcePath"></param>
		/// <param name="dbc">データベース</param>
		private void UpgradeDatabase(string resourcePath, AtDbContext dbc)
		{
			string sqltext = "";
			System.Reflection.Assembly assm = System.Reflection.Assembly.GetExecutingAssembly();

			using (var stream = assm.GetManifestResourceStream(resourcePath))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					sqltext = reader.ReadToEnd();
				}
			}

			dbc.Database.ExecuteSqlCommand(sqltext);
		}

		/// <summary>
		/// 現在のバージョンからマイグレーションするファイルがリソースファイルにあるか探します。
		/// リソースファイルがある場合はそのファイルに含まれるSQLを実行し、ファイル名からマイグレーション後のバージョンを取得します。
		/// </summary>
		/// <param name="version">現在のバージョン。アップグレード元のバージョン。</param>
		/// <returns>次のバージョン番号。マイグレーションを実施しなかった場合は、versionの値がそのまま帰ります。</returns>
		private string UpgradeFromResource(string version, AtDbContext @dbc)
		{
			System.Reflection.Assembly assm = System.Reflection.Assembly.GetExecutingAssembly();

			string currentVersion = version;
			var mss = assm.GetManifestResourceNames();

			// 「Yukikaze.Assets.Sql.upgrade-1.0.0-1.1.0.txt」というリソースファイルを探す。
			// この方法で読み込みができるリソースファイルは、「埋め込みリソース」です。
			var r = new Regex("Mogami.Assets.Sql.upgrade-" + currentVersion + "-(.+)\\.txt");
			foreach (var rf in assm.GetManifestResourceNames())
			{
				var matcher = r.Match(rf);
				if (matcher.Success && matcher.Groups.Count > 1)
				{
					UpgradeDatabase(rf, @dbc);
					currentVersion = matcher.Groups[1].Value; // 正規表現にマッチした箇所が、マイグレート後のバージョンになります。
				}
			}

			return currentVersion;
		}

		#endregion メソッド

	}
}

