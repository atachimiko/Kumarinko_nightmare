using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalib.Sample.VirtualWrapImageList.Model
{
	/// <summary>
	/// 遅延読み込みをサポートするアイテム
	/// </summary>
	interface ILazyLoadingItem
	{
		bool IsLoaded { get; }
		event EventHandler Loaded;
	}
}
