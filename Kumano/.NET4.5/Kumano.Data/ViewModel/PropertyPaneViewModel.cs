using Akalib.Wpf.Mvvm;
using Kumano.Core;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kumano.Data.ViewModel
{
	/// <summary>
	/// プロパティペインのViewMode
	/// </summary>
	public class PropertyPaneViewModel : PaneViewModelBase
	{

		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(PropertyPaneViewModel));

		#endregion フィールド


		#region コンストラクタ

		public PropertyPaneViewModel()
		{
			this.ContentId = "PropertyPane";
			this.Title = "プロパティ";
		}

		#endregion コンストラクタ

		#region メソッド

		/// <summary>
		/// 
		/// </summary>
		protected override void OnActiveContentChanged()
		{
			if (this.ActiveContent != null)
			{

				// ドキュメントのクラス名と同じDataTemplateをMainWindowリソースから探す
				// ※MainWindow.xamlに、プロパティペインで表示するテンプレートをDataTemplateで定義します。
				var name = this.ActiveContent.GetType().Name;
				this.ViewTemplate = ApplicationContext.MainWindow.FindResource(name) as DataTemplate;
			}
			else
			{
				this.ViewTemplate = null;
			}
		}

		#endregion メソッド
	}
}
