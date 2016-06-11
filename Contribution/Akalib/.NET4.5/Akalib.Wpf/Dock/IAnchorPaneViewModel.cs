using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalib.Wpf.Dock
{
	/// <summary>
	/// アンカーペイン
	/// </summary>
	public interface IAnchorPaneViewModel : IPaneViewModel
	{
		/// <summary>
		/// 
		/// </summary>
		bool IsVisible { get; }
	}
}
