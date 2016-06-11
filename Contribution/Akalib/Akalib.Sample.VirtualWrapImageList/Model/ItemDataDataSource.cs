using Livet;
using Livet.EventListeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Akalib.Sample.VirtualWrapImageList.Model
{
	/// <summary>
	/// ItemDataモデルのデータソース
	/// </summary>
	public class ItemDataDataSource : IDataSource<DispatcherCollection<ItemData>>, IDisposable
	{
		public ItemDataDataSource()
		{
			this._Items = new DispatcherCollection<ItemData>(DispatcherHelper.UIDispatcher);
		}

		/// <summary>
		/// 新しいItemDataを追加する
		/// </summary>
		/// <param name="item"></param>
		public void AddItem(ItemData item)
		{
			var listener = new EventListener<EventHandler>(
				h => item.Loaded += h,
				h => item.Loaded -= h,
				(sender, e) => { OnLazyDataLoad((ItemData)sender); }
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

		DispatcherCollection<ItemData> _Items;
		public DispatcherCollection<ItemData> Items { get { return _Items; } }

		ObservableSynchronizedCollection<EventListener<EventHandler>> _Listeners = new ObservableSynchronizedCollection<EventListener<EventHandler>>();

		async void OnLazyDataLoad(ItemData sender)
		{
			//Console.WriteLine("OnPropertyChangedIsLoaded " + sender.IdText);

			await DispatcherHelper.UIDispatcher.InvokeAsync(async () =>
			{
				var results = await this.GetData();

				sender.Label = results[0].Label;
				sender.Icon = results[0].Icon;
			});
		}

		// ネットワークの向こうのデータをとってくるイメージ
		private async Task<List<ItemData>> GetData()
		{
			var @rnd = new Random();
			return await Task.Delay(@rnd.Next(1000, 10000))
				.ContinueWith(_ =>
				{
					List<ItemData> l = new List<ItemData>();

					var r = new ItemData { Label = DateTime.Now.ToString() };
					var path = @"Assets/Penguins.jpg";
					r.Icon = BitmapFrame.Create(new Uri(path, UriKind.Relative));

					l.Add(r);

					return l;
				});
		}
	}
}
