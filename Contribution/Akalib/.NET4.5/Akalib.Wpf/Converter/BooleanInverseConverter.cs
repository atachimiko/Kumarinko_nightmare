using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace Akalib.Wpf.Converter
{
	[ValueConversion(typeof(Boolean), typeof(Boolean))]
	public class BooleanInverseConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return false;
			}
			Boolean b = (Boolean)value;
			return b == true ? false : true;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return false;
			}
			Boolean b = (Boolean)value;
			return b == true ? false : true;
		}
	}
}
