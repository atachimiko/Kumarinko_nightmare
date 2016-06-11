using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Akalib.Wpf.Converter
{
	[ValueConversion(typeof(Int32),typeof(String))]
	public class InputTextIntegerConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			try
			{
				return Int32.Parse(value.ToString());
			}
			catch (Exception e)
			{
				return 0;
			}
		}
	}
}
