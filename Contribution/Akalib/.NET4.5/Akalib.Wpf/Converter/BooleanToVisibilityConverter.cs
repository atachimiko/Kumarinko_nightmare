using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Globalization;

namespace Akalib.Wpf.Converter
{
	[ValueConversion(typeof(Boolean), typeof(Visibility))]
	public class BooleanToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return Visibility.Visible;
			}
			Boolean b = (Boolean)value;
			return b == true ? Visibility.Visible : Visibility.Hidden;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}

	[ValueConversion(typeof(Boolean), typeof(Visibility))]
	public class BooleanToUnVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return Visibility.Visible;
			}
			Boolean b = (Boolean)value;
			return b == false ? Visibility.Visible : Visibility.Hidden;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
