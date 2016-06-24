using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace Kumano.View
{
	public class LocalNameTemplateSelector : DataTemplateSelector
	{

		#region メソッド

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			XmlElement data = item as XmlElement;
			if (data != null)
			{
				return ((FrameworkElement)container).FindResource(data.LocalName) as DataTemplate;
			}
			return null;
		}

		#endregion メソッド
	}
}
