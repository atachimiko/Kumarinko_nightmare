using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Text.RegularExpressions;

namespace Akalib.Wpf.Converter
{
	/// <summary>
	/// 文字列内に含まれる全角スペースを半角スペースに置き換えます。また。文字列の前後に含まれるスペースをトリミングします。
	/// </summary>
	[ValueConversion(typeof(string), typeof(string))]
	public class WhiteSpaceRemoveConverter : IValueConverter
	{
		static Regex spaceRegex = new Regex(@"　+");
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return null;

			if (value is string)
			{

				var trimed = ((string)value).Trim();
				var s = spaceRegex.Replace(trimed, " ");
				return s;
			}

			return value;
		}


		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
				return null;

			if (value is string)
			{

				var trimed = ((string)value).Trim();
				var s = spaceRegex.Replace(trimed, " ");
				return s;
			}

			return value;
		}
	}
}
