using Akalib.Wpf.Dock;
using Akalib.Wpf.Mvvm;
using Kumano.Contrib.Infrastructures;
using Kumano.Core;
using Kumano.Data.Construction;
using Kumano.Data.Infrastructure;
using Kumano.Data.Service;
using Livet;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace Kumano.Data.ViewModel
{
	public class ArtifactNavigationListDocumentViewModel : DocumentViewModelBase, IDocumentPaneViewModel
	{
		double _ListViewSampleContainerVertialOffset;
		public double ListViewSampleContainerVertialOffset
		{
			get
			{ return _ListViewSampleContainerVertialOffset; }
			set
			{
				LOG.InfoFormat("ListViewSampleContainerVertialOffset.Value = {0}", value);
				if (_ListViewSampleContainerVertialOffset == value)
					return;
				_ListViewSampleContainerVertialOffset = value;
				RaisePropertyChanged();
			}
		}

		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(ArtifactNavigationListDocumentViewModel));

		/// <summary>
		/// 画像リスト。遅延読み込み対応データ
		/// </summary>
		LazyDataSource<ImageListLazyItem> _Images = new LazyDataSource<ImageListLazyItem>();

		/// <summary>
		/// ビジーフラグ
		/// </summary>
		private bool _IsBusy;

		long _LoadCategoryId;

		long _LoadTagId;

		ListView _ScrollingListView;

		/// <summary>
		/// 
		/// </summary>
		Timer _ScrollingTimer;
		private ImageListLazyItem _SelectedItem;

		private bool IsInitializedContainer = false;

		bool IsListLoaded = false;

		#endregion フィールド


		#region コンストラクタ

		public ArtifactNavigationListDocumentViewModel()
		{
			this.Title = "画像一覧";

			this._ScrollingTimer = new Timer();
			this._ScrollingTimer.Interval = 1000;
			this._ScrollingTimer.Elapsed += OnScrollingTimer_Elapsed;
			this._ScrollingTimer.AutoReset = false;
		}

		#endregion コンストラクタ


		#region プロパティ

		public string ContentId
		{
			get { return "ImageListDocument"; }
		}

		public bool IsBusy
		{
			get
			{ return _IsBusy; }
			set
			{ 
				if (_IsBusy == value)
					return;
				_IsBusy = value;
				RaisePropertyChanged();
			}
		}

		public DispatcherCollection<ImageListLazyItem> Items { get { return _Images.Items; } }

		public long LoadCategoryId
		{
			get
			{
				return _LoadCategoryId;
			}
			set
			{
				if (value == 0) return;

				_LoadCategoryId = value;
				IsInitializedContainer = true;
				LoadServiceImageList();
			}
		}

		public long LoadTagId
		{
			get
			{
				return _LoadTagId;
			}
			set
			{
				if (value == 0) return;

				_LoadTagId = value;
				IsInitializedContainer = true;
				LoadServiceImageList();
			}
		}

		public IPropertyPaneItem PropertyPaneItem
		{
			get { return null; }
		}

		public ImageListLazyItem SelectedItem
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

		public void ActiveChanged()
		{
			LOG.DebugFormat("Active ImageListDocumentViewModel IsActive={0}", this.IsActive);

			// プロパティペインへの表示を行います
			if (this.IsActive)
				UpdatePropertyPane();
			else
				UpdatePropertyPane(true);
		}

		public override void Close()
		{
			((WorkspaceViewModel)ApplicationContext.Workspace).Close(this);
		}

		public void LoadComponentListView(ListView listView)
		{
			this._ScrollingListView = listView;
			IsInitializedContainer = true;
		}

		// 画像リストの読み込みを、最初の一度だけ実行するようにする
		
		/// <summary>
		/// サーバーから画像一覧を取得する(async)
		/// </summary>
		/// <remarks>
		/// 指定した条件に一致する画像一覧をサーバーから取得します。
		/// </remarks>
		public async void LoadServiceImageList()
		{
			LOG.InfoFormat("LoadServiceImageList IsListLoaded={0}", IsListLoaded);
			if (IsListLoaded) return;
			
			if (!IsInitializedContainer) return;

			REQUEST_FINDARTIFACT param = null;

			if (_LoadCategoryId != 0L)
			{
				param = new REQUEST_FINDARTIFACT();
				param.TargetType = FINDTARGET_SELECTOR.CATEGORY;
				param.TargetId = _LoadCategoryId;
				_LoadCategoryId = 0L;
			}
			else
			if(_LoadTagId != 0L)
			{
				param = new REQUEST_FINDARTIFACT();
				param.TargetType = FINDTARGET_SELECTOR.TAG;
				param.TargetId = _LoadTagId;
				_LoadTagId = 0L;
			}

			if (param == null) return;

			IsListLoaded = true;
			this.IsBusy = true;

			this._Images.Items.Clear();

			using (var proxy = new MogamiApiServiceClient())
			{
				proxy.Login();
				
				RESPONSE_FINDARTIFACT result = await proxy.FindArtifactAsync(param);
				foreach (var prop in result.Artifacts)
				{
					this._Images.AddItem(new ImageListLazyItem
					{
						IdText = prop.Id.ToString(),
						ArtifactId = prop.Id,
						Label = prop.Title,
						ThumbnailKey = prop.ThumbnailKey
					});
				}
			}

			this.IsBusy = false;
			this.IsListLoaded = false;
		}

		public void OutputLog(string message)
		{
			LOG.Info("OutputLog : " + message);
			if (this.SelectedItem != null)
				LOG.InfoFormat("   Selected={0}", this.SelectedItem.Label);
		}

		/// <summary>
		/// 
		/// </summary>
		public async void ReloadImageList()
		{
			this.IsListLoaded = false;
			LoadServiceImageList();
		}
		public void Scrolling()
		{
			this._ScrollingTimer.Stop();
			this._ScrollingTimer.Start();
		}

		/// <summary>
		/// 選択中の画像のプレビューペインを表示する
		/// </summary>
		public async void ShowSelectedImagePreview()
		{
			LOG.Info("ShowSelectedImagePreview");
			
			if (this.SelectedItem == null)
			{
				LOG.Info("選択している画像がありません。");
				return;
			}

			var message = new DoImagePreviewPaneMessage();
			message.IsWithActive = true;

			using (var proxy = new MogamiApiServiceClient())
			{
				var request = new REQUEST_LOADARTIFACT();
				request.TargetArtifactId = SelectedItem.ArtifactId;

				var rsp = await proxy.LoadArtifactAsync(request);
	
				message.LoadImageInfo = new LoadImageInfo
				{
					BitmapFilePath = rsp.FilePath
				};

				await Messenger.RaiseAsync(message);
			}

			
		}

		/// <summary>
		/// プロパティペインに表示している、画像詳細情報を更新します。
		/// </summary>
		public void UpdatePropertyPane(bool isClear = false)
		{
			if (!isClear)
			{
				// PropertyPaneに表示しているViewModelを取得します。
				var message = new ShowPropertyPaneMessage()
				{
					ShowPropertyPaneItemType = ShowPropertyPaneItemType.ImageListProperty
				};
				Messenger.Raise(message);

				var vm = message.Response as ArtifactNavigationListPropertyViewModel;
				if (vm != null)
				{
					// TODO: プロパティ用のViewModelに画像リストの情報を書き込む
				}
			}
			else
			{
				var message = new ShowPropertyPaneMessage()
				{
					ShowPropertyPaneItemType = ShowPropertyPaneItemType.Clear
				};
				Messenger.Raise(message);
			}
		}

		async void OnScrollingTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			try
			{
				if (_ScrollingListView != null)
				{
					IItemContainerGenerator generator = _ScrollingListView.ItemContainerGenerator;
					for (int index = 0; index < _ScrollingListView.Items.Count; index++)
					{
						GeneratorPosition position = generator.GeneratorPositionFromIndex(index);
						if (position.Offset != 0)
						{
							dynamic d = _ScrollingListView.Items[index];
							d.Unload();
						}
					}
				}
			}
			catch (Exception expr)
			{
				LOG.Warn("イベントハンドラ内でのエラー");
			}
		}

		#endregion メソッド

	}


	/// <summary>
	/// 遅延読み込みサポート
	/// </summary>
	public class ImageListLazyItem : ItemDataBase
	{


		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(ImageListLazyItem));

		private long _ArtifactId;

		private string _IdText;

		private string _Label;

		private BitmapSource _Thumbnail;

		private string _ThumbnailKey;

		/// <summary>
		/// 画像のキャッシュを行ったかどうかのフラグ
		/// </summary>
		/// <remarks>
		/// </remarks>
		private bool IsCached = false;

		#endregion フィールド


		#region プロパティ

		public long ArtifactId
		{
			get
			{ return _ArtifactId; }
			set
			{
				if (_ArtifactId == value)
					return;
				_ArtifactId = value;
				RaisePropertyChanged();
			}
		}

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

		public BitmapSource Thumbnail
		{
			get
			{ return _Thumbnail; }
			set
			{
				if (_Thumbnail == value)
					return;
				_Thumbnail = value;
				RaisePropertyChanged();
			}
		}

		public string ThumbnailKey
		{
			get
			{
				return _ThumbnailKey;
			}
			set
			{
				_ThumbnailKey = value;
			}
		}

		#endregion プロパティ


		#region メソッド

		public override void LoadedFromData(ILazyLoadingItem loadedData)
		{
			if (IsCached) return;
			IsCached = true;
			
			if (!string.IsNullOrEmpty(this.ThumbnailKey))
			{
				using (var proxy = new MogamiApiServiceClient())
				{
					proxy.Login();

					var loadthumbparam = new REQUEST_LOADTHUMBNAIL { ThumbnailKey = ThumbnailKey };
					var rsp = proxy.LoadThumbnail(loadthumbparam);
					
					using(Stream stream = new MemoryStream(rsp.ThumbnailBytes))
					{
						// ロックしないように指定したstreamを使用する。
						BitmapDecoder decoder = BitmapDecoder.Create(
							stream,
							BitmapCreateOptions.None, // この辺のオプションは適宜
							BitmapCacheOption.Default // これも
						);
						BitmapSource bmp = new WriteableBitmap(decoder.Frames[0]);
						bmp.Freeze();

						this.Thumbnail = bmp;
					}
				}
			}
		}

		#endregion メソッド

	}
}
