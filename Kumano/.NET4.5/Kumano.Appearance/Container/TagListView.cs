using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kumano.View.Container
{
	/// <summary>
	/// 
	/// </summary>
	public class TagListView : ViewBase
	{

		#region プロパティ

		//ListViewのスタイルのキーを返す
		protected override object DefaultStyleKey
		{
			get { return new ComponentResourceKey(GetType(), "TagListView"); }
		}

		//ListViewItemのスタイルのキーを返す
		protected override object ItemContainerDefaultStyleKey
		{
			get { return new ComponentResourceKey(GetType(), "TagListViewItem"); }
		}

		#endregion プロパティ

	}
}
