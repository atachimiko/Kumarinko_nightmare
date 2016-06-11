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
	/// TreeListで使用しているコレクション
	/// 幾つかの便利な処理が実装されたメソッドを提供している。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ObservableCollectionAdv<T> : ObservableCollection<T>
	{

		#region メソッド
		/// <summary>
		/// 複数の項目をコレクションに追加します
		/// </summary>
		/// <param name="index"></param>
		/// <param name="collection"></param>
		public void InsertRange(int index, IEnumerable<T> collection)
		{
			this.CheckReentrancy();
			var items = this.Items as List<T>;
			items.InsertRange(index, collection);
			OnReset();
		}

		/// <summary>
		/// 指定した範囲の項目をコレクションから削除します
		/// </summary>
		/// <param name="index"></param>
		/// <param name="count"></param>
		public void RemoveRange(int index, int count)
		{
			this.CheckReentrancy();
			var items = this.Items as List<T>;
			items.RemoveRange(index, count);
			OnReset();
		}

		private void OnPropertyChanged(string propertyName)
		{
			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		private void OnReset()
		{
			OnPropertyChanged("Count");
			OnPropertyChanged("Item[]");
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(
				NotifyCollectionChangedAction.Reset));
		}

		#endregion メソッド
	}
}
