using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalib.Wpf.Dock
{
	public interface IDocumentPaneViewModel : IPaneViewModel, INotifyPropertyChanged
	{

		/// <summary>
		/// 
		/// </summary>
		bool EnabledZoomControl { get; }
		/// <summary>
		/// プロパティペインに表示するアイテム
		/// 何も表示をしない場合はNullを返します。
		/// </summary>
		IPropertyPaneItem PropertyPaneItem { get; }
	}
}
