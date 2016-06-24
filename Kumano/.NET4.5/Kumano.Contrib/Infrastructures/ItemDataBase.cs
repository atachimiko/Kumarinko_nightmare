using Livet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kumano.Contrib.Infrastructures
{
	public abstract class ItemDataBase : NotificationObject, ILazyLoadingItem
	{

		#region フィールド

		// IsLoadedプロパティとバインドするための添付プロパティ
		public static readonly DependencyProperty LoadedProperty =
			DependencyProperty.RegisterAttached(
				"Loaded",
				typeof(bool),
				typeof(ItemDataBase),
				new PropertyMetadata(false));

		private bool _IsLoaded = false;

		#endregion フィールド

		#region イベント

		/// <summary>
		/// IsLoadedプロパティが読み込まれたときに発生するイベント
		/// </summary>
		public event EventHandler Loaded;

		#endregion イベント


		#region プロパティ

		public bool IsLoaded
		{
			get
			{
				if (!_IsLoaded)
					OnLoaded();
				_IsLoaded = true;
				return true;
			}
		}

		#endregion プロパティ

		#region メソッド

		/// <summary>
		/// 遅延ロード処理
		/// </summary>
		/// <param name="loadedData"></param>
		public abstract void LoadedFromData(ILazyLoadingItem loadedData);

		/// <summary>
		/// 
		/// </summary>
		public virtual void Unload()
		{
			if (this._IsLoaded)
			{
				this._IsLoaded = false;
			}
		}

		private void OnLoaded()
		{
			var h = this.Loaded;
			if (h != null)
			{
				h(this, EventArgs.Empty);
			}
		}

		#endregion メソッド
	}
}
