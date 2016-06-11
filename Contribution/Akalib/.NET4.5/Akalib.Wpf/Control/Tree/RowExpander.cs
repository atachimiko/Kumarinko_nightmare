using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Akalib.Wpf.Control.Tree
{
	public class RowExpander : System.Windows.Controls.Control
	{

		#region コンストラクタ

		static RowExpander()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(RowExpander), new FrameworkPropertyMetadata(typeof(RowExpander)));
		}

		#endregion コンストラクタ
	}
}
