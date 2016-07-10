using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Pcm
{
	/// <summary>
	/// This class is used to make possible to create a plugin that implements
	/// IPlugIn interface. A plugin must derive this class to be used by
	/// main application.
	/// </summary>
	/// <typeparam name="TApp">Type of main application interface</typeparam>
	public abstract class PlugIn<TApp> : IPlugIn
	{
		/// <summary>
		/// Gets a reference to main application.
		/// </summary>
		public IPlugInApplication<TApp> Application { get; internal set; }

		/// <summary>
		/// プラグイン名
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		protected PlugIn()
		{
			Application = new PlugInApplication<TApp>();

			//Get Name from PlugIn attribute.
			var thisPlugInType = GetType();
			var plugInAttribute = PcmHelper.GetAttribute<PlugInAttribute>(thisPlugInType);
			Name = plugInAttribute == null ? thisPlugInType.Name : plugInAttribute.Name;
		}
	}
}
