using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Pcm
{
	public interface IPlugIn : IPluggable
	{
		/// <summary>
		/// プラグイン名
		/// </summary>
		string Name { get; }
	}
}
