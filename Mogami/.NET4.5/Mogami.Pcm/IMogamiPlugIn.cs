using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Pcm
{
	/// <summary>
	/// </summary>
	/// <typeparam name="TPlugIn">プラグインインターフェース</typeparam>
	public interface IMogamiPlugIn<out TPlugIn>
	{
		/// <summary>
		/// プラグイン名
		/// </summary>
		string Name { get; }

		/// <summary>
		/// アプリケーションからプラグインインターフェースを使用してプラグインの処理を
		/// 呼び出します。
		/// </summary>
		TPlugIn PlugInProxy { get; }
	}
}
