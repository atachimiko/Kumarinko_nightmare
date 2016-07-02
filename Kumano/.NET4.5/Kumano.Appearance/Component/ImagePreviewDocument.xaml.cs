using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kumano.View.Component
{
	/// <summary>
	/// ImagePreviewDocument.xaml の相互作用ロジック
	/// </summary>
	public partial class ImagePreviewDocument : UserControl
	{
		public ImagePreviewDocument()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			Point mousePos = Mouse.GetPosition((IInputElement)sender);

			dynamic vm = this.DataContext;

			var @bitmap = vm.ImageBitmap as BitmapSource;
			if (@bitmap != null)
			{
				Size size = vm.CurrentScaledImageSize();

				// 「サムネイル：画像」の表示割合
				double dShowThumbHeight = size.Height / this.ThumbnailImage.ActualHeight;
				double dShowThumbWidth = size.Width / this.ThumbnailImage.ActualWidth;

				var mouseTopBasePos = mousePos.Y * dShowThumbHeight;
				var mouseLeftBasePos = mousePos.X * dShowThumbWidth;
				//Console.WriteLine("実際の画像でのピクセル位置 X:{0} Y:{1}", mouseLeftBasePos, mouseTopBasePos);

				// ScrollViewのスクロール位置を変更
				this.ScrollViewerElement.ScrollToHorizontalOffset(mouseLeftBasePos);
				this.ScrollViewerElement.ScrollToVerticalOffset(mouseTopBasePos);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			UpdateYJiku();
			UpdateXJiku();
		}

		/// <summary>
		/// X軸方向のみ
		/// </summary>
		private void UpdateXJiku()
		{
			dynamic vm = this.DataContext;

			var @bitmap = vm.ImageBitmap as BitmapSource;
			if (@bitmap != null)
			{
				Size size = vm.CurrentScaledImageSize();

				//Console.WriteLine("Image.ActualHeight = {0} ScrollContentsHeight = {1}", this.SourceImage.ActualHeight, this.ImageScrollArea.ScrollableHeight);

				// 表示割合
				double dShowWidth = this.ThumbnailImage.ActualWidth / size.Width;
				double dFillWidth = this.ScrollViewerElement.ActualWidth / size.Width;
				//Console.WriteLine("表示割合={0} 塗りつぶし割合={1}", dShowWidth, dFillWidth);

				// サムネイル領域のY方向位置
				//Console.WriteLine("スクロール位置(H) = {0}", this.ImageScrollArea.VerticalOffset);
				double thumbBorderLeft = this.ScrollViewerElement.HorizontalOffset * dShowWidth;
				//Console.WriteLine("サムネイル位置(H) = {0}", thumbBorderTop);

				// サムネイル領域の塗りつぶし幅
				double fillWidth = this.ThumbnailImage.ActualWidth * dFillWidth;
				//Console.WriteLine("塗りつぶし幅={0}", fillWidth);


				// サムネイル領域の表示領域を描画
				Canvas.SetLeft(this.TargetBorder, thumbBorderLeft);

				var @dif = (thumbBorderLeft + fillWidth) - ThumbnailImage.ActualWidth;
				//Console.WriteLine("はみ出し？ = {0}", @dif);
				if (@dif < 0) @dif = 0;

				this.TargetBorder.Width = fillWidth - @dif;
			}
		}

		/// <summary>
		/// Y軸方向のみ
		/// </summary>
		private void UpdateYJiku()
		{
			dynamic vm = this.DataContext;

			var @bitmap = vm.ImageBitmap as BitmapSource;
			if (@bitmap != null)
			{
				Size size = vm.CurrentScaledImageSize();

				//Console.WriteLine("Image.ActualHeight = {0}", this.SourceImage.ActualHeight);
				//Console.WriteLine("Image.Size         = {0}", size);

				// 表示割合
				double dShowHeight = this.ThumbnailImage.ActualHeight / size.Height;
				double dFillHeight = this.ScrollViewerElement.ActualHeight / size.Height;
				//Console.WriteLine("表示割合={0} 塗りつぶし割合={1}", dShowHeight, dFillHeight);

				// サムネイル領域のY方向位置
				//Console.WriteLine("スクロール位置(H) = {0}", this.ImageScrollArea.VerticalOffset);
				double thumbBorderTop = this.ScrollViewerElement.VerticalOffset * dShowHeight;
				//Console.WriteLine("サムネイル位置(H) = {0}", thumbBorderTop);

				// サムネイル領域の塗りつぶし高さ
				double fillHeight = this.ThumbnailImage.ActualHeight * dFillHeight;
				//Console.WriteLine("塗りつぶし高さ={0} ({1})", fillHeight, this.ThumbnailImage.ActualHeight);


				// サムネイル領域の表示領域を描画
				Canvas.SetTop(this.TargetBorder, thumbBorderTop);

				var @dif = (thumbBorderTop + fillHeight) - ThumbnailImage.ActualHeight;
				//Console.WriteLine("はみ出し？ = {0}", @dif);

				if (@dif < 0) @dif = 0;

				this.TargetBorder.Height = fillHeight - @dif;
			}
		}

	}
}
