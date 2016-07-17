using Kumano.Core.Constractures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kumano.Core.Infrastructures
{
	

	public interface IApplicationContext
	{

		#region プロパティ

		/// <summary>
		/// アプリケーションが使用するファイルシステム上のディレクトリのパスを取得します
		/// </summary>
		string ApplicationDirectoryPath { get; }

		/// <summary>
		/// アプリケーションのバージョン情報を取得します
		/// </summary>
		System.Diagnostics.FileVersionInfo ApplicationFileVersionInfo { get; }

		/// <summary>
		/// アプリケーション設定データ
		/// </summary>
		ApplicationSettingInfo ApplicationSetting { get; }

		/// <summary>
		/// コンフィグファイルを保存するディレクトリのパスを取得します
		/// </summary>
		string ConfigDirectoryPath { get; }

		/// <summary>
		/// 
		/// </summary>
		DeviceSettingInfo DeviceSettingInfo { get; }

		/// <summary>
		/// 
		/// </summary>
		Window MainWindow { get; }

		/// <summary>
		/// 
		/// </summary>
		string TemporaryDirectoryPath { get; }

		/// <summary>
		/// アプリケーションのワークスペースViewModelを取得します
		/// </summary>
		IWorkspaceViewModel Workspace { get; }

		#endregion プロパティ

		#region メソッド

		/// <summary>
		/// アプリケーションを終了します
		/// </summary>
		void ApplicationExit();

		/// <summary>
		/// アプリケーションの初期化
		/// </summary>
		void InitializeApplication();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		Task<bool> LoadDeviceSettingInfoAsync();

		/// <summary>
		/// アプリケーション設定情報の保存
		/// </summary>
		void SaveApplicationSettingFile();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		Task<bool> SaveDeviceSettingInfoAsync();
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		void Watch(string message);

		#endregion メソッド

	}
}
