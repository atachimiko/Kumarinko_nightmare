using Livet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Akalib.Sample.VirtualWrapImageList.Model
{
	public class ItemData : NotificationObject, ILazyLoadingItem
	{
		public ItemData()
		{
			this.Label = "Loading...";
			LoadDefaultIcon();
		}

		/// <summary>
		/// 
		/// </summary>
		public void Unload()
		{
			if (this._IsLoaded)
			{
				Console.WriteLine("Unload Id:{0}", this.IdText);
				LoadDefaultIcon();

				this._IsLoaded = false;
			}
		}

		#region [プロパティ]
		#region Label変更通知プロパティ
		private string _Label;

		public string Label
		{
			get
			{ return _Label; }
			set
			{
				if (_Label == value)
					return;
				_Label = value;
				RaisePropertyChanged();
			}
		}
		#endregion


		#region IdText変更通知プロパティ
		private string _IdText;

		public string IdText
		{
			get
			{ return _IdText; }
			set
			{
				if (_IdText == value)
					return;
				_IdText = value;
				RaisePropertyChanged();
			}
		}
		#endregion


		#region Icon変更通知プロパティ
		private BitmapSource _Icon;

		public BitmapSource Icon
		{
			get
			{ return _Icon; }
			set
			{
				if (_Icon == value)
					return;
				_Icon = value;
				RaisePropertyChanged();
			}
		}
		#endregion

		/// <summary>
		/// 画面に表示されたことを検知するためのプロパティ
		/// </summary>
		/// <remarks>
		/// XAMLでバインディングが呼び出された場合にgetプロパティが呼び出されます。
		/// </remarks>
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
		private bool _IsLoaded = false;
		#endregion


		/// <summary>
		/// IsLoadedプロパティが読み込まれたときに発生するイベント
		/// </summary>
		public event EventHandler Loaded;

		private void OnLoaded()
		{
			var h = this.Loaded;
			if (h != null)
			{
				h(this, EventArgs.Empty);
			}
		}

		// IsLoadedプロパティとバインドするための添付プロパティ
		public static readonly DependencyProperty LoadedProperty =
			DependencyProperty.RegisterAttached(
				"Loaded",
				typeof(bool),
				typeof(ItemData),
				new PropertyMetadata(false));

		public static bool GetLoaded(DependencyObject obj)
		{
			return (bool)obj.GetValue(LoadedProperty);
		}

		public static void SetLoaded(DependencyObject obj, bool value)
		{
			obj.SetValue(LoadedProperty, value);
		}

		private void LoadDefaultIcon()
		{
			var path = @"Assets/Desert.jpg";
			this.Icon = BitmapFrame.Create(new Uri(path, UriKind.Relative));
		}
	}
}
