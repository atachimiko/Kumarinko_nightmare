using Akalib.Wpf.Mvvm;
using Livet.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kumano.Data.Infrastructure
{
	public class DialogMessage : ResponsiveInteractionMessage<string>
	{
		#region コンストラクタ

		//Viewでメッセージインスタンスを生成する時のためのコンストラクタ
		public DialogMessage()
		{
		}

		/// <summary>
		/// ViewModelからMessenger経由での発信目的でメッセージインスタンスを生成するためのコンストラクタ
		/// </summary>
		/// <param name="dialogViewModel"></param>
		public DialogMessage(IDialogViewModel dialogViewModel)
			: this(dialogViewModel, "Dialog")
		{
		}

		/*
		 * メッセージに保持させたい情報をプロパティとして定義してください。
		 * Viewでバインド可能なプロパティにするために依存関係プロパティとして定義する事をお勧めします。
		 * 通常依存関係プロパティはコードスニペット propdpを使用して定義します。
		 * もし普通のプロパティとして定義したい場合はコードスニペット propを使用して定義します。
		 */

		/// <summary>
		/// ViewModelからMessenger経由での発信目的でメッセージインスタンスを生成するためのコンストラクタ
		/// </summary>
		/// <param name="dialogViewModel">表示するダイアログに設定するViewModel</param>
		/// <param name="messageKey"></param>
		public DialogMessage(IDialogViewModel dialogViewModel, string messageKey)
			: base(messageKey)
		{
			this.ViewModel = dialogViewModel;
		}

		#endregion コンストラクタ


		#region プロパティ

		public bool ModerlessDialog { get; set; }

		public IDialogViewModel ViewModel { get; set; }

		#endregion プロパティ

		#region メソッド

		/// <summary>
		/// 派生クラスでは必ずオーバーライドしてください。Freezableオブジェクトとして必要な実装です。<br/>
		/// 通常このメソッドは、自身の新しいインスタンスを返すように実装します。
		/// </summary>
		/// <returns>自身の新しいインスタンス</returns>
		protected override System.Windows.Freezable CreateInstanceCore()
		{
			var a = new DialogMessage(ViewModel, MessageKey);
			a.ModerlessDialog = this.ModerlessDialog;
			return a;
		}

		#endregion メソッド
	}
}
