using Livet;
using Akalib.Wpf.Mvvm;
using Kumano.Contrib.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kumano.Core;

namespace Kumano.Data.ViewModel
{
	/// <summary>
	/// 
	/// </summary>
	public class AttachTagItemPaneViewModel : PaneViewModelBase
	{


		#region フィールド

		ObservableSynchronizedCollection<AttachTagListItem> _Items;
		AttachTagListItem _SelectedItem;

		#endregion フィールド


		#region コンストラクタ

		public AttachTagItemPaneViewModel()
		{
			this.ContentId = "AttachTagItemPane";
			this.Title = "タグ設定";
			this._Items = new ObservableSynchronizedCollection<AttachTagListItem>();

			ApplicationContext.Event.LoadedDeviceSetting += OnEvent_LoadedDeviceSetting;
		}

		#endregion コンストラクタ

		#region プロパティ

		/// <summary>
		/// コントロールに表示する項目一覧を取得します
		/// </summary>
		public ObservableSynchronizedCollection<AttachTagListItem> Items
		{
			get { return _Items; }
		}

		/// <summary>
		/// コントロールで選択中のアイテム
		/// </summary>
		public AttachTagListItem SelectedItem
		{
			get
			{ return _SelectedItem; }
			set
			{
				if (_SelectedItem == value)
					return;
				_SelectedItem = value;
				RaisePropertyChanged();
			}
		}

		#endregion プロパティ


		#region メソッド

		/// <summary>
		/// デバイス設定情報イベント発生時
		/// </summary>
		private void OnEvent_LoadedDeviceSetting()
		{
			this._Items.Add(new AttachTagListItem { Label = "Test1" });
			this._Items.Add(new AttachTagListItem { Label = "Test2" });
			this._Items.Add(new AttachTagListItem { Label = "Test4" });
		}

		#endregion メソッド

		#region 内部クラス

		/// <summary>
		/// 
		/// </summary>
		public sealed class AttachTagListItem : ItemDataBase
		{


			#region フィールド

			private string _Label;

			#endregion フィールド


			#region プロパティ

			public string Label
			{
				get { return _Label; }
				set
				{
					if (_Label == value) return;
					_Label = value;
				}
			}

			#endregion プロパティ


			#region メソッド

			public override void LoadedFromData(ILazyLoadingItem loadedData)
			{
				// EMPTY
			}

			public void OutputLog()
			{
				Console.WriteLine(Label + " をクリックしました");
			}

			#endregion メソッド

		}

		#endregion 内部クラス

	}

}
