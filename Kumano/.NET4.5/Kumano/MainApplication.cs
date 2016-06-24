using Akalib;
using Kumano.Appearance;
using Kumano.Core;
using Kumano.Data.ViewModel;
using Kumano.View;
using Livet;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kumano
{
	public static class MainApplication
	{

		#region フィールド

		static App application;
		static ILog LOG = LogManager.GetLogger(typeof(MainApplication));

		#endregion フィールド


		#region メソッド

		[STAThread]
		public static int Main(string[] args)
		{
			MainWindow mainWindow;

			System.Threading.Thread.CurrentThread.Name = "Kumarinko.Kumano";


#if !DEBUG
			// Don't catch exceptions when debugging - we want to have Visual Studio catch them where and when
			// they are thrown
			try {

			System.Threading.Mutex mutex = new System.Threading.Mutex(false, System.Threading.Thread.CurrentThread.Name);
			if (mutex.WaitOne(0, false) == false)
			{
				MessageBox.Show("アプリケーションはすでに起動中です。\nこのアプリケーションの多重起動はできません。", "アプリケーションの起動エラー | SakuraFs", MessageBoxButton.OK, MessageBoxImage.Stop);
			}
			else
			{
#endif
			application = new App();
			DispatcherHelper.UIDispatcher = application.Dispatcher;

			DumpEnvironmentLog();

			CreateApplicationContext();

			ApplicationContext.Watch("データモデルの作成");
			var workspace = new WorkspaceViewModel();

			InitializeApplicationContext();

			// [アプリケーションの実行]
			//	- アプリケーションが終了するまで、リターンはブロックされます。
			//  - コメントアウトすると、ウィンドウを表示せずにアプリケーションは終了します。
			((ApplicationContextImpl)ApplicationContextImpl.GetInstance()).Workspace = workspace;
			ApplicationContext.Watch("メインウィンドウの作成");

			mainWindow = new MainWindow();
			mainWindow.DataContext = workspace;
			((ApplicationContextImpl)ApplicationContextImpl.GetInstance()).MainWindow = mainWindow;

			SettingConfigWindowsStatus(mainWindow);

			// メインウィンドウの表示
			application.Run(mainWindow);

			// アプリケーション終了時の、ウィンドウの位置を設定に保存する
			ApplicationContext.ApplicationSetting.WindowLocation = new Point(mainWindow.Left, mainWindow.Top);

			// アプリケーション終了時に行う処理の呼び出し
			((ApplicationContextImpl)ApplicationContextImpl.GetInstance()).ShutdownProcess();
#if !DEBUG
				//ミューテックスを解放する
				mutex.ReleaseMutex();
			}

			} catch (Exception e) {
				System.Diagnostics.Debug.WriteLine("There was a program exception: " + e);
				MessageBox.Show("There was a program exception: " + e);
				return -1;
			}//catch
#endif
			LOG.Info("アプリケーションを終了しました");
			return 1;
		}

		/// <summary>
		/// アプリケーションの起動
		/// </summary>
		static void CreateApplicationContext()
		{
			ApplicationContext.SetupApplicationContextImpl(ApplicationContextImpl.CreateInstance(application));
			var version = ((ApplicationContextImpl)ApplicationContextImpl.GetInstance()).ApplicationFileVersionInfo;
			LOG.InfoFormat("アプリケーション(Version {0})を起動します", version.ProductVersion);
		}

		/// <summary>
		/// 実行環境のログ出力
		/// </summary>
		static void DumpEnvironmentLog()
		{
			var osVersion = new OsVersion();
			LOG.Info(osVersion.GetOsDisplayString());
			LOG.Info(osVersion.GetEnvironmentString());

			string clrVersionRuntime = System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion();
			LOG.Info("実行中のCLR = " + clrVersionRuntime);
		}



		/// <summary>
		/// アプリケーションの初期化
		/// </summary>
		static void InitializeApplicationContext()
		{
			((ApplicationContextImpl)ApplicationContextImpl.GetInstance()).InitializeApplicationFiles();
			((ApplicationContextImpl)ApplicationContextImpl.GetInstance()).InitializeApplication();
		}


		/// <summary>
		///     アプリケーション設定情報から取得した
		///     ウィンドウの表示位置や大きさを設定します。
		/// </summary>
		/// <param name="window"></param>
		static void SettingConfigWindowsStatus(MainWindow mainWindow)
		{
			if (ApplicationContext.ApplicationSetting.WindowSize != null)
			{
				if (ApplicationContext.ApplicationSetting.WindowSize.Width > 0.0)
					mainWindow.Width = ApplicationContext.ApplicationSetting.WindowSize.Width;
				if (ApplicationContext.ApplicationSetting.WindowSize.Height > 0.0)
					mainWindow.Height = ApplicationContext.ApplicationSetting.WindowSize.Height;

				if (ApplicationContext.ApplicationSetting.WindowLocation.X > 0.0)
				{
					mainWindow.WindowStartupLocation = WindowStartupLocation.Manual;
					mainWindow.Left = ApplicationContext.ApplicationSetting.WindowLocation.X;
				}

				if (ApplicationContext.ApplicationSetting.WindowLocation.Y > 0.0)
				{
					mainWindow.WindowStartupLocation = WindowStartupLocation.Manual;
					mainWindow.Top = ApplicationContext.ApplicationSetting.WindowLocation.Y;
				}
			}
		}
		#endregion メソッド
	}
}
