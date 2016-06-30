using Kumano.Contrib.Infrastructures;
using Kumano.Data.Service;
using Livet;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kumano.Data.Struction
{
	/// <summary>
	/// カテゴリ情報
	/// </summary>
	/// <remarks>
	/// アプリケーション用のデータ構造です。
	/// </remarks>
	public class CategoryTreeItem : Livet.NotificationObject,IDataExport<DataCategory>
	{

		#region フィールド

		/// <summary>
		/// カテゴリツリーの階層構造のための、子要素一覧コレクション
		/// </summary>
		/// <remarks>
		/// Halcyonが管理するタグDtaaModelの階層構造とは別です。
		/// この階層構造は、アプリケーションがツリーなどでタグ表示するための階層構造を構築します。
		/// </remarks>
		private readonly ObservableCollection<ServerCategoryTestData> _children = new ObservableCollection<ServerCategoryTestData>();

		/// <summary>
		/// ReadOnlyなDispatcherを使っているが、TreeListは遅延読み込みに対応していない
		/// </summary>
		private readonly ReadOnlyDispatcherCollection<CategoryTreeItem> _childrenro = null;

		/// <summary>
		/// カテゴリのID
		/// </summary>
		private long _Id;

		/// <summary>
		/// 子要素がサーバから取得済みか示すフラグ
		/// </summary>
		private bool _isLoadedChildren = false;

		private string _Name;
		ILog LOG = LogManager.GetLogger(typeof(CategoryTreeItem));

		#endregion フィールド


		#region コンストラクタ

		public CategoryTreeItem()
		{
			_childrenro = ViewModelHelper.CreateReadOnlyDispatcherCollection<ServerCategoryTestData, CategoryTreeItem>(
				_children,
				prop => new CategoryTreeItem
				{
					_Id = prop.Id,
					Name = prop.Name,
					HasChildServer = prop.IsChild.HasValue ? prop.IsChild.Value : false
				},
				DispatcherHelper.UIDispatcher);
		}

		#endregion コンストラクタ

		#region プロパティ

		/// <summary>
		/// TagTreeで表示する小階層の要素を取得します
		/// </summary>
		public ReadOnlyDispatcherCollection<CategoryTreeItem> Children
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

		public bool HasChildServer { get; set; }
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

		public DataCategory Export()
		{
			var immobj = new DataCategory()
			{
				Id = _Id,
				Name = _Name
			};
			return immobj;
		}

		public void Load(bool rootLoad = false)
		{
			// サーバからデータを取得してきた、というイメージ。
			// サーバからは、現在のタグの小階層タグの一覧とその小階層が更に小階層を持つかどうかのフラグを取得するような実装とする。
			try
			{
				if (rootLoad)
				{
					using (var proxy = new MogamiApiServiceClient())
					{
						var param = new REQUEST_LOADCATEGORY();
						var result = proxy.LoadCategory(param);
						foreach (var prop in result.Categories)
						{
							_children.Add(new ServerCategoryTestData
							{
								Id = prop.Id,
								IsChild = prop.IsHasChild,
								Name = prop.Name
							});
						}
					}
				}
				else
				{
					using (var proxy = new MogamiApiServiceClient())
					{
						var param = new REQUEST_LOADCATEGORY();
						param.TargetCategortId = this._Id;
						var result = proxy.LoadCategory(param);
						foreach (var prop in result.Categories)
						{
							_children.Add(new ServerCategoryTestData
							{
								Id = prop.Id,
								IsChild = prop.IsHasChild,
								Name = prop.Name
							});
						}
					}
				}
			}catch(Exception expr)
			{
				LOG.Warn(expr.Message);
			}
		}

		#endregion メソッド
	}

	class ServerCategoryTestData
	{


		#region プロパティ

		public long Id { get; set; }
		public bool? IsChild { get; set; }
		public string Name { get; set; }

		#endregion プロパティ

	}
}
