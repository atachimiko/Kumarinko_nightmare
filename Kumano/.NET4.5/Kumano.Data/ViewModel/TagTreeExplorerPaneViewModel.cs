using Akalib.Wpf.Control.Tree;
using Akalib.Wpf.Mvvm;
using Kumano.Data.Infrastructure;
using Kumano.Data.Struction;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kumano.Data.ViewModel
{
	public class TagTreeExplorerPaneViewModel : PaneViewModelBase
	{


		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(TagTreeExplorerPaneViewModel));

		private readonly TagTreeModel _TagTreeData;

		private TagTreeExplorerPaneContextMenuViewModel _ContextMenuInfo = new TagTreeExplorerPaneContextMenuViewModel();

		bool _IsBusy;

		private bool _IsContextMenuOpened;

		TreeNode _SelectedTreeListNode;

		private IList<TreeNode> _SelectedTreeListNodes;

		#endregion フィールド


		#region コンストラクタ

		public TagTreeExplorerPaneViewModel()
		{
			this.ContentId = "TagTreeExplorerPane";
			this.Title = "タグ一覧";

			this._TagTreeData = TagTreeModel.CreateInitializeTreeModel();
		}

		#endregion コンストラクタ

		#region プロパティ

		/// <summary>
		/// コンテキストメニューとバインドするインスタンスを取得します
		/// </summary>
		public TagTreeExplorerPaneContextMenuViewModel ContextMenuInfo
		{
			get
			{ return _ContextMenuInfo; }
			protected set
			{
				if (_ContextMenuInfo == value)
					return;
				_ContextMenuInfo = value;
				RaisePropertyChanged();
			}
		}

		public bool IsBusy
		{
			get
			{ return _IsBusy; }
			set
			{
				if (_IsBusy == value)
					return;
				_IsBusy = value;
				RaisePropertyChanged();
			}
		}

		public bool IsContextMenuOpened
		{
			get
			{ return _IsContextMenuOpened; }
			set
			{
				if (_IsContextMenuOpened == value)
					return;
				_IsContextMenuOpened = value;
				RaisePropertyChanged();
			}
		}

		public TreeNode SelectedTreeListNode
		{
			get
			{ return _SelectedTreeListNode; }
			set
			{
				if (_SelectedTreeListNode == value)
					return;
				_SelectedTreeListNode = value;
				RaisePropertyChanged();
			}
		}

		public IList<TreeNode> SelectedTreeListNodes
		{
			get
			{ return _SelectedTreeListNodes; }
			set
			{
				if (_SelectedTreeListNodes == value)
					return;
				_SelectedTreeListNodes = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// TreeListに表示するタグツリー項目を取得します
		/// </summary>
		public TagTreeModel TagTreeData { get { return _TagTreeData; } }

		#endregion プロパティ

		#region メソッド

		/// <summary>
		/// 画像一覧ペインの表示
		/// </summary>
		public async void ShowImageListDocument()
		{
			// すでに画像一覧ペインが表示済みかチェックする
			var message_2 = new FindDocumentPaneMessage(typeof(ArtifactNavigationListDocumentViewModel));
			await Messenger.RaiseAsync(message_2);

			var message_3 = new DoArtifactNavigationListPaneMessage();

			var item = this.SelectedTreeListNode.Tag as TagTreeItem;
			message_3.FindByTagId = item.TagId;

			if (message_2.Response == null)
			{
				await Messenger.RaiseAsync(message_3);
			}
			else
			{
				await Messenger.RaiseAsync(message_3);
			}
		}

		public void UpdateContextMenuStatus()
		{
			var selected_count = this.SelectedTreeListNodes.Count;
			if (selected_count == 0)
			{
				this.ContextMenuInfo.IsEnableMenu_Rename = false;
			}
			else if (selected_count == 1)
			{
				this.ContextMenuInfo.IsEnableMenu_Rename = true;
			}
			else if (selected_count > 0)
			{
				this.ContextMenuInfo.IsEnableMenu_Rename = false;
			}
		}

		#endregion メソッド

		#region 内部クラス

		/// <summary>
		/// タグツリーのコンテキストメニューの表示状態
		/// </summary>
		public class TagTreeExplorerPaneContextMenuViewModel : Livet.NotificationObject
		{
			// TOOD: XAMLへのバインディングを実装する


			#region フィールド

			private bool _IsEnableMenu_AppendList;

			private bool _IsEnableMenu_Delete;

			private bool _IsEnableMenu_Move;

			private bool _IsEnableMenu_Rename;

			private bool _IsEnableMenu_ReplaceList;

			#endregion フィールド

			#region プロパティ

			/// <summary>
			/// メニューの「現在の一覧に追加」の有効無効を設定する
			/// </summary>
			public bool IsEnableMenu_AppendList
			{
				get
				{ return _IsEnableMenu_AppendList; }
				set
				{
					if (_IsEnableMenu_AppendList == value)
						return;
					_IsEnableMenu_AppendList = value;
					RaisePropertyChanged();
				}
			}

			/// <summary>
			/// メニューの「削除」の有効無効を設定する
			/// </summary>
			public bool IsEnableMenu_Delete
			{
				get
				{ return _IsEnableMenu_Delete; }
				set
				{
					if (_IsEnableMenu_Delete == value)
						return;
					_IsEnableMenu_Delete = value;
					RaisePropertyChanged();
				}
			}

			/// <summary>
			/// メニューの「タグの移動」の有効無効を設定する
			/// </summary>
			public bool IsEnableMenu_Move
			{
				get
				{ return _IsEnableMenu_Move; }
				set
				{
					if (_IsEnableMenu_Move == value)
						return;
					_IsEnableMenu_Move = value;
					RaisePropertyChanged();
				}
			}

			/// <summary>
			/// メニューの「名前の変更」の有効無効を設定する
			/// </summary>
			public bool IsEnableMenu_Rename
			{
				get
				{ return _IsEnableMenu_Rename; }
				set
				{
					if (_IsEnableMenu_Rename == value)
						return;
					_IsEnableMenu_Rename = value;
					RaisePropertyChanged();
				}
			}

			/// <summary>
			/// メニューの「現在の一覧を置き換え」の有効無効を設定する
			/// </summary>
			public bool IsEnableMenu_ReplaceList
			{
				get
				{ return _IsEnableMenu_ReplaceList; }
				set
				{
					if (_IsEnableMenu_ReplaceList == value)
						return;
					_IsEnableMenu_ReplaceList = value;
					RaisePropertyChanged();
				}
			}

			#endregion プロパティ

		}
		/// <summary>
		/// TagデータをTagTreeで表示するためのデータラッピングクラス
		/// </summary>
		public class TagTreeModel : ITreeModel
		{


			#region フィールド

			private TagTreeItem _Root;

			#endregion フィールド


			#region コンストラクタ

			public TagTreeModel()
			{
				this.Root = new TagTreeItem();
			}

			#endregion コンストラクタ


			#region プロパティ

			public TagTreeItem Root
			{
				get { return _Root; }
				private set { _Root = value; }
			}

			#endregion プロパティ


			#region メソッド

			public static TagTreeModel CreateInitializeTreeModel()
			{
				var model = new TagTreeModel();
				model.Root.Load(true);
				return model;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="parent"></param>
			/// <returns></returns>
			public System.Collections.IEnumerable GetChildren(object parent)
			{
				if (parent == null)
				{
					return Root.Children;
				}
				else
				{
					var item = parent as TagTreeItem;
					var c = (item as TagTreeItem).Children;
					return c;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="parent"></param>
			/// <returns></returns>
			public bool HasChildren(object parent)
			{
				return (parent as TagTreeItem).HasChildServer;
			}

			#endregion メソッド

		}

		#endregion 内部クラス

	}
}
