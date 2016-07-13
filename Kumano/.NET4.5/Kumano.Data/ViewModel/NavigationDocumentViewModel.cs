using Akalib.Wpf.Dock;
using Akalib.Wpf.Mvvm;
using Kumano.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kumano.Data.ViewModel
{
	/// <summary>
	/// 
	/// </summary>
	public class NavigationDocumentViewModel : DocumentViewModelBase, IDocumentPaneViewModel
	{
		public NavigationDocumentViewModel()
		{
			var name = "ArtifactNavigationListDocumentViewModel";
			this.ViewTemplate = ApplicationContext.MainWindow.FindResource(name) as DataTemplate;

		}

		#region フィールド

		private DataTemplate _ViewTemplate;

		#endregion フィールド


		#region プロパティ

		public string ContentId
		{
			get
			{
				return "NavigationDocument";
			}
		}

		public IPropertyPaneItem PropertyPaneItem
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>
		/// ドキュメントに表示するテンプレートを取得します。
		/// </summary>
		public DataTemplate ViewTemplate
		{
			get
			{
				return _ViewTemplate;
			}
			protected set
			{
				_ViewTemplate = value;
				RaisePropertyChanged(() => ViewTemplate);
			}
		}

		#endregion プロパティ


		#region メソッド

		public void ActiveChanged()
		{

		}

		public override void Close()
		{

		}

		#endregion メソッド

	}
}
