using Akalib.Wpf.Dock;
using Livet;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalib.Wpf.Mvvm
{
	public abstract class WorkspaceViewModelBase : DocumentViewModelBase
	{
		#region フィールド

		/// <summary>
		/// アンカーペインに表示するViewModel
		/// </summary>
		/// <remarks>
		/// アンカーペインは表示するデータの入れ替えをオブジェクト単位では行わないので、
		/// 1度作成したViewModelオブジェクトを使い回す。そのため、アンカーペインに表示するViewModelのコレクションは
		/// 配列でよい。
		/// </remarks>
		protected readonly ObservableSynchronizedCollection<IAnchorPaneViewModel> _anchors = new ObservableSynchronizedCollection<IAnchorPaneViewModel>();

		protected readonly DispatcherCollection<IDocumentPaneViewModel> _Contents = new DispatcherCollection<IDocumentPaneViewModel>(DispatcherHelper.UIDispatcher);

		static ILog LOG = LogManager.GetLogger(typeof(WorkspaceViewModelBase));

		private IPaneViewModel _activePane = null;

		private IDocumentPaneViewModel _LastSelectedDocumentPane;

		private ReadOnlyDispatcherCollection<IDocumentPaneViewModel> _readonlyContents;

		#endregion フィールド

		#region プロパティ

		/// <summary>
		///
		/// </summary>
		public IPaneViewModel ActivePane
		{
			get { return _activePane; }
			set
			{
				//if (_activePane == value)
				//	return;
				//_activePane = value;
				//RaisePropertyChanged();

				IPaneViewModel oldActivePane = _activePane;
				if (oldActivePane != null)
				{
					//oldActivePane.IsActive = false;
					//oldActivePane.IsSelected = false;
				}

				_activePane = value;

				if (_activePane != null)
				{
					//_activePane.IsActive = true;
					//_activePane.IsSelected = true;

					OnActivePanePropertyChanged();
					RaisePropertyChanged(() => ActivePane);

					if (oldActivePane != null)
						oldActivePane.ActiveChanged();
					_activePane.ActiveChanged();
				}
				else
				{
					OnActivePanePropertyChanged();
					RaisePropertyChanged(() => ActivePane);

					if (oldActivePane != null)
						oldActivePane.ActiveChanged();
				}
			}
		}

		public ObservableSynchronizedCollection<IAnchorPaneViewModel> AnchorContents
		{
			get
			{
				return _anchors;
			}
		}

		/// <summary>
		/// ドッキングパネル(AvalonDock)に表示中のペイン一覧を取得します
		/// </summary>
		public DispatcherCollection<IDocumentPaneViewModel> Contents
		{
			get
			{
				return _Contents;
			}
		}

		/// <summary>
		/// 最後に選択したドキュメントペインを取得します
		/// </summary>
		public IDocumentPaneViewModel LastSelectedDocumentPane
		{
			get
			{ return _LastSelectedDocumentPane; }
			protected set
			{
				if (_LastSelectedDocumentPane == value)
					return;
				_LastSelectedDocumentPane = value;

				OnLastSelectedDocumentPane();
				RaisePropertyChanged();
			}
		}

		#endregion プロパティ

		#region メソッド

		/// <summary>
		/// closeのDisposeは呼び出しません。
		/// </summary>
		/// <param name="close"></param>
		public void Close(IDocumentPaneViewModel close)
		{
			LOG.Debug("Close");

			this._Contents.Remove(close);

			OnClosed(close);
		}

		/// <summary>
		///     コンテンツペインに同一のContentIdが存在するかどうか探します。
		/// </summary>
		/// <param name="pane"></param>
		/// <returns>
		/// ContentIdが同一のドキュメントペインがあった場合はそのオブジェクトを返します。
		/// 見つからない場合はNullを返します。
		/// </returns>
		public virtual IDocumentPaneViewModel ContainsContent(string contentId)
		{
			var r = from u in _Contents
					where u.ContentId == contentId
					select u;
			var vm2 = r.FirstOrDefault();
			return vm2; // 見つからなかった場合はNull
		}

		/// <summary>
		///     コンテンツペインに同一のContentIdが存在するかどうか探します。
		/// </summary>
		/// <param name="pane"></param>
		/// <returns>
		/// ContentIdが同一のドキュメントペインがあった場合はそのオブジェクトを返します。
		/// 見つからない場合はNullを返します。
		/// </returns>
		public virtual IDocumentPaneViewModel ContainsContent(IDocumentPaneViewModel pane)
		{
			var r = from u in _Contents
					where u.ContentId == pane.ContentId
					select u;
			var vm2 = r.FirstOrDefault();
			return vm2; // 見つからなかった場合はNull
		}

		/// <summary>
		///     ドキュメントペインにドキュメントを表示します。
		/// </summary>
		/// <returns>目的のドキュメントペインが見つからない場合はNullを返します。</returns>
		public virtual IDocumentPaneViewModel ShowDocument(string contentId)
		{
			var n = ContainsContent(contentId);
			if (n == null)
			{
				return null;
			}

			this.ActivePane = n;
			return n;
		}

		/// <summary>
		///     ドキュメントペインにドキュメントを表示します
		/// </summary>
		public virtual IDocumentPaneViewModel ShowDocument(IDocumentPaneViewModel document)
		{
			var n = ContainsContent(document);
			if (n == null)
			{
				_Contents.Add(document);
				n = document;
			}

			this.ActivePane = n;
			return n;
		}

		protected virtual void OnActivePanePropertyChanged()
		{
			// 最後に選択したペインがコンテンツペインの場合、そのインスタンスをキャッシュ
			if (this.ActivePane is IDocumentPaneViewModel)
			{
				this.LastSelectedDocumentPane = _activePane as IDocumentPaneViewModel;
			}
			else
			{
				if (this.ActivePane == null)
					this.LastSelectedDocumentPane = null;
			}
		}

		protected virtual void OnClosed(IDocumentPaneViewModel close)
		{
		}

		/// <summary>
		///
		/// </summary>
		protected virtual void OnLastSelectedDocumentPane()
		{
		}

		#endregion メソッド

	}
}
