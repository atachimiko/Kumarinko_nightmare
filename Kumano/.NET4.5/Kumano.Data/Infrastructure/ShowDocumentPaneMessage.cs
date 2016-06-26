using Akalib.Wpf.Dock;
using Livet.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kumano.Data.Infrastructure
{
	public enum ShowDocumentPaneType
	{
		/// <summary>
		/// 画像一覧
		/// </summary>
		DocumentPaneImageList,

		/// <summary>
		/// 画像プレビュー
		/// </summary>
		DocumentPanePreviewImage,
	}

	/// <summary>
	/// 
	/// </summary>
	public class ShowDocumentPaneMessage : ResponsiveInteractionMessage<IDocumentPaneViewModel>
	{
		#region フィールド

		public static readonly DependencyProperty ActiveDocumentProperty =
			DependencyProperty.Register("ActiveDocument", typeof(IDocumentPaneViewModel), typeof(ShowDocumentPaneMessage), new PropertyMetadata());

		public static readonly DependencyProperty ShowDocumentPaneTypeProperty =
			DependencyProperty.Register("ShowDocumentPaneType", typeof(ShowDocumentPaneType), typeof(ShowDocumentPaneMessage), new PropertyMetadata());

		#endregion フィールド


		#region コンストラクタ

		public ShowDocumentPaneMessage()
			: base("ShowDocumentPane")
		{

		}

		/// <summary>
		/// ドキュメント部に表示する内容を指定してインスタンスを作成するコンストラクタ
		/// </summary>
		/// <param name="showDocumentPaneType"></param>
		public ShowDocumentPaneMessage(ShowDocumentPaneType showDocumentPaneType)
			: this("ShowDocumentPane")
		{
			this.ShowDocumentPaneType = showDocumentPaneType;
		}

		/// <summary>
		/// 表示したいドキュメントペインのViewModelを使ったコンストラクタ
		/// </summary>
		public ShowDocumentPaneMessage(IDocumentPaneViewModel activeDocument)
			: this("ShowDocumentPane")
		{
			this.ActiveDocument = activeDocument;
		}

		private ShowDocumentPaneMessage(string messageKey)
			: base(messageKey)
		{

		}

		#endregion コンストラクタ


		#region プロパティ

		public IDocumentPaneViewModel ActiveDocument
		{
			get { return (IDocumentPaneViewModel)GetValue(ActiveDocumentProperty); }
			set { SetValue(ActiveDocumentProperty, value); }
		}

		public ShowDocumentPaneType ShowDocumentPaneType
		{
			get { return (ShowDocumentPaneType)GetValue(ShowDocumentPaneTypeProperty); }
			set { SetValue(ShowDocumentPaneTypeProperty, value); }
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
			var cpy = new ShowDocumentPaneMessage(this.ShowDocumentPaneType);
			cpy.MessageKey = this.MessageKey;
			return cpy;
		}

		#endregion メソッド

	}
}
