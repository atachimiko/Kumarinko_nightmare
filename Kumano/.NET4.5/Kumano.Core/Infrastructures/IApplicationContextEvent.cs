using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kumano.Core.Infrastructures
{
	public delegate void LoadedDeviceSettingEventHandler();

	public interface IApplicationContextEvent
	{


		#region イベント

		event LoadedDeviceSettingEventHandler LoadedDeviceSetting;

		#endregion イベント

	}
}
