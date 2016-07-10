using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Pcm
{
	/// <summary>
	/// This class must be inherited by applications that uses plugins.
	/// </summary>
	/// <typeparam name="TPlugIn">Type of PlugIn interface that is used by application</typeparam>
	public class PlugInBasedApplication<TPlugIn> : IPlugInBasedManager
	{
		/// <summary>
		/// プラグインマネージャ名
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// 読み込み済みのプラグイン一覧
		/// </summary>
		public List<IMogamiPlugIn<TPlugIn>> PlugIns { get; private set; }

		/// <summary>
		/// プラグイン格納ディレクトリ
		/// Default: Root directory of application.
		/// </summary>
		public string PlugInFolder { get; set; }

		/// <summary>
		/// プラグインのロード状態
		/// </summary>
		public bool PlugInsLoaded { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public PlugInBasedApplication()
		{
			Initialize();
			PlugInFolder = PcmHelper.GetCurrentDirectory();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="plugInFolder">プラグイン格納ディレクトリパス</param>
		public PlugInBasedApplication(string plugInFolder)
		{
			Initialize();
			PlugInFolder = plugInFolder;
		}

		/// <summary>
		/// プラグイン格納ディレクトリから、プラグインを読み込みます。
		/// </summary>
		public void LoadPlugIns()
		{
			if (string.IsNullOrEmpty(PlugInFolder) || !Directory.Exists(PlugInFolder))
			{
				throw new ApplicationException("PlugInFoler must be a valid folder path");
			}

			var assemblyFiles = PcmHelper.FindAssemblyFiles(PlugInFolder);
			var plugInType = typeof(TPlugIn);
			foreach (var assemblyFile in assemblyFiles)
			{
				var allTypes = Assembly.LoadFrom(assemblyFile).GetTypes();
				foreach (var type in allTypes)
				{
					if (plugInType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
					{
						PlugIns.Add(new MogamiPlugIn<TPlugIn>(this, type));
					}
				}
			}

			PlugInsLoaded = true;
		}

		/// <summary>
		/// 初期化
		/// </summary>
		private void Initialize()
		{
			var plugInApplicationAttribute = PcmHelper.GetAttribute<PlugInApplicationAttribute>(GetType());
			Name = plugInApplicationAttribute == null ? GetType().Name : plugInApplicationAttribute.Name;
			PlugIns = new List<IMogamiPlugIn<TPlugIn>>();
		}
	}
}
