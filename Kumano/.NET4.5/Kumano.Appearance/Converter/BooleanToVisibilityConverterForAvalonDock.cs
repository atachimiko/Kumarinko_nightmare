﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Kumano.View.Converter
{
	/// <summary>
	///  AvalonDock用のBoolranToVisibilityConverter
	/// </summary>
	[ValueConversion(typeof(Boolean), typeof(Visibility))]
	public class BooleanToVisibilityConverterForAvalonDock : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool && targetType == typeof(Visibility))
			{
				bool val = (bool)value;
				if (val)
					return Visibility.Visible;
				else
					if (parameter != null && parameter is Visibility)
					return parameter;
				else
					return Visibility.Collapsed;

			}
			else
			{
				return Visibility.Visible;
			}
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
