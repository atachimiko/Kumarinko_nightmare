using Akalib.Sample.VirtualWrapImageList.Model;
using Livet;
using System;
using System.Timers;
using System.Windows.Controls.Primitives;

namespace Akalib.Sample.VirtualWrapImageList
{
	public class MainWindowViewModel:NotificationObject
	{


		#region フィールド

		ItemDataDataSource _ItemDataDataSource;

		private double _ListViewSampleContainerVertialOffset;

		MainWindow _MainWindow;

		private string _Message;

		Timer _ScrollingTimer;

		Timer _Timer;

		double lastScrollVerticalOffset = 0.0;

		#endregion フィールド


		#region コンストラクタ

		public MainWindowViewModel(MainWindow mainWindow)
		{
			this._MainWindow = mainWindow;
			this._Timer = new Timer();
			this._Timer.Interval = 100;
			this._Timer.Elapsed += OnTimer_Elapsed;

			this._ScrollingTimer = new Timer();
			this._ScrollingTimer.Interval = 1000;
			this._ScrollingTimer.Elapsed += OnScrollingTimer_Elapsed;
			this._ScrollingTimer.AutoReset = false;

			this._ItemDataDataSource = new ItemDataDataSource();

			// 初期データ
			for (int i = 0; i < 30; i++)
			{
				this._ItemDataDataSource.AddItem(new ItemData { IdText = "Id=" + i, Label = "初期値" + i });
			}

			_MainWindow.ListViewSampleContainer.ItemsSource = this._ItemDataDataSource.Items;

		}

		#endregion コンストラクタ


		#region プロパティ

		public double ListViewSampleContainerVertialOffset
		{
			get
			{ return _ListViewSampleContainerVertialOffset; }
			set
			{
				Console.WriteLine("ListViewSampleContainerVertialOffset.Value = {0}", value);
				if (_ListViewSampleContainerVertialOffset == value)
					return;
				_ListViewSampleContainerVertialOffset = value;
				RaisePropertyChanged();
			}
		}

		public string Message
		{
			get
			{ return _Message; }
			set
			{
				if (_Message == value)
					return;
				_Message = value;
				RaisePropertyChanged();
			}
		}

		#endregion プロパティ

		#region メソッド

		/// <summary>
		/// 
		/// </summary>
		public void AddItem()
		{
			int pos = this._ItemDataDataSource.Items.Count + 1;
			this._ItemDataDataSource.AddItem(new ItemData { Label = "追加 " + pos });
			/*
			if (ListViewSampleContainerVertialOffset == lastScrollVerticalOffset)
				ListViewSampleContainerVertialOffset = lastScrollVerticalOffset + 0.1;
			else
				ListViewSampleContainerVertialOffset = lastScrollVerticalOffset;
			*/
		}

		/// <summary>
		/// 表示中のメッセージをクリアします
		/// </summary>
		public void MessageClear()
		{
			this.Message = string.Empty;
		}

		public void Scrolling()
		{
			this._ScrollingTimer.Stop();
			this._ScrollingTimer.Start();

			//var @scroll = this._MainWindow.ListViewSampleContainer.GetScrollViewer();

			//lastScrollVerticalOffset = @scroll.VerticalOffset;
			MessageClear();
			//ShowGeneratorPosition();
		}

		/// <summary>
		/// GeneratorPositionの状態を出力します
		/// </summary>
		public void ShowGeneratorPosition()
		{
			this.Message += _MainWindow.ListViewSampleContainer.GetMessage("Show GeneratorPosition");
		}

		async void OnScrollingTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			Console.WriteLine("OnScrollingTimer_Elapsed");

			IItemContainerGenerator generator = this._MainWindow.ListViewSampleContainer.ItemContainerGenerator;
			for (int index = 0; index < this._MainWindow.ListViewSampleContainer.Items.Count; index++)
			{
				GeneratorPosition position = generator.GeneratorPositionFromIndex(index);

				if (position.Offset != 0)
				{
					dynamic d = this._MainWindow.ListViewSampleContainer.Items[index];
					d.Unload();
				}
			}


			await DispatcherHelper.UIDispatcher.InvokeAsync(() =>
			{
				ShowGeneratorPosition();
			});
		}
		
		void OnTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			AddItem();
		}

		#endregion メソッド

	}
}
