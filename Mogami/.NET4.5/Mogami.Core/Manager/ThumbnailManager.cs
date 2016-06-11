using log4net;
using Mogami.Core.Attributes;
using Mogami.Core.Constructions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Mogami.Core.Manager
{
	public class ThumbnailManager
	{

		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(ThumbnailManager));

		#endregion フィールド


		#region メソッド

		/// <summary>
		/// サムネイル作成
		/// </summary>
		/// <returns></returns>
		public void BuildThumbnail(string key, string baseImageFilePath)
		{
			LOG.Debug("サムネイルの作成を開始します");
			Byte[] imageByte = null;
			{
				imageByte = LoadImageBytes(baseImageFilePath);

				if (imageByte == null) return; // 画像ファイルを取得できなかった。


				//　サムネイル種類別にすべてのサムネイルを生成する
				foreach (var thmbType in Enum.GetValues(typeof(ThumbnailType)))
				{
					ThumbnailInfoAttribute[] infos = (ThumbnailInfoAttribute[])thmbType.GetType().GetField(thmbType.ToString()).GetCustomAttributes(typeof(ThumbnailInfoAttribute), false);
					if (infos.Length > 0)
					{
						var @attr = infos[0];
						var dir = ThumbnailFileDir(key, @attr);
						System.IO.Directory.CreateDirectory(dir); // ディレクトリが無い場合は、作成します。

						var resizedImage = CreateImage(imageByte, 300, 0); // 生成するサムネイル画像の大きさは「300」(TODO?)

						var invoker = new ThumbnailEncodingInvoker(resizedImage, Path.Combine(dir, key));
						var encodingBackground = new BackgroundWorker();
						encodingBackground.DoWork += invoker.Do;
						encodingBackground.RunWorkerAsync();

						// スレッドの処理が完了するまで待機
						while (encodingBackground.IsBusy)
							//await Task.Delay(10);
							System.Threading.Thread.Sleep(1);

						encodingBackground.DoWork -= invoker.Do;
					}
				}
			}
			LOG.Debug("サムネイルの作成を完了します");
		}

		/// <summary>
		/// 画像のリサイズ(縮小)を行います
		/// </summary>
		/// <param name="rawImage">画像のバイナリ</param>
		/// <param name="decodePixelWidth">リサイズ後の横幅</param>
		/// <param name="decodePixelHeight">リサイズ後の高さ</param>
		/// <returns></returns>
		private ImageSource CreateImage(byte[] rawImage, int decodePixelWidth, int decodePixelHeight)
		{
			BitmapImage result = new BitmapImage();
			result.BeginInit();

			if (decodePixelWidth > 0)
				result.DecodePixelWidth = decodePixelWidth;
			if (decodePixelHeight > 0)
				result.DecodePixelHeight = decodePixelHeight;

			result.StreamSource = new MemoryStream(rawImage);
			result.CacheOption = BitmapCacheOption.Default;
			result.EndInit();
			return result;
		}

		/// <summary>
		/// 任意のファイルのデータをバイナリ列で取得する
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		byte[] LoadImageBytes(string filePath)
		{
			// チェックはしていないが、画像ファイル限定。
			using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
			using (BinaryReader br = new BinaryReader(fs))
			{
				byte[] imageBytes = br.ReadBytes((int)fs.Length);
				return imageBytes;
			}
		}

		// <summary>
		/// サムネイル定義毎のサムネイルを保存するためのディレクトリパス
		/// </summary>
		/// <param name="key">サムネイルのキー</param>
		/// <param name="attr">サムネイル定義情報</param>
		/// <returns></returns>
		string ThumbnailFileDir(string key, ThumbnailInfoAttribute attr)
		{
			var d1 = key.Substring(0, 2);
			var d2 = key.Substring(2, 4);
			return ApplicationContext.ThumbnailDirectoryPath + "\\" + @attr.ThumbnailDirectoryName + "\\" + d1 + "\\" + d2;
		}

		#endregion メソッド

		#region 内部クラス

		/// <summary>
		/// サムネイルファイルの出力をバックグラウンドスレッドで行うためのラップクラス
		/// </summary>
		class ThumbnailEncodingInvoker
		{

			#region フィールド

			readonly ImageSource _ImageSource;
			readonly string _OutputPath;

			#endregion フィールド

			#region コンストラクタ

			/// <summary>
			/// コンストラクタ
			/// </summary>
			/// <param name="imageSource"></param>
			/// <param name="outputPath">サムネイル画像ファイルの出力先ファイルパス</param>
			public ThumbnailEncodingInvoker(ImageSource imageSource, string outputPath)
			{
				this._ImageSource = imageSource;
				this._OutputPath = outputPath;
			}

			#endregion コンストラクタ


			#region メソッド

			/// <summary>
			/// サムネイルを生成します
			/// </summary>
			/// <param name="sender"></param>
			/// <param name="args"></param>
			public void Do(object sender, DoWorkEventArgs args)
			{
				try
				{
					using (FileStream wstream = new FileStream(this._OutputPath, FileMode.Create))
					{
						PngBitmapEncoder encoder = new PngBitmapEncoder();
						var frame = BitmapFrame.Create((BitmapImage)this._ImageSource);
						encoder.Frames.Add(frame);
						encoder.Save(wstream);
						wstream.Close();
					}
				}
				catch (NotSupportedException expr)
				{
					LOG.WarnFormat(expr.Message);
				}
			}

			#endregion メソッド
		}

		#endregion 内部クラス
	}
}
