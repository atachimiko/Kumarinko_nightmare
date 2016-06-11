using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Akalib.Wpf
{
	/// <summary>
	/// ScrollViewerとMVVMの親和性を高めるためのユーティリティクラス
	/// </summary>
	/// <remarks>
	/// ScrollViewer.ScrollToHorizontalOffsetメソッドなどへの関数でしか提供されていないScrollViewerのメンバーに
	/// 依存関係プロパティを使ってアクセスするためのユーティリティを含みます
	/// 
	/// </remarks>
	public static class ScrollViewerUtilities
	{
		// http://marlongrech.wordpress.com/2009/09/14/how-to-set-wpf-scrollviewer-verticaloffset-and-horizontal-offset/
		#region 依存関係プロパティ

		#region HorizontalOffset : ScrollViewer.ScrollToHorizontalOffsetとバインドするための依存関係プロパティ
		/// <summary>
		/// HorizontalOffset Attached Dependency Property
		/// </summary>
		public static readonly DependencyProperty HorizontalOffsetProperty =
			DependencyProperty.RegisterAttached("HorizontalOffset", typeof(double), typeof(ScrollViewerUtilities),
			   new FrameworkPropertyMetadata((double)0.0,
				   new PropertyChangedCallback(OnHorizontalOffsetChanged)));

		public static double GetHorizontalOffset(DependencyObject d)
		{
			return (double)d.GetValue(HorizontalOffsetProperty);
		}

		public static void SetHorizontalOffset(DependencyObject d, double value)
		{
			d.SetValue(HorizontalOffsetProperty, value);
		}

		/// <summary>
		/// Handles changes to the HorizontalOffset property.
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e"></param>
		private static void OnHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var viewer = (ScrollViewer)d;
			viewer.ScrollToHorizontalOffset((double)e.NewValue);
		}
		#endregion

		#region VerticalOffset
		public static readonly DependencyProperty VerticalOffsetProperty =
			DependencyProperty.RegisterAttached("VerticalOffset", typeof(double), typeof(ScrollViewerUtilities),
			 new FrameworkPropertyMetadata((double)0.0,
				 new PropertyChangedCallback(OnVerticalOffsetChanged)));

		public static double GetVerticalOffset(DependencyObject d)
		{
			return (double)d.GetValue(VerticalOffsetProperty);
		}

		public static void SetVerticalOffset(DependencyObject d, double value)
		{
			d.SetValue(VerticalOffsetProperty, value);
		}

		private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var viewer = (ScrollViewer)d;
			viewer.ScrollToVerticalOffset((double)e.NewValue);
		}
		#endregion


		#endregion
	}
}
