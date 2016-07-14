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


		#region フィールド

		private IDocumentPaneViewModel _ActiveContent;

		private DataTemplate _ViewTemplate;

		#endregion フィールド


		#region コンストラクタ

		public NavigationDocumentViewModel()
		{
			UpdateActiveViewTemplate();
		}

		#endregion コンストラクタ


		#region プロパティ

		/// <summary>
		/// 表示するコンポーネントのViewModelを取得、または設定します
		/// </summary>
		public IDocumentPaneViewModel ActiveContent
		{
			get { return _ActiveContent; }
			set
			{
				if (_ActiveContent == value) return;

				_ActiveContent = value;
				OnActiveContent();
				RaisePropertyChanged();
			}
		}

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

		protected virtual void OnActiveContent()
		{
			UpdateActiveViewTemplate();
		}

		/// <summary>
		/// 表示するDataTemplateを更新します
		/// </summary>
		private void UpdateActiveViewTemplate()
		{
			if (this.ActiveContent == null)
			{
				this.ViewTemplate = null;
			}
			else
			{
				var name = this.ActiveContent.GetType().Name;
				this.ViewTemplate = ApplicationContext.MainWindow.FindResource(name) as DataTemplate;
			}
		}

		#endregion メソッド

	}
}
