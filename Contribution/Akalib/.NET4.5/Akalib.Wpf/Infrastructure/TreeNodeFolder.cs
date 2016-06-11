using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Akalib.Wpf.Infrastructure
{
	/// <summary>
	///
	/// </summary>
	public class TreeNodeFolder : TreeNodeBase
	{
		#region Protected Fields
		
		/// <summary>
		/// 子ノードのコレクション
		/// </summary>
		protected ObservableCollection<object> _Children = new ObservableCollection<object>();

		#endregion Protected Fields

		#region Private Fields

		/// <summary>
		/// ソートに使用する比較関数
		/// </summary>
		private IComparer sorter = null;

		#endregion Private Fields

		#region Public Constructors

		public TreeNodeFolder()
		{
		}

		public TreeNodeFolder(TreeNodeFolder parent)
			: base(parent)
		{
		}

		#endregion Public Constructors

		#region Public Properties

		public ReadOnlyCollection<object> Children
		{
			get
			{
				if (_Children == null || _Children.Count == 0) return new ReadOnlyCollection<object>(new List<object>());
				return new ReadOnlyCollection<object>(_Children);
			}
		}

		public IComparer ChildrenSort
		{
			set
			{
				sorter = value;
				System.Collections.ArrayList.Adapter(_Children).Sort(sorter);
				RiseChildUpdate();
			}
			get { return sorter; }
		}

		#endregion Public Properties

		#region Public Methods

		/// <summary>
		/// 子要素の現在の末尾にセパレータアイテムを追加する
		/// </summary>
		public void AddSeparator()
		{
			this._Children.Add(new Separator());
		}

		public void RiseChildUpdate()
		{
			RaisePropertyChanged(() => Children);
		}

		/// <summary>
		/// 子ノードの並べ替えを行います。
		/// 並べ替えを行うには、前もってsorterを設定してある必要があります。
		/// </summary>
		public void SortChildren()
		{
			if (sorter != null)
			{
				// 項目展開の度に、ソートを実行する
				System.Collections.ArrayList.Adapter(_Children).Sort(sorter);
				RiseChildUpdate();
			}
		}

		#endregion Public Methods

		#region Internal Methods

		internal void AddChild(TreeNodeBase node)
		{
			if (node.Parent != null) throw new ApplicationException();
			_Children.Add(node);
			RiseChildUpdate();
		}

		internal void RemoveChild(TreeNodeBase node)
		{
			if (_Children.Contains(node))
			{
				_Children.Remove(node);
				node.Parent = null;
				RiseChildUpdate();
			}
		}

		#endregion Internal Methods

		#region Protected Methods

		protected void ClearChild()
		{
			if (_Children == null) return;

			foreach (var node in _Children)
			{
				if (node is TreeNodeBase)
					((TreeNodeBase)node).ParentClear();
			}

			_Children.Clear();
			RiseChildUpdate();
		}

		/// <summary>
		///
		/// </summary>
		protected override void OnExpanded()
		{
			base.OnExpanded();
			SortChildren();
		}

		#endregion Protected Methods
	}
}
