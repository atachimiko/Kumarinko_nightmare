using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Akalib.Wpf.Dock
{
	/// <summary>
	/// ペイン
	/// </summary>
	public interface IPaneViewModel
	{
		void ActiveChanged();
		string ContentId { get; }
		ImageSource IconSource { get; }
		
		string Title { get; }
		string ToolTip { get; }
	}
}
