using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Akalib.Wpf
{
	/// <summary>
	/// WebBrowserとMVVMの親和性を高めるためのユーティリティ
	/// </summary>
	/// <remarks>
	/// WebBrowserのSourceプロパティはBindingを使って値を設定することができません。
	/// いちど、このユーティリティへBindingを行うことで、その値をこのユーティリティではWebBrowserへ設定します。
	/// </remarks>
	/// <example>
	/// <WebBrowser ns:WebBrowserUtility.BindableSource="{Binding WebAddress}"
	///             ScrollViewer.HorizontalScrollBarVisibility="Disabled" 
	///             ScrollViewer.VerticalScrollBarVisibility="Disabled" 
	///             Width="300"
	///             Height="200" />
	/// </example>
	public static class WebBrowserUtility
	{
		//=====================================================================
		#region 依存関係プロパティ
		//=====================================================================

		#region BindableSource
		public static readonly DependencyProperty BindableSourceProperty =
			DependencyProperty.RegisterAttached("BindableSource", typeof(string), typeof(WebBrowserUtility), new UIPropertyMetadata(null, BindableSourcePropertyChanged));

		public static string GetBindableSource(DependencyObject obj)
		{
			return (string)obj.GetValue(BindableSourceProperty);
		}

		public static void SetBindableSource(DependencyObject obj, string value)
		{
			obj.SetValue(BindableSourceProperty, value);
		}

		public static void BindableSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			WebBrowser browser = o as WebBrowser;
			if (browser != null)
			{
				string uri = e.NewValue as string;
				browser.Source = uri != null ? new Uri(uri) : null;
			}
		}
		#endregion

		#region BindableHtml
		public static readonly DependencyProperty BindableHtmlProperty =
			 DependencyProperty.RegisterAttached("BindableHtml", typeof(string), typeof(WebBrowserUtility), new UIPropertyMetadata(null, BindableHtmlPropertyChanged));

		[AttachedPropertyBrowsableForType(typeof(WebBrowser))]
		public static string GetBindableHtml(DependencyObject obj)
		{
			return (string)obj.GetValue(BindableHtmlProperty);
		}

		public static void SetBindableHtml(DependencyObject obj, string value)
		{
			obj.SetValue(BindableHtmlProperty, value);
		}

		public static void BindableHtmlPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			var browser = o as WebBrowser;
			if (browser != null)
			{
				if (e.NewValue != null && !string.IsNullOrEmpty(e.NewValue.ToString()))
					browser.NavigateToString(e.NewValue.ToString());
				else
					browser.NavigateToString("<html></html>");
			}
		}
		#endregion

		#endregion
	}
}
