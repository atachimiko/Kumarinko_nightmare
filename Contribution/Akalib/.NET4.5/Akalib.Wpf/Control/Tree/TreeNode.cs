using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalib.Wpf.Control.Tree
{
	/// <summary>
	/// ツリーに表示する項目
	/// </summary>
	public class TreeNode : INotifyPropertyChanged
	{


		#region フィールド
		/// <summary>
		/// 子ノードのコレクション
		/// </summary>
		private Collection<TreeNode> _children;

		/// <summary>
		/// _childrenのコレクションが変化した場合のメッセージハンドラ
		/// </summary>
		private INotifyCollectionChanged _childrenSource;
		private int _index = -1;
		private bool _isExpanded;
		private bool _isSelected;
		private ReadOnlyCollection<TreeNode> _nodes;
		private TreeNode _parent;
		private object _tag;
		private TreeList _tree;

		#endregion フィールド


		#region コンストラクタ
		/// <summary>
		/// 
		/// </summary>
		/// <param name="tree">所属するツリーコントロール</param>
		/// <param name="tag">ノードに設定する任意の情報</param>
		internal TreeNode(TreeList tree, object tag)
		{
			if (tree == null)
				throw new ArgumentNullException("tree");

			_tree = tree;
			_children = new NodeCollection(this);
			_nodes = new ReadOnlyCollection<TreeNode>(_children);
			_tag = tag;
		}

		#endregion コンストラクタ


		#region イベント

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion イベント


		#region プロパティ

		public IEnumerable<TreeNode> AllVisibleChildren
		{
			get
			{
				int level = this.Level;
				TreeNode node = this;
				while (true)
				{
					node = node.NextVisibleNode;
					if (node != null && node.Level > level)
						yield return node;
					else
						break;
				}
			}
		}

		public bool HasChildren
		{
			get;
			internal set;
		}

		public int Index
		{
			get
			{
				return _index;
			}
		}

		public bool IsExpandable
		{
			get
			{
				return (HasChildren && !IsExpandedOnce) || Nodes.Count > 0;
			}
		}

		public bool IsExpanded
		{
			get { return _isExpanded; }
			set
			{
				if (value != IsExpanded)
				{
					Tree.SetIsExpanded(this, value);
					OnPropertyChanged("IsExpanded");
					OnPropertyChanged("IsExpandable");
				}
			}
		}

		public bool IsExpandedOnce
		{
			get;
			internal set;
		}

		public bool IsSelected
		{
			get { return _isSelected; }
			set
			{
				if (value != _isSelected)
				{
					_isSelected = value;
					OnPropertyChanged("IsSelected");
				}
			}
		}

		public int Level
		{
			get
			{
				if (_parent == null)
					return -1;
				else
					return _parent.Level + 1;
			}
		}

		public TreeNode NextNode
		{
			get
			{
				if (_parent != null)
				{
					int index = Index;
					if (index < _parent.Nodes.Count - 1)
						return _parent.Nodes[index + 1];
				}
				return null;
			}
		}

		public ReadOnlyCollection<TreeNode> Nodes
		{
			get { return _nodes; }
		}

		public TreeNode Parent
		{
			get { return _parent; }
		}

		public TreeNode PreviousNode
		{
			get
			{
				if (_parent != null)
				{
					int index = Index;
					if (index > 0)
						return _parent.Nodes[index - 1];
				}
				return null;
			}
		}

		/// <summary>
		/// ノードに設定されている任意の情報を取得します
		/// </summary>
		public object Tag
		{
			get { return _tag; }
		}

		public int VisibleChildrenCount
		{
			get
			{
				return AllVisibleChildren.Count();
			}
		}

		internal TreeNode BottomNode
		{
			get
			{
				TreeNode parent = this.Parent;
				if (parent != null)
				{
					if (parent.NextNode != null)
						return parent.NextNode;
					else
						return parent.BottomNode;
				}
				return null;
			}
		}

		internal Collection<TreeNode> Children
		{
			get { return _children; }
		}

		internal INotifyCollectionChanged ChildrenSource
		{
			get { return _childrenSource; }
			set
			{
				if (_childrenSource != null)
					_childrenSource.CollectionChanged -= ChildrenChanged;

				_childrenSource = value;

				if (_childrenSource != null)
					_childrenSource.CollectionChanged += ChildrenChanged;
			}
		}

		/// <summary>
		/// Returns true if all parent nodes of this node are expanded.
		/// </summary>
		internal bool IsVisible
		{
			get
			{
				TreeNode node = _parent;
				while (node != null)
				{
					if (!node.IsExpanded)
						return false;
					node = node.Parent;
				}
				return true;
			}
		}

		internal TreeNode NextVisibleNode
		{
			get
			{
				if (IsExpanded && Nodes.Count > 0)
					return Nodes[0];
				else
				{
					TreeNode nn = NextNode;
					if (nn != null)
						return nn;
					else
						return BottomNode;
				}
			}
		}

		internal TreeList Tree
		{
			get { return _tree; }
		}

		#endregion プロパティ


		#region メソッド

		public override string ToString()
		{
			if (Tag != null)
				return Tag.ToString();
			else
				return base.ToString();
		}

		internal void AssignIsExpanded(bool value)
		{
			_isExpanded = value;
		}

		void ChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					if (e.NewItems != null)
					{
						int index = e.NewStartingIndex;
						int rowIndex = Tree.Rows.IndexOf(this);
						foreach (object obj in e.NewItems)
						{
							Tree.InsertNewNode(this, obj, rowIndex, index);
							index++;
						}
					}
					break;

				case NotifyCollectionChangedAction.Remove:
					if (Children.Count > e.OldStartingIndex)
						RemoveChildAt(e.OldStartingIndex);
					break;

				case NotifyCollectionChangedAction.Move:
				case NotifyCollectionChangedAction.Replace:
				case NotifyCollectionChangedAction.Reset:
					while (Children.Count > 0)
						RemoveChildAt(0);
					Tree.CreateChildrenNodes(this);
					break;
			}
			HasChildren = Children.Count > 0;
			OnPropertyChanged("IsExpandable");
		}

		private void ClearChildrenSource(TreeNode node)
		{
			node.ChildrenSource = null;
			foreach (var n in node.Children)
				ClearChildrenSource(n);
		}

		private void OnPropertyChanged(string name)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(name));
		}

		private void RemoveChildAt(int index)
		{
			var child = Children[index];
			Tree.DropChildrenRows(child, true);
			ClearChildrenSource(child);
			Children.RemoveAt(index);
		}

		#endregion メソッド


		#region Classes

		private class NodeCollection : Collection<TreeNode>
		{


			#region フィールド

			private TreeNode _owner;

			#endregion フィールド

			#region コンストラクタ

			/// <summary>
			/// 
			/// </summary>
			/// <param name="owner">親ノード</param>
			public NodeCollection(TreeNode owner)
			{
				_owner = owner;
			}

			#endregion コンストラクタ


			#region メソッド

			protected override void ClearItems()
			{
				while (this.Count != 0)
					this.RemoveAt(this.Count - 1);
			}

			protected override void InsertItem(int index, TreeNode item)
			{
				if (item == null)
					throw new ArgumentNullException("item");

				if (item.Parent != _owner)
				{
					if (item.Parent != null)
						item.Parent.Children.Remove(item);
					item._parent = _owner;
					item._index = index;
					for (int i = index; i < Count; i++)
						this[i]._index++;
					base.InsertItem(index, item);
				}
			}

			protected override void RemoveItem(int index)
			{
				TreeNode item = this[index];
				item._parent = null;
				item._index = -1;
				for (int i = index + 1; i < Count; i++)
					this[i]._index--;
				base.RemoveItem(index);
			}

			protected override void SetItem(int index, TreeNode item)
			{
				if (item == null)
					throw new ArgumentNullException("item");
				RemoveAt(index);
				InsertItem(index, item);
			}

			#endregion メソッド

		}

		#endregion Classes

	}
}
