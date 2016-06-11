using Akalib.Wpf.Dock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Akalib.Wpf.Mvvm.Converter
{
	public class ActiveDocumentConverter : IValueConverter
	{


		#region メソッド

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is IDocumentPaneViewModel)
				return value;

			return Binding.DoNothing;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is IDocumentPaneViewModel)
				return value;

			return Binding.DoNothing;
		}

		#endregion メソッド

	}
}
