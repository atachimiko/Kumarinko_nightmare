using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Akalib.Sample.VirtualWrapImageList
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// 参照元 ・・・ http://csfun.blog49.fc2.com/blog-entry-90.html
	/// </remarks>
	public class ImageListView : ViewBase
	{
		//ListViewのスタイルのキーを返す
		protected override object DefaultStyleKey
		{
			get { return new ComponentResourceKey(GetType(), "ImageView"); }
		}

		//ListViewItemのスタイルのキーを返す
		protected override object ItemContainerDefaultStyleKey
		{
			get { return new ComponentResourceKey(GetType(), "ImageViewItem"); }
		}
	}
}
