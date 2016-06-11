using Livet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Akalib.Sample.VirtualWrapImageList
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{


		#region フィールド

		MainWindowViewModel Data;

		#endregion フィールド


		#region コンストラクタ

		public MainWindow()
		{
			InitializeComponent();

			DispatcherHelper.UIDispatcher = this.Dispatcher;

			this.Data = new MainWindowViewModel(this);
			this.DataContext = this.Data;
		}

		#endregion コンストラクタ


		#region メソッド

		private void OnTextChanged(object sender, TextChangedEventArgs e)
		{
			var textBox = sender as TextBox;
			if (textBox != null)
				textBox.ScrollToLine(textBox.LineCount - 1);
		}

		private void ScrollOffsetButton_Click(object sender, RoutedEventArgs e)
		{
			this.Data.ListViewSampleContainerVertialOffset += 1.0;
		}

		#endregion メソッド

	}
}
