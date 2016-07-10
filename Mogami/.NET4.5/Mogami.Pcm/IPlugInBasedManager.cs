using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Pcm
{
	/// <summary>
	/// All plugin-based applications implements this interface.
	/// Applications does not directly implement this interface, but they implement by inheriting PlugInBasedApplication class.
	/// </summary>
	public interface IPlugInBasedManager : IPluggable
	{
		/// <summary>
		/// アプリケーション名
		/// </summary>
		string Name { get; }
	}
}
