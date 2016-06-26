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
	/// <summary>
	/// ドキュメント部に表示しているペイン一覧を取得するメッセージ
	/// </summary>
	public class FindDocumentPaneMessage : ResponsiveInteractionMessage<IList<IDocumentPaneViewModel>>
	{

		#region フィールド

		public static readonly DependencyProperty DocumentPaneViewModelClazzProperty =
			DependencyProperty.Register("DocumentPaneViewModelClazz", typeof(Type), typeof(FindDocumentPaneMessage), new PropertyMetadata());

		#endregion フィールド

		#region コンストラクタ

		/// <summary>
		///     検索したいドキュメントペインの型情報で初期化を行うコンストラクタです。
		/// </summary>
		/// <param name="documentPaneViewModelClazz"></param>
		public FindDocumentPaneMessage(Type documentPaneViewModelClazz)
			: this("FindDocumentPane")
		{
			this.DocumentPaneViewModelClazz = documentPaneViewModelClazz;
		}

		public FindDocumentPaneMessage()
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="messageKey"></param>
		private FindDocumentPaneMessage(string messageKey)
			: base(messageKey)
		{

		}

		#endregion コンストラクタ


		#region プロパティ

		public Type DocumentPaneViewModelClazz
		{
			get { return (Type)GetValue(DocumentPaneViewModelClazzProperty); }
			set { SetValue(DocumentPaneViewModelClazzProperty, value); }
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
			var cpy = new FindDocumentPaneMessage(this.DocumentPaneViewModelClazz);
			cpy.MessageKey = this.MessageKey;
			return cpy;
		}

		#endregion メソッド

	}
}
