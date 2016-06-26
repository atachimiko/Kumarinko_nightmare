using Akalib.Wpf.Dock;
using Livet;
using Livet.Commands;
using Livet.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Windows;
using Akalib.Wpf.Mvvm.Attribute;
using System.Windows.Data;

namespace Akalib.Wpf.Mvvm
{
	/// <summary>
	/// AvalonDockを使用したMVVMアーキテクチャで、ドキュメントコンテナのViewModelクラスの基本クラスとして使用します。
	/// </summary>
	/// <remarks>
	/// このクラスを継承する場合は、一緒にIDocumentPaneViewModelインターフェースのインプリメントが必要です。
	/// </remarks>
	public abstract class DocumentViewModelBase : ViewModel
	{

		#region フィールド

		/// <summary>
		///     OnLoadedが初めて呼び出されたかどうかのフラグです
		/// </summary>
		protected bool IsOnLoaded = false;

		/// <summary>
		///     OnLoadedの処理中かどうかを示すフラグ
		/// </summary>
		protected bool IsOnLoading = false;

		private static ILog LOG = LogManager.GetLogger(typeof(DocumentViewModelBase));

		private ViewModelCommand _CloseCommand;

		private bool _EnabledZoomControl;

		private List<Action> _InitializeActions = new List<Action>();

		private bool _IsActive = false;

		private string _Title;

		private string _ToolTip;

		#endregion フィールド


		#region プロパティ
		[ContentProperty]
		public ViewModelCommand CloseCommand
		{
			get
			{
				if (_CloseCommand == null)
				{
					_CloseCommand = new ViewModelCommand(Close);
				}
				return _CloseCommand;
			}

			set
			{
				_CloseCommand = value;
				RaisePropertyChanged(() => CloseCommand);
			}
		}

		public bool EnabledZoomControl
		{
			get
			{ return _EnabledZoomControl; }
			set
			{
				if (_EnabledZoomControl == value)
					return;
				_EnabledZoomControl = value;
				RaisePropertyChanged();
			}
		}

		[ContentProperty]
		public System.Windows.Media.ImageSource IconSource
		{
			get;
			set;
		}

		[ContentProperty(BindingMode = BindingMode.TwoWay)]
		public bool IsActive
		{
			get { return _IsActive; }
			set
			{
				_IsActive = value;
				RaisePropertyChanged(() => IsActive);
			}
		}

		[ContentProperty(BindingMode = BindingMode.TwoWay)]
		public bool IsSelected
		{
			get;
			set;
		}

		[ContentProperty]
		public string Title
		{
			get { return this._Title; }
			set
			{
				this._Title = value;
				RaisePropertyChanged(() => Title);
			}
		}

		[ContentProperty]
		public string ToolTip
		{
			get { return _ToolTip; }
			set
			{
				if (Equals(_ToolTip, value)) return;
				this._ToolTip = value;
			}
		}

		/// <summary>
		///     初回表示時のみ呼び出す処理
		/// </summary>
		protected List<Action> InitializeActions { get { return _InitializeActions; } }

		#endregion プロパティ


		#region メソッド

		public abstract void Close();

		/// <summary>
		///     [UI]
		/// </summary>
		/// <remarks>
		/// コンテナが表示される度に行いたい処理を実装します。
		/// InitializeActionsコレクションの処理は初めてコンテナが表示される際にしか呼び出しません。
		/// IsOnLoadedフラグをチェックして初めてのOnLoadedメソッド呼び出しかどうかを調べることができます。
		/// </remarks>
		public void OnLoaded()
		{
			this.IsOnLoading = true;
			if (!IsOnLoaded)
			{
				foreach (var prop in _InitializeActions)
				{
					prop.Invoke();
				}
			}

			OnPrviewLoaded();

			this.IsOnLoading = false;
			this.IsOnLoaded = true;
		}

		protected virtual void OnPrviewLoaded()
		{
		}

		protected void ShowMessageBox(string message)
		{
			var information = new InformationMessage(message, "エラー", "NotifyDialog");
			Messenger.Raise(information);
		}

		#endregion メソッド

	}
}
