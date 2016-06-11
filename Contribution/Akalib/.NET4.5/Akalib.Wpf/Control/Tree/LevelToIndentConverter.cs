using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Akalib.Wpf.Control.Tree
{
	internal class CanExpandConverter : IValueConverter
	{

		#region メソッド

		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			if ((bool)o)
				return Visibility.Visible;
			else
				return Visibility.Hidden;
		}

		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		#endregion メソッド
	}

	/// <summary>
	/// Convert Level to left margin
	/// </summary>
	internal class LevelToIndentConverter : IValueConverter
	{

		#region フィールド

		private const double IndentSize = 19.0;

		#endregion フィールド


		#region メソッド

		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			return new Thickness((int)o * IndentSize, 0, 0, 0);
		}

		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		#endregion メソッド
	}
}
