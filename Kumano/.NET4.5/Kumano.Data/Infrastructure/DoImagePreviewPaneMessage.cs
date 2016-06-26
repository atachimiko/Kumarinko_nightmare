using Kumano.Data.ViewModel;
using Livet.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kumano.Data.Infrastructure
{
	/// <summary>
	/// ImagePreviewペインの表示・操作を行います
	/// </summary>
	public class DoImagePreviewPaneMessage : InteractionMessage
	{


		#region コンストラクタ

		public DoImagePreviewPaneMessage()
		{
			this.MessageKey = "DoImagePreview";
		}

		#endregion コンストラクタ


		#region プロパティ

		/// <summary>
		/// プレビューViewで表示する画像情報
		/// </summary>
		public LoadImageInfo LoadImageInfo { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool IsWithActive { get; set; }

		#endregion プロパティ

		#region メソッド

		/// <summary>
		/// 派生クラスでは必ずオーバーライドしてください。Freezableオブジェクトとして必要な実装です。<br/>
		/// 通常このメソッドは、自身の新しいインスタンスを返すように実装します。
		/// </summary>
		/// <returns>自身の新しいインスタンス</returns>
		protected override System.Windows.Freezable CreateInstanceCore()
		{
			var cpy = new DoImagePreviewPaneMessage();

			cpy.MessageKey = this.MessageKey;
			cpy.LoadImageInfo = this.LoadImageInfo;
			cpy.IsWithActive = this.IsWithActive;

			return cpy;
		}

		#endregion メソッド

	}
}
