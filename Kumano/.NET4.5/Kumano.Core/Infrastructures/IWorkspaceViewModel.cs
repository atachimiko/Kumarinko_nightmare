using Akalib.Wpf.Dock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kumano.Core.Infrastructures
{
	public interface IWorkspaceViewModel
	{

		#region メソッド

		/// <summary>
		/// ドキュメント部に表示しているViewModel一覧を取得する
		/// </summary>
		/// <param name="type">取得するViewModel</param>
		/// <returns></returns>
		IEnumerable<IDocumentPaneViewModel> FindDocumentPane(Type clazz);

		/// <summary>
		/// プロパティペイン内に表示するアイテムのViewModelを設定します
		/// </summary>
		/// <param name="itemViewModel"></param>
		void SetPropertyPaneItem(IPropertyPaneItem itemViewModel);

		/// <summary>
		/// 
		/// </summary>
		IDocumentPaneViewModel ShowDocument(IDocumentPaneViewModel document);

		/// <summary>
		/// プロパティペインの表示を行います
		/// </summary>
		void ShowPropertyPane();

		#endregion メソッド

	}
}
