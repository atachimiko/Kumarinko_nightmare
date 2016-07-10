using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Pcm
{
	internal class MogamiPlugIn<TPlugIn> : IMogamiPlugIn<TPlugIn>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="plugInApplication"></param>
		/// <param name="plugInType"></param>
		public MogamiPlugIn(IPlugInBasedManager plugInApplication, Type plugInType)
		{
			PlugInProxy = (TPlugIn)Activator.CreateInstance(plugInType);

			var plugInObjectType = PlugInProxy.GetType();

			var applicationProperty = plugInObjectType.GetProperty("Application");
			var applicationPropertyValue = applicationProperty.GetValue(PlugInProxy, null);
			var applicationPropertyType = applicationPropertyValue.GetType();

			applicationPropertyType.GetProperty("Name").SetValue(applicationPropertyValue, plugInApplication.Name, null);
			applicationPropertyType.GetProperty("ApplicationProxy").SetValue(applicationPropertyValue, plugInApplication, null);

			Name = ((IPlugIn)PlugInProxy).Name;
		}

		/// <summary>
		/// プラグイン名
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// アプリケーションからプラグインインターフェースを使用してプラグインの処理を
		/// 呼び出します。
		/// </summary>
		public TPlugIn PlugInProxy { get; private set; }
	}
}
