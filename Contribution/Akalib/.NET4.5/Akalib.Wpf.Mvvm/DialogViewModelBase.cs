using Livet;
using Livet.Messaging.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalib.Wpf.Mvvm
{
	/// <summary>
	///
	/// </summary>
	/// <remarks>
	/// ダイアログの「×」ボタンではダイアログはクローズしません。
	/// 「×」ボタンが押されると、CloseCanceledメソッドが呼び出されるのでサブクラスではCloseCanceledメソッドをオーバーライドし、
	/// 閉じてもいい場合はRequestClose()またはRequestCancel()を呼び出します。
	/// </remarks>
	public abstract class DialogViewModelBase : ViewModel, IDialogViewModel
	{
		#region Private Fields

		private bool _CanClose = false;

		private string _ErrorMessageText;

		private bool _IsCanceled = true;

		#endregion Private Fields

		#region Public Constructors

		public DialogViewModelBase()
		{
			CanClose = true;
		}

		#endregion Public Constructors

		#region Public Properties

		/// <summary>
		/// ダイアログを閉じるかどうかのフラグ
		/// </summary>
		/// <remarks>
		/// ダイアログを閉じたい場合は、このプロパティをTrueにセットし、WindowsAction.Closeを呼び出してください。
		/// </remarks>
		public bool CanClose
		{
			get
			{
				return _CanClose;
			}
			set
			{
				if (EqualityComparer<bool>.Default.Equals(_CanClose, value))
					return;
				_CanClose = value;
				RaisePropertyChanged(() => CanClose);
			}
		}

		/// <summary>
		/// エラーテキストを取得、または設定します。
		/// </summary>
		/// <remarks>
		/// このプロパティの使用は任意です。
		/// このプロパティを使ってエラーメッセージを表示するようにUIを設計してもよいですし、
		/// プロパティそのものを使用せずに設計してもかまいません。
		/// </remarks>
		public string ErrorMessageText
		{
			get
			{ return _ErrorMessageText; }
			set
			{
				if (_ErrorMessageText == value)
					return;
				_ErrorMessageText = value;
				RaisePropertyChanged(() => ErrorMessageText);
			}
		}

		public bool IsCanceled
		{
			get
			{ return _IsCanceled; }
			set
			{
				if (_IsCanceled == value)
					return;
				_IsCanceled = value;
				RaisePropertyChanged("IsCanceled");
			}
		}

		#endregion Public Properties

		#region Public Methods

		/// <summary>
		/// 閉じるボタンによるクローズがキャンセルされた場合の処理をオーバーライドで実装してください
		/// </summary>
		public virtual void CloseCanceled()
		{
		}

		public async virtual void InitRendered()
		{
			// EMPTY
		}

		/// <summary>
		/// キャンセルフラグをセットしてダイアログを閉じます
		/// </summary>
		public void RequestCancel()
		{
			if (OnRequestCancel())
			{
				CanClose = true;
				IsCanceled = true;
				Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));
			}
		}

		/// <summary>
		/// ButtonコントロールなどのClickイベントで明示的にメソッドを呼び出します
		/// </summary>
		public void RequestClose()
		{
			if (OnRequestClose())
			{
				CanClose = true;
				IsCanceled = false;
				Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));
			}
		}

		#endregion Public Methods

		#region Protected Methods

		protected virtual bool OnRequestCancel()
		{
			return true;
		}

		/// <summary>
		/// ダイアログを閉じる場合は、Trueを返すように処理をオーバーライドしてください。
		/// </summary>
		/// <returns></returns>
		protected virtual bool OnRequestClose()
		{
			return true;
		}

		#endregion Protected Methods
	}
}
