using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Akalib.Wpf.Converter
{
	/// <summary>
	/// 値がNULLの時、0を返すコンバーター(Integer専用)
	/// </summary>
	public class IntegerNullConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is int) { return value; }
			return 0;
		}


		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null) return 0;

			return value;
		}
	}
}
