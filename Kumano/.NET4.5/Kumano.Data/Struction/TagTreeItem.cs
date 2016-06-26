using Livet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kumano.Data.Struction
{
	/// <summary>
	/// タグ情報
	/// </summary>
	/// <remarks>
	/// クライアントアプリケーションが使用するタグのデータ情報です。
	/// Halcyonのタグとは別物です。
	/// </remarks>
	public class TagTreeItem : Livet.NotificationObject
	{

		#region フィールド

		/// <summary>
		/// DataModeコレクション
		/// </summary>
		private readonly ObservableCollection<ServerTagTestData> _children = new ObservableCollection<ServerTagTestData>();

		/// <summary>
		/// タグの階層構造のための、子要素一覧コレクション
		/// </summary>
		/// <remarks>
		/// Halcyonが管理するタグDtaaModelの階層構造とは別です。
		/// この階層構造は、アプリケーションがツリーなどでタグ表示するための階層構造を構築します。
		/// </remarks>
		private readonly ReadOnlyDispatcherCollection<TagTreeItem> _childrenro = null;
		/// <summary>
		/// 子要素がサーバから取得済みか示すフラグ
		/// </summary>
		private bool _isLoadedChildren = false;

		/// <summary>
		/// タグ名称
		/// </summary>
		private string _Name;

		#endregion フィールド


		#region コンストラクタ

		public TagTreeItem()
		{
			this._childrenro = ViewModelHelper.CreateReadOnlyDispatcherCollection<ServerTagTestData, TagTreeItem>(
				_children,
				prop => new TagTreeItem { Name = prop.Name, HasChildServer = prop.IsChild },
				DispatcherHelper.UIDispatcher);
		}

		#endregion コンストラクタ

		#region プロパティ

		/// <summary>
		/// TagTreeで表示する小階層の要素を取得します
		/// </summary>
		public ReadOnlyDispatcherCollection<TagTreeItem> Children
		{
			get
			{
				if (!_isLoadedChildren)
				{
					_isLoadedChildren = true;
					if (HasChildServer)
						Load();
				}

				return _childrenro;
			}
		}

		/// <summary>
		/// 子要素があることをサーバから受けている場合、Trueを返す。
		/// </summary>
		public bool HasChildServer { get; set; }

		/// <summary>
		/// ラベルに表示されるラベル文字列
		/// </summary>
		public string Name
		{
			get
			{ return _Name; }
			set
			{
				if (_Name == value)
					return;
				_Name = value;
				RaisePropertyChanged();
			}
		}

		#endregion プロパティ


		#region メソッド

		public void Load(bool rootLoad = false)
		{
			// サーバからデータを取得してきた、というイメージ。
			// サーバからは、現在のタグの小階層タグの一覧とその小階層が更に小階層を持つかどうかのフラグを取得するような実装とする。

			if (rootLoad)
			{
				for (int i = 0; i < 10; i++)
				{
					var serverdata = new ServerTagTestData();
					serverdata.Name = "Tag" + i;
					serverdata.IsChild = false;

					if (i == 2) serverdata.IsChild = true;

					_children.Add(serverdata);
				}
			}
			else
			{
				for (int i = 0; i < 10; i++)
				{
					var serverdata = new ServerTagTestData();
					serverdata.Name = "SubTag" + i;
					serverdata.IsChild = false;

					_children.Add(serverdata);
				}
			}

		}

		#endregion メソッド
	}

	/// <summary>
	/// サーバからデータを取得するという実装例で使用するデータ
	/// </summary>
	class ServerTagTestData
	{

		#region プロパティ

		public bool IsChild { get; set; }
		public string Name { get; set; }

		#endregion プロパティ
	}
}
