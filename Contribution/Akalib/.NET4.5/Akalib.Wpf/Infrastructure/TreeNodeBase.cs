using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalib.Wpf.Infrastructure
{
	/// <summary>
	///
	/// </summary>
	public abstract class TreeNodeBase : Livet.NotificationObject
	{
		#region Protected Fields

		protected TreeNodeFolder _ParentTreeFolder;

		#endregion Protected Fields

		#region Private Fields

		private bool _IsExpanded;

		private bool _IsSelected;

		private string _NodeName;

		#endregion Private Fields

		#region Public Constructors

		public TreeNodeBase()
		{
		}

		public TreeNodeBase(TreeNodeFolder parent)
		{
			if (parent != null)
			{
				parent.AddChild(this);
			}
			_ParentTreeFolder = parent;
		}

		#endregion Public Constructors

		#region Public Properties

		public bool IsExpanded
		{
			get
			{ return _IsExpanded; }
			set
			{
				_IsExpanded = value;
				RaisePropertyChanged(() => IsExpanded);

				if (_IsExpanded && _ParentTreeFolder != null) // 設定値がTrueの場合のみ
					_ParentTreeFolder.IsExpanded = value;

				// Expandedがtrueの場合のみ展開時の処理を実行する
				if (value)
				{
					OnExpanded();
				}
			}
		}

		public bool IsSelected
		{
			get
			{ return _IsSelected; }
			set
			{
				if (_IsSelected == value)
					return;
				_IsSelected = value;
				RaisePropertyChanged(() => IsSelected);

				// Selectedがtrueの場合のみ展開時の処理を実行する
				if (value)
				{
					OnSelected();
				}
			}
		}

		public string NodeName
		{
			get
			{ return GetNodeNameAware(); }
			set
			{
				if (_NodeName == value)
					return;
				_NodeName = value;
				RaisePropertyChanged(() => NodeName);
			}
		}

		public string NodePath
		{
			get
			{
				if (Parent != null) return Parent.NodePath + "/" + NodeName;
				return NodeName;
			}
		}
		
		public TreeNodeFolder Parent
		{
			get { return _ParentTreeFolder; }
			set
			{
				// 新しい親ノードの設定
				var old = _ParentTreeFolder;
				if (old != null)
				{
					old.RemoveChild(this);
				}

				if (value != null)
				{
					_ParentTreeFolder = null;
					value.AddChild(this);
				}
				_ParentTreeFolder = value;
			}
		}

		#endregion Public Properties

		#region Internal Methods

		internal void ParentClear()
		{
			_ParentTreeFolder = null;
		}

		#endregion Internal Methods

		#region Protected Methods

		protected virtual string GetNodeNameAware()
		{
			return _NodeName;
		}

		/// <summary>
		/// Expandedが開くときのみ呼び出します
		/// </summary>
		protected virtual void OnExpanded()
		{
		}

		/// <summary>
		///
		/// </summary>
		protected virtual void OnSelected()
		{
		}

		#endregion Protected Methods
	}
}
