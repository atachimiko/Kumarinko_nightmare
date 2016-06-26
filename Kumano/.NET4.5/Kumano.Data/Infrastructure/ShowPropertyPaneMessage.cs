using Akalib.Wpf.Dock;
using Kumano.Data.Construction;
using Livet.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kumano.Data.Infrastructure
{
	/// <summary>
	/// プロパティペインの表示を行うメッセージです
	/// </summary>
	public class ShowPropertyPaneMessage : ResponsiveInteractionMessage<IPropertyPaneItem>
	{

		#region フィールド

		public static readonly DependencyProperty ShowPropertyPaneItemTypeProperty =
			DependencyProperty.Register("ShowPropertyPaneItemType", typeof(ShowPropertyPaneItemType), typeof(ShowPropertyPaneMessage), new PropertyMetadata());

		#endregion フィールド

		#region コンストラクタ

		/// <summary>
		/// 
		/// </summary>
		public ShowPropertyPaneMessage()
			: base("ShowPropertyPane")
		{

		}

		#endregion コンストラクタ

		#region プロパティ

		/// <summary>
		/// 表示するプロパティペインの種類を取得、または設定します。
		/// </summary>
		public ShowPropertyPaneItemType ShowPropertyPaneItemType
		{
			get { return (ShowPropertyPaneItemType)GetValue(ShowPropertyPaneItemTypeProperty); }
			set { SetValue(ShowPropertyPaneItemTypeProperty, value); }
		}

		#endregion プロパティ

		#region メソッド

		/// <summary>
		/// 派生クラスでは必ずオーバーライドしてください。Freezableオブジェクトとして必要な実装です。<br/>
		/// 通常このメソッドは、自身の新しいインスタンスを返すように実装します。
		/// </summary>
		/// <returns>自身の新しいインスタンス</returns>
		protected override System.Windows.Freezable CreateInstanceCore()
		{
			var cpy = new ShowPropertyPaneMessage();
			cpy.MessageKey = this.MessageKey;
			return cpy;
		}

		#endregion メソッド
	}
}
