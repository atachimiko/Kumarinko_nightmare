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
	/// <summary>
	/// 
	/// </summary>
	public class CategoryTreeExplorerPaneViewModel : PaneViewModelBase
	{
		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(CategoryTreeExplorerPaneViewModel));
		private readonly CategoryTreeModel _CategoryTreeModel;
		private bool _IsBusy;
		private TreeNode _SelectedTreeListNode;

		private IList<TreeNode> _SelectedTreeListNodes;

		#endregion フィールド


		#region コンストラクタ

		public CategoryTreeExplorerPaneViewModel()
		{
			this.ContentId = "CategoryTreeExplorerPane";
			this.Title = "カテゴリ一覧";

			this._CategoryTreeModel = CategoryTreeModel.CreateInitializeTreeModel();
		}

		#endregion コンストラクタ


		#region プロパティ

		public CategoryTreeModel CategoryTreeData { get { return _CategoryTreeModel; } }

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

		#endregion プロパティ


		#region メソッド

		public void OnReload()
		{
			this.IsBusy = !this.IsBusy;
		}

		public void OutputLog(string message)
		{
			LOG.Info("OutputLog : " + message);

			if (this.SelectedTreeListNodes != null)
			{
				foreach (var node in SelectedTreeListNodes)
				{
					LOG.Debug("    Selected:" + node.Tag);
				}
			}
			else
			{
				LOG.Debug("    Selected: None");
			}

		}

		/// <summary>
		/// 画像一覧ペインの表示
		/// </summary>
		public async void ShowImageListDocument()
		{
			// すでに画像一覧ペインが表示済みかチェックする
			var message_2 = new FindDocumentPaneMessage(typeof(ArtifactNavigationListDocumentViewModel));
			await Messenger.RaiseAsync(message_2);

			if (message_2.Response == null)
			{
				var message_3 = new DoArtifactNavigationListPaneMessage();
				await Messenger.RaiseAsync(message_3);
			}
			else
			{
				var message_3 = new DoArtifactNavigationListPaneMessage();
				await Messenger.RaiseAsync(message_3);
			}
		}

		#endregion メソッド

		#region 内部クラス

		/// <summary>
		/// TreeListに表示するモデル
		/// </summary>
		public class CategoryTreeModel : ITreeModel
		{


			#region コンストラクタ

			public CategoryTreeModel()
			{
				this.Root = new CategoryTreeItem();
			}

			#endregion コンストラクタ


			#region プロパティ

			public CategoryTreeItem Root { get; private set; }

			#endregion プロパティ


			#region メソッド

			public static CategoryTreeModel CreateInitializeTreeModel()
			{
				var model = new CategoryTreeModel();
				model.Root.Load(true);
				return model;
			}

			public System.Collections.IEnumerable GetChildren(object parent)
			{
				if (parent == null)
				{
					return Root.Children;
				}
				else
				{
					var item = parent as CategoryTreeItem;
					var c = (item as CategoryTreeItem).Children;
					return c;
				}
			}

			public bool HasChildren(object parent)
			{
				return (parent as CategoryTreeItem).HasChildServer;
			}

			#endregion メソッド

		}

		#endregion 内部クラス

	}
}
