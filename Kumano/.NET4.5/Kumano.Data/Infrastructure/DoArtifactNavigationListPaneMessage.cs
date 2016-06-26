using Livet.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kumano.Data.Infrastructure
{
	/// <summary>
	/// 
	/// </summary>
	public class DoArtifactNavigationListPaneMessage : InteractionMessage
	{
		#region コンストラクタ

		public DoArtifactNavigationListPaneMessage()
		{
			this.MessageKey = "DoImageListPane";
		}

		#endregion コンストラクタ

		#region メソッド

		/// <summary>
		/// 派生クラスでは必ずオーバーライドしてください。Freezableオブジェクトとして必要な実装です。<br/>
		/// 通常このメソッドは、自身の新しいインスタンスを返すように実装します。
		/// </summary>
		/// <returns>自身の新しいインスタンス</returns>
		protected override System.Windows.Freezable CreateInstanceCore()
		{
			var cpy = new DoArtifactNavigationListPaneMessage();
			cpy.MessageKey = this.MessageKey;
			return cpy;
		}

		#endregion メソッド
	}
}
