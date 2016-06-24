using Livet;
using Livet.EventListeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kumano.Contrib.Infrastructures
{
	public class LazyDataSource<T> : IDataSource<DispatcherCollection<T>>, IDisposable
			where T : ILazyLoadingItem, new()
	{


		#region フィールド

		DispatcherCollection<T> _Items;

		ObservableSynchronizedCollection<EventListener<EventHandler>> _Listeners = new ObservableSynchronizedCollection<EventListener<EventHandler>>();

		#endregion フィールド


		#region コンストラクタ

		public LazyDataSource()
		{
			this._Items = new DispatcherCollection<T>(DispatcherHelper.UIDispatcher);
		}

		#endregion コンストラクタ


		#region プロパティ

		public DispatcherCollection<T> Items { get { return _Items; } }

		#endregion プロパティ

		#region メソッド

		/// <summary>
		/// 新しいItemDataを追加する
		/// </summary>
		/// <param name="item"></param>
		public void AddItem(T item)
		{
			var listener = new EventListener<EventHandler>(
				h => item.Loaded += h,
				h => item.Loaded -= h,
				(sender, e) => { OnLazyDataLoad((T)sender); }
			);
			_Listeners.Add(listener);

			this.Items.Insert(this.Items.Count, item);
		}

		public void Dispose()
		{
			foreach (var @d in _Listeners)
			{
				@d.Dispose();
			}
		}

		/// <summary>
		/// 遅延ロードによりデータを取得するタイミングで呼び出される非同期メソッド
		/// 
		/// 実装部では非同期タスクにより別タスク・別スレッドを使用した処理を行い、UIスレッドへのパフォーマンスを維持しながら
		/// データの取得を行うことができます。
		/// 
		/// ※ネットワークの向こうのデータをとってくるイメージ
		/// </summary>
		/// <returns></returns>
		protected virtual async Task<T> GetData()
		{
			return await Task.Delay(0).ContinueWith(_ =>
			{
				return new T();
			});
		}

		async void OnLazyDataLoad(T sender)
		{
			//Console.WriteLine("OnPropertyChangedIsLoaded " + sender.IdText);

			await DispatcherHelper.UIDispatcher.InvokeAsync(async () =>
			{
				var results = await this.GetData();

				sender.LoadedFromData(results);

			});
		}

		#endregion メソッド

	}
}
