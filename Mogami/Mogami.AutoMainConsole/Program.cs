using log4net;
using Mogami.Applus.Manager;
using Mogami.Core;
using Mogami.Core.Model;
using Mogami.Gateway;
using Mogami.Model;
using Mogami.Model.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Mogami.AutoMainConsole
{
	class Program
	{


		#region フィールド

		private static Application application;
		private static ILog LOG = LogManager.GetLogger(typeof(Program));

		#endregion フィールド


		#region メソッド
		/// <summary>
		/// Workspace(Id:2)のファイル監視を行うオブジェクトを作成する
		/// </summary>
		/// <returns></returns>
		static VirtualFileSystemWatcherManager GenerateWatcher()
		{
			using (var dbc = new AppDbContext())
			{
				var repo = new WorkspaceRepository(dbc);
				var entity = repo.Load(2L);
				if (entity == null) throw new ApplicationException("エンティティを見つけることができませんでした。");
				var inst = new VirtualFileSystemWatcherManager(entity);
				return inst;
			}
		}

		static void ImportSqlFile(string ImportSqlFileName)
		{
			string sqltext = "";
			System.Reflection.Assembly assm = System.Reflection.Assembly.GetExecutingAssembly();

			using (var stream = assm.GetManifestResourceStream(string.Format("Mogami.AutoMainConsole.Assets.Sql.{0}", ImportSqlFileName)))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					sqltext = reader.ReadToEnd();
				}
			}

			using (var @dbc = new AppDbContext())
			{
				@dbc.Database.ExecuteSqlCommand(sqltext);
				@dbc.SaveChanges();
			}
		}

		/// <summary>
		/// アプリケーションの初期化を行います
		/// </summary>
		static void InitializeMogamiApplication()
		{
			System.Threading.Thread.CurrentThread.Name = "Mogami AutoMainConsole application thread";
			application = new Application();

			var buildAssemblyType = new BuildAssemblyParameter();
			buildAssemblyType.Params["ApplicationDirectoryPath"] = @"\Mogami.AutoMainConsole";

			ApplicationContext.SetupApplicationContextImpl(ApplicationContextImpl.CreateInstance(application, buildAssemblyType));

			var version = ((ApplicationContextImpl)ApplicationContextImpl.GetInstance()).ApplicationFileVersionInfo;
			LOG.InfoFormat("アプリケーション(Version {0})を起動します", version.ProductVersion);

			((ApplicationContextImpl)ApplicationContextImpl.GetInstance()).InitializeApplication();
		}

		/// <summary>
		/// サンプルで使用する仮想ディレクトリ空間のファイルリストを
		/// データベースに初期データとして追加します。
		/// </summary>
		static void InitializeSampleAclFileImport(Workspace workspace, AppDbContext dbcontext)
		{
			var repo = new FileMappingInfoRepository(dbcontext);
			string[] entries = Directory.GetFiles(workspace.WorkspacePath, "*.alcgene.*", SearchOption.AllDirectories);
			foreach (string entry in entries)
			{
				LOG.InfoFormat("初期データ:{0}", entry);
				var relativePath = workspace.TrimWorekspacePath(entry, false);

				var mapping = new FileMappingInfo();
				mapping.Workspace = workspace;
				mapping.AclHash = "SAMPLE";
				mapping.MappingFilePath = relativePath;

				repo.Add(mapping);
			}

			repo.Save();
		}

		static void Main(string[] args)
		{
			VirtualFileSystemWatcherManager manager = null;
			Console.WriteLine("アプリケーションを起動しています");

			InitializeMogamiApplication();
			ImportSqlFile("OriginalData.sql.txt");

			using (var dbc = new AppDbContext())
			{
				var repo = new WorkspaceRepository(dbc);
				var entity = repo.Load(2L);
				InitializeSampleAclFileImport(entity, dbc);
			}

			PrintWorkspaces();

			manager = GenerateWatcher();

			if (manager != null)
			{
				Console.WriteLine("ディレクトリの監視を開始します。");
				manager.IsSuspendIndex = true;
				manager.StartWatch();
				if (manager.IsSuspendIndex) Console.WriteLine("タイマーは停止中です。");

				bool isExitWatch = false;
				while (!isExitWatch)
				{
					Console.WriteLine("監視動作モード入力:");
					Console.WriteLine(" 1=Dump Workspace");
					Console.WriteLine(" 2=Toggle Suspend Mode [" + manager.IsSuspendIndex + "]");
					Console.WriteLine(" e=Exit");
					Console.Write("> ");
					string watchmode = Console.ReadLine();
					switch (watchmode)
					{
						case "1":
							LOG.Info("\n" + manager.DumpUpdateWatchedFile());
							break;
						case "2":
							manager.IsSuspendIndex = !manager.IsSuspendIndex;
							if (manager.IsSuspendIndex) Console.WriteLine("タイマー処理を一時停止しています。");
							else Console.WriteLine("タイマー処理が有効です。");

							break;
						case "e":
							isExitWatch = true;
							break;
					}
				}
			}


			Console.WriteLine("アプリケーションを終了します");
			Console.ReadLine();
		}

		static void PrintWorkspaces()
		{
			using (var dbc = new AppDbContext())
			{
				var repo = new WorkspaceRepository(dbc);
				foreach (var p in repo.GetAll())
				{
					LOG.InfoFormat("[ID:{0}] Name:{1} Ph:{2} Vi:{3}", p.Id, p.Name, p.PhysicalPath, p.WorkspacePath);
				}
			}
		}

		#endregion メソッド

	}
}
