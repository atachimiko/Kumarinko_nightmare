using log4net;
using Mogami.Core;
using Mogami.Core.Constructions;
using Mogami.Gateway;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mogami.CT.xUnit.Action
{
	/// <summary>
	/// ＜NUnitのアクション＞
	/// </summary>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple = true)]
	public class UsingDatabaseAction : System.Attribute, ITestAction
	{

		#region フィールド

		static Application application = null;
		static ILog LOG = LogManager.GetLogger(typeof(UsingDatabaseAction));

		#endregion フィールド


		#region プロパティ

		public bool ImportInitializeDataFlag { get; set; }
		/// <summary>
		/// ImportInitializeDataFlagフラグがTrueの時、ユニットテスト用のSQLファイルを読み込みます。
		/// </summary>
		public string ImportSqlFileName { get; set; }

		public ActionTargets Targets
		{
			get { return ActionTargets.Suite; }
		}

		bool InitializeApplicationCompleteFlag { get; set; }

		#endregion プロパティ


		#region メソッド

		public void AfterTest(ITest testDetails)
		{
			lock (this)
			{
				LOG.Info("UsingDatabaseAction::AfterTest");
				ApplicationContextImpl.Dispose();
			}
		}

		public void BeforeTest(ITest testDetails)
		{
			lock (this)
			{
				LOG.Info("UsingDatabaseAction::BeforeTest");

				string personalDirectoryPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				Console.WriteLine("Dir={0}", personalDirectoryPath);

				if (application == null)
					application = new Application();

				var buildAssemblyType = new BuildAssemblyParameter();
				buildAssemblyType.Params["ApplicationDirectoryPath"] = @"\Mogami.CT.xUnit";

				ApplicationContext.SetupApplicationContextImpl(ApplicationContextImpl.CreateInstance(application, buildAssemblyType));
				var task = Task.Factory.StartNew(() =>
				{
					LOG.Info("アプリケーションの初期化を開始します。");
					this.InitializeApplicationCompleteFlag = false;
					((ApplicationContextImpl)ApplicationContextImpl.GetInstance()).RemoveUnitDbFile(); // DBファイルが削除できないこともある
																									   //((ApplicationContextImpl)ApplicationContextImpl.GetInstance()).InitializeApplicationProgressEvent += UsingDatabaseActionAttribute_InitializeApplicationProgressEvent;
					((ApplicationContextImpl)ApplicationContextImpl.GetInstance()).InitializeApplication();

					DeleteAllData("App");
					DeleteAllData("Thumbnail");

					((ApplicationContextImpl)ApplicationContextImpl.GetInstance()).Initialize(InitializeParamType.DATABASE);

					LOG.Info("アプリケーションの初期化が終了しました。");
				});
				task.Wait();

				LOG.Info("データベースの初期化を完了しました。");

				if (ImportInitializeDataFlag)
				{
					ImportInitializeData();
					if (!string.IsNullOrEmpty(ImportSqlFileName))
						ImportSqlFile();
				}
			}
			Console.WriteLine("BeforeTest");
		}


		/// <summary>
		/// 
		/// </summary>
		void DeleteAllData(string databasename)
		{
			// データベースにテーブルなどの構造を初期化する
			string sqltext = "";
			System.Reflection.Assembly assm = System.Reflection.Assembly.GetExecutingAssembly();

			using (var stream = assm.GetManifestResourceStream(
				string.Format("Mogami.CT.xUnit.Assets.Sql.{0}.DeleteAllData.txt", databasename)
			))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					sqltext = reader.ReadToEnd();
				}
			}

			if (!string.IsNullOrEmpty(sqltext))
			{
				using (var @dbc = new AppDbContext())
				{
					@dbc.Database.ExecuteSqlCommand(sqltext);
					@dbc.SaveChanges();
				}

				using (var @dbc = new ThumbDbContext())
				{
					@dbc.Database.ExecuteSqlCommand(sqltext);
					@dbc.SaveChanges();
				}
			}
		}

		/// <summary>
		/// ユニットテスト用のデータをデータベースに書き込む
		/// </summary>
		void ImportInitializeData()
		{
			// データベースにテーブルなどの構造を初期化する
			string sqltext = "";
			System.Reflection.Assembly assm = System.Reflection.Assembly.GetExecutingAssembly();

			using (var stream = assm.GetManifestResourceStream("Mogami.CT.xUnit.Assets.Sql.App.CommonInitializeData.txt"))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					sqltext = reader.ReadToEnd();
				}
			}

			if (!string.IsNullOrEmpty(sqltext))
			{
				using (var @dbc = new AppDbContext())
				{
					@dbc.Database.ExecuteSqlCommand(sqltext);
					@dbc.SaveChanges();
				}
			}
		}

		/// <summary>
		/// ユニットテスト用のSQLファイルを読み込む
		/// </summary>
		void ImportSqlFile()
		{
			string sqltext = "";
			System.Reflection.Assembly assm = System.Reflection.Assembly.GetExecutingAssembly();

			using (var stream = assm.GetManifestResourceStream(string.Format("Mogami.CT.xUnit.Assets.Sql.{0}", ImportSqlFileName)))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					sqltext = reader.ReadToEnd();
				}
			}

			if (!string.IsNullOrEmpty(sqltext))
			{
				using (var @dbc = new AppDbContext())
				{
					@dbc.Database.ExecuteSqlCommand(sqltext);
					@dbc.SaveChanges();
				}
			}
		}

		#endregion メソッド

	}
}
