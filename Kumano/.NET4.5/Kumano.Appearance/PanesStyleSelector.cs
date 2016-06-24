using Akalib.Wpf.Dock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kumano.View
{
	class PanesStyleSelector : StyleSelector
	{
		public override System.Windows.Style SelectStyle(object item, System.Windows.DependencyObject container)
		{
			if (item is IAnchorPaneViewModel)
				return DefaultAnchorContntsStyle;

			return DefaultContentsStyle;
		}
		public Style DefaultAnchorContntsStyle { get; set; }
		public Style DefaultContentsStyle { get; set; }
	}
}
