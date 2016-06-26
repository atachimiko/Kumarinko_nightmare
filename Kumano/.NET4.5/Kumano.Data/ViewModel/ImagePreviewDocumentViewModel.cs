using Akalib.Wpf.Dock;
using Akalib.Wpf.Mvvm;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Akalib.Wpf;
using System.IO;
using Kumano.Data.Infrastructure;
using Kumano.Data.Construction;
using Kumano.Core;

namespace Kumano.Data.ViewModel
{
	/// <summary>
	/// 
	/// </summary>
	public class ImagePreviewDocumentViewModel : DocumentViewModelBase, IDocumentPaneViewModel
	{

		#region フィールド

		// 縮小率。
		// この値を大きくすると、少ないスクロール量でもスケールの変動が大きく(大胆に)変化する。
		// この値を小さくすると、スクロール量に対してスクロールの変動が小さくなります。
		const double repeatValue = 0.008;

		/// <summary>
		/// 
		/// </summary>
		const double SCALE_MIN = 0.01;

		static ILog LOG = LogManager.GetLogger(typeof(ImagePreviewDocumentViewModel));

		private int _CurrentImagePosition;

		/// <summary>
		/// 
		/// </summary>
		Point _dragOffset;

		private bool _EnabledImageFitMode = true;

		BitmapSource _ImageBitmap = null;

		/// <summary>
		/// 画像を表示するキャンバス(UI)のサイズ
		/// </summary>
		Size _ImageCanvasSize = new Size();

		double _ImageOffsetX = 0.0;

		double _ImageOffsetY = 0.0;

		double _ImageScaleX = 1.0;

		double _ImageScaleY = 1.0;

		private double _ImageScrollViwerVerticalOffset;

		Stretch _ImageStretch = Stretch.None;

		Transform _ImageTransform = null;

		/// <summary>
		/// コンポーネントが初期化済みかどうかのフラグ
		/// </summary>
		private bool _IsInitialized = false;

		private LoadImageInfo _LoadImageInfo = null;

		/// <summary>
		/// 表示中の項目位置(NavigationItemsの要素位置)
		/// </summary>
		int _NavigationItemPosition = 0;

		/// <summary>
		///     キャンバス内でのドラッグ操作中かどうかを示すフラグです。
		/// </summary>
		bool IsImageCanvasDragging = false;

		private double oldImageOffsetX = 0.0;

		private double oldImageOffsetY = 0.0;

		#endregion フィールド


		#region コンストラクタ

		public ImagePreviewDocumentViewModel()
		{
			this.Title = "○○○画像"; // TODO: ドキュメントペインのタイトルを適切なもので表示
		}

		#endregion コンストラクタ


		#region プロパティ

		public string ContentId
		{
			get { return "ImageDocument"; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int CurrentImagePosition
		{
			get
			{ return _CurrentImagePosition; }
			set
			{
				if (_CurrentImagePosition == value)
					return;
				_CurrentImagePosition = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// フィットモード
		/// </summary>
		/// <remarks>
		/// このフラグがTrueの場合、画像をキャンバスのサイズにフィットさせて表示する。
		/// </remarks>
		public bool EnabledImageFitMode
		{
			get
			{ return _EnabledImageFitMode; }
			private set
			{
				if (_EnabledImageFitMode == value)
					return;
				_EnabledImageFitMode = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// 画像ファイルデータ
		/// </summary>
		/// <remarks>
		/// LoadImage()で画像を読み込むまでは、このプロパティはNULLを返します。
		/// </remarks>
		public BitmapSource ImageBitmap
		{
			get { return _ImageBitmap; }
			internal set
			{
				_ImageBitmap = value;
				RaisePropertyChanged(() => ImageBitmap);
			}
		}

		public double ImageOffsetX
		{
			get { return _ImageOffsetX; }
			set
			{
				if (Equals(_ImageOffsetX, value)) return;
				_ImageOffsetX = value;
				RaisePropertyChanged(() => ImageOffsetX);
			}
		}

		public double ImageOffsetY
		{
			get { return _ImageOffsetY; }
			set
			{
				if (Equals(_ImageOffsetY, value)) return;
				_ImageOffsetY = value;
				RaisePropertyChanged(() => ImageOffsetY);
			}
		}

		/// <summary>
		/// 表示する画像のスケール(X軸)
		/// </summary>
		public double ImageScaleX
		{
			get { return _ImageScaleX; }
			set
			{
				if (Equals(_ImageScaleX, value)) return;
				_ImageScaleX = value;
				RaisePropertyChanged(() => ImageScaleX);
			}
		}

		/// <summary>
		/// 表示する画像のスケール(Y軸)
		/// </summary>
		public double ImageScaleY
		{
			get { return _ImageScaleY; }
			set
			{
				if (Equals(_ImageScaleY, value)) return;
				_ImageScaleY = value;
				RaisePropertyChanged(() => ImageScaleY);
			}
		}

		/// <summary>
		/// 画像を表示しているScrollViewerの縦方向のスクロール位置です。
		/// スクロールの制御にはコードビハインドが働いているので、このプロパティを使ってむやみにスクロール位置を変更しないように。
		/// </summary>
		public double ImageScrollViwerVerticalOffset
		{
			get
			{ return _ImageScrollViwerVerticalOffset; }
			set
			{
				if (_ImageScrollViwerVerticalOffset == value)
					return;
				_ImageScrollViwerVerticalOffset = value;
				RaisePropertyChanged();
			}
		}

		public Stretch ImageStretch
		{
			get { return _ImageStretch; }
			set
			{
				if (Equals(_ImageStretch, value)) return;
				_ImageStretch = value;
				RaisePropertyChanged(() => ImageStretch);
			}
		}

		/// <summary>
		/// 画像コントロールでの画像の表示サイズの自動スケーリングを設定する
		/// </summary>
		public Transform ImageTransform
		{
			get { return _ImageTransform; }
			set
			{
				if (Equals(_ImageTransform, value)) return;
				_ImageTransform = value;
				RaisePropertyChanged(() => ImageTransform);
			}
		}

		public LoadImageInfo PreviewImageInfo
		{
			get
			{ return _LoadImageInfo; }
			set
			{
				if (_LoadImageInfo == value)
					return;
				_LoadImageInfo = value;
				
				OnLoadedImageInfo();

				RaisePropertyChanged();
			}
		}

		public IPropertyPaneItem PropertyPaneItem
		{
			get { throw new NotImplementedException(); }
		}

		#endregion プロパティ

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
					ShowPropertyPaneItemType = ShowPropertyPaneItemType.ImageDetailProperty
				};
				Messenger.Raise(message);

				var vm = message.Response as ImagePreviewPropertyViewModel;
				if (vm != null)
				{
					// TODO: プロパティ用のViewModelに画像詳細情報を書き込む
					// 表示するデータは未決定。
					vm.SampleText = "山田哲人";
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

		#region メソッド

		public void ActiveChanged()
		{
			LOG.DebugFormat("Active ImagePreviewDocumentViewModel IsActive={0}",this.IsActive);

			// プロパティペインへの表示を行います
			if (this.IsActive)
				UpdatePropertyPane();
			else
				UpdatePropertyPane(true);
		}

		public override void Close()
		{
			((WorkspaceViewModel)ApplicationContext.Workspace).Close(this);

			// NOTE: ペインを閉じるときに行う特別な処理があれば実装します。
			this.ImageBitmap = null;
		}

		/// <summary>
		///     現在のスケーリングでの画像サイズ
		/// </summary>
		/// <returns>画像のサイズ</returns>
		public Size CurrentScaledImageSize()
		{
			return new Size(
				this.ImageBitmap.PixelWidth * this.ImageScaleX,
				this.ImageBitmap.PixelHeight * this.ImageScaleY
				);
		}

		/// <summary>
		/// 表示領域に画像がすべて描画できるように、画像の縮小を行う。
		/// </summary>
		public void FitImage()
		{
			this.EnabledImageFitMode = true;
			FittingViewSize(this._ImageCanvasSize.Width, this._ImageCanvasSize.Height);
		}

		/// <summary>
		/// 画像の拡大表示を行います
		/// </summary>
		public void ImageZoomIn()
		{
			if (ImageBitmap == null) return;

			//if (!ApplicationContext.ApplicationSetting.ImageViewConfig.ResetZoomLevelNavigationFlag)
			//	this.EnabledImageFitMode = false; // 一度でも拡大率を変更した場合は、フィットモードを解除する。

			double v = ImageScaleX;
			v = v + repeatValue;
			if (v >= 10.0) v = 10.0; // スケールの最大値
			ImageScaleX = ImageScaleY = v;

			ImageStretch = Stretch.None;
			UpdateImageTransform();
		}

		/// <summary>
		/// 画像の縮小表示を行います
		/// </summary>
		public void ImageZoomOut()
		{
			if (ImageBitmap == null) return;

			//if (!ApplicationContext.ApplicationSetting.ImageViewConfig.ResetZoomLevelNavigationFlag)
			//	this.EnabledImageFitMode = false; // 一度でも拡大率を変更した場合は、フィットモードを解除する。

			double v = ImageScaleX;
			v = v - repeatValue;
			if (v <= SCALE_MIN) v = SCALE_MIN; // スケールの最小値

			ImageScaleX = ImageScaleY = v;

			ImageStretch = Stretch.None;
			UpdateImageTransform();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		public void OnCanvasLoaded(RoutedEventArgs e)
		{
			var imageControl = e.Source as Image;
			dynamic prop = imageControl.Parent;
			var grid = prop.Parent as Grid;
			if (grid != null)
			{
				this._ImageCanvasSize.Height = grid.ActualHeight;
				this._ImageCanvasSize.Width = grid.ActualWidth;
			}

			if (this.PreviewImageInfo != null)
				LoadImageAsync(); // コンポーネント表示時に表示される画像の読み込み

			_IsInitialized = true;
		}

		/// <summary>
		///     キャンバスのMouseLeftButtonDownイベントのハンドラ
		/// </summary>
		/// <param name="e"></param>
		public void OnCanvasMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			// ドラッグ開始
			var imageControl = e.Source as Image;

			IsImageCanvasDragging = true;

			// ドラッグ開始時のImageコントロールのオフセット値を保持
			oldImageOffsetX = ImageOffsetX;
			oldImageOffsetY = ImageOffsetY;

			var @scroll = imageControl.Parent.FindAncestor<ScrollViewer>();
			Point pt = Mouse.GetPosition(@scroll);
			_dragOffset = pt;
			imageControl.CaptureMouse();
		}

		/// <summary>
		///     キャンバスのMouseLeftButtonUpイベントのハンドラ
		/// </summary>
		/// <param name="e"></param>
		public void OnCanvasMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			// ドラッグ終了
			var imageControl = e.Source as Image;
			imageControl.ReleaseMouseCapture();
			IsImageCanvasDragging = false;
		}

		/// <summary>
		///     キャンバスのMouseMoveイベントのハンドラ
		/// </summary>
		/// <param name="e"></param>
		public void OnCanvasMouseMove(MouseEventArgs e)
		{
			// キャンバスのドラッグ中ならば、
			// 現在のマウス座標との差分を取って、Imageコントロールを表示する位置をScrollViewerに設定する。

			if (IsImageCanvasDragging)
			{
				var imageControl = e.Source as Image;

				var @scroll = imageControl.Parent.FindAncestor<ScrollViewer>();
				Point pt = Mouse.GetPosition(@scroll);
				double diffX = pt.X - _dragOffset.X;
				double diffY = pt.Y - _dragOffset.Y;


				// ImageOffsetX/ImageOffsetYはMVVMで
				// ScrollViewerのOffset値とバインドしている。
				this.ImageOffsetX = oldImageOffsetX - diffX;
				this.ImageOffsetY = oldImageOffsetY - diffY;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		public void OnCanvasSizeChanged(SizeChangedEventArgs e)
		{
			var imageControl = e.Source as Image;
			dynamic prop = imageControl.Parent;
			var grid = prop.Parent as Grid;
			if (grid != null)
			{
				this._ImageCanvasSize.Height = grid.ActualHeight;
				this._ImageCanvasSize.Width = grid.ActualWidth;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		public async void OnFileControlMouseWheel(MouseWheelEventArgs p)
		{
			try
			{
				if (p.Delta > 0)
				{
					//await UpdatePrevNavigationSelection();
				}
				else if (p.Delta < 0)
				{
					//await UpdateNextNavigationSelection();
				}
			}
			catch (Exception expr)
			{
				LOG.ErrorFormat("{0}\n\t{1}", expr.Message, expr.StackTrace);
			}
		}

		public void OnGridLoaded(RoutedEventArgs e)
		{
			var gridControl = e.Source as Grid;
			//LOG.InfoFormat("ImageGridLoadedCommandコマンドの呼び出し");
			//LOG.InfoFormat("Gridのサイズ {0} {1}", gridControl.ActualWidth, gridControl.ActualHeight);

			// コンテナが設置された際のコンテナのサイズをActualWidth/ActualHeightで取得可能。
			this._ImageCanvasSize = new Size(gridControl.ActualWidth, gridControl.ActualHeight);
			this.EnabledImageFitMode = true;

			if (ImageBitmap != null && this.EnabledImageFitMode) // 画像が読み込み済みの場合は、画像を表示領域にフィットさせる。
			{
				FittingViewSize(gridControl.ActualWidth, gridControl.ActualHeight);
			}

			//ShowPropertyView();
		}

		public void OnGridSizeChanged(SizeChangedEventArgs e)
		{
			// リサイズ後のコンテナのサイズを取得
			double lastHeight = e.NewSize.Height;
			double lastWidth = e.NewSize.Width;

			//LOG.InfoFormat("サイズ変更イベント2 Height={0} Width={1}", lastHeight, lastWidth);

			this._ImageCanvasSize = new Size(lastWidth, lastHeight);

			if (ImageBitmap != null && this.EnabledImageFitMode) // 画像が読み込み済みの場合は、画像を表示領域にフィットさせる。
			{
				FittingViewSize(lastWidth, lastHeight);
			}
		}

		/// <summary>
		///     
		/// </summary>
		public void OnImageViewMouseDoubleClick()
		{
			// このドキュメントコンテンツペインを閉じます。
			// TODO: 画像リストから、項目をダブルクリックした場合の処理

			//WorkspaceViewModel workspace;
			//if (this._Workspace.TryGetTarget(out workspace))
			//{
			//	workspace.Close(this);
			//}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		public async void OnImageViewPreviewKeyDown(KeyEventArgs e)
		{
			try
			{
				if (e.Key == Key.Right)
				{
					//await UpdateNextNavigationSelection();
				}
				else if (e.Key == Key.Left)
				{
					//await UpdatePrevNavigationSelection();
				}
			}
			catch (Exception expr)
			{
				LOG.ErrorFormat("{0}\n\t{1}", expr.Message, expr.StackTrace);
			}
		}

		public void OnZoomControlMouseWheel(MouseWheelEventArgs p)
		{
			if (ImageBitmap == null) return;

			p.Handled = true;

			if (p.Delta < 0)
			{
				ImageZoomIn();
			}
			if (p.Delta > 0)
			{
				ImageZoomOut();
			}
		}

		/// <summary>
		/// 指定した領域内に画像が表示されるように画像の縮小スケールを設定します
		/// </summary>
		/// 
		/// <param name="width">キャンバスの幅。画像を表示したい領域の横幅。</param>
		/// <param name="height">キャンバスの高さ。画像を表示したい領域の縦幅。</param>
		void FittingViewSize(double width, double height)
		{
			double v = 1.0;
			if (ImageBitmap == null) return;

			if (ImageBitmap.PixelHeight >= ImageBitmap.PixelWidth)
			{
				// 縦幅が大きい
				if (height < ImageBitmap.PixelHeight)
				{
					v = height / ImageBitmap.PixelHeight;
				}

				double cheight = v * ImageBitmap.PixelHeight;

				// 縮小時のサイズが630ピクセル未満になるまで、
				// スケールの値を小さくしていく。
				while (cheight > height - 20)
				{
					v -= 0.002; // 少しずつ小さくしていく
					cheight = v * ImageBitmap.PixelHeight;
				}
			}
			else
			{
				// 横幅が大きい
				if (width < ImageBitmap.PixelWidth)
				{
					v = width / ImageBitmap.PixelWidth;
				}

				double cwidth = v * ImageBitmap.PixelWidth;

				// 縮小時のサイズがキャンバスサイズ(width)に収まるまで
				// 縮小スケール値を減少させていく。(「width-20」はImageコントロールに設定しているMarginの値)
				while (cwidth > width - 40)
				{
					v -= 0.002; // 少しずつ小さくしていく
					cwidth = v * ImageBitmap.PixelWidth;
				}
			}

			ImageScaleX = ImageScaleY = v;

			//string msg = string.Format("画像のピクセル数 {0}x{1} (DPI: {2}x{3})", ImageBitmap.PixelWidth, ImageBitmap.PixelHeight, ImageBitmap.DpiX , ImageBitmap.DpiY);
			//LOG.DebugFormat(msg);

			//ApplicationContext.AddMessage(msg);
			//msg = string.Format("\t{0}x{1}", ImageBitmap.Width, ImageBitmap.Height);
			//ApplicationContext.AddMessage(msg);

			UpdateImageTransform();
		}

		/// <summary>
		/// 
		/// </summary>
		private async Task LoadImageAsync()
		{
			if (this.PreviewImageInfo == null || this.PreviewImageInfo.BitmapFilePath == null)
			{
				LOG.Warn("読み込む画像情報が未設定です。");
				return;
			}

			var sw = new Stopwatch();
			sw.Start();
			await Task.Factory.StartNew(() =>
			{
				// NOTE: 画像ファイルの読み込み

				// サンプルコード:読み込んでいるファイルは固定
				try
				{
					string filePath = string.Format(this.PreviewImageInfo.BitmapFilePath);
					using (Stream stream = new FileStream(
						filePath,
						FileMode.Open,
						FileAccess.Read,
						FileShare.ReadWrite | FileShare.Delete
					))
					{
						// ロックしないように指定したstreamを使用する。
						BitmapDecoder decoder = BitmapDecoder.Create(
							stream,
							BitmapCreateOptions.None, // この辺のオプションは適宜
							BitmapCacheOption.Default // これも
						);
						var srcBmp = decoder.Frames[0];
						int pixelHeight = srcBmp.PixelHeight;
						int pixelWidth = srcBmp.PixelWidth;

						WriteableBitmap bmp = new WriteableBitmap(
							srcBmp.PixelWidth,
							srcBmp.PixelHeight,
							96, 96,
							srcBmp.Format,
							srcBmp.Palette);

						// Calculate stride of source
						int stride = (srcBmp.PixelWidth * srcBmp.Format.BitsPerPixel + 7) / 8;

						// Create data array to hold source pixel data
						byte[] data = new byte[stride];

						for (int i = 0; i < pixelHeight; i++)
						{

							srcBmp.CopyPixels(
								new Int32Rect(0, i, pixelWidth, 1),
								data,
								stride,
								0);


							bmp.WritePixels(
								new Int32Rect(0, 0, pixelWidth, 1),
								data,
								stride,
								0,
								i);
						}
						bmp.Freeze();

						this.ImageBitmap = bmp;
					}
				}
				catch (Exception exc)
				{
					//MessageBox.Show(this, "[" + exc.Message + "]\n" + exc.StackTrace, this.Title);
				}
				// -- サンプルコード

				//var decorator = new ImageArtifactDecorator(this.Entity.ModelInstance, ImageArtifactDecorator.OperationType.LoadImage);
				//decorator.Operation();
				//this.ImageBitmap = decorator.Image;
			});
			sw.Stop();

			LOG.DebugFormat("画像表示までの時間:{0}", sw.ElapsedMilliseconds);


			// 画像サイズ調整
			if (this.EnabledImageFitMode)
			{
				FittingViewSize(this._ImageCanvasSize.Width, this._ImageCanvasSize.Height);
			}
			else
			{
				this.ImageStretch = Stretch.None;

				UpdateImageTransform();
			}
		}

		private void OnLoadedImageInfo()
		{
			// コンポーネントが初期化済みの場合、画像の読み込みを行う。
			if (this._IsInitialized)
				LoadImageAsync();
		}
		/// <summary>
		/// 
		/// </summary>
		private void UpdateImageTransform()
		{
			var scaleTransform = new ScaleTransform(this.ImageScaleX, this.ImageScaleY);
			scaleTransform.CenterX = this.ImageBitmap.PixelWidth / 2.0;
			scaleTransform.CenterY = this.ImageBitmap.PixelHeight / 2.0;
			this.ImageTransform = scaleTransform;
		}

		#endregion メソッド

	}

	/// <summary>
	/// ビューに表示する画像データの情報
	/// </summary>
	public class LoadImageInfo
	{


		#region フィールド

		public string BitmapFilePath;

		#endregion フィールド

	}
}
