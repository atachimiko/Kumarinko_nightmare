using Akalib.String;
using log4net;
using Mogami.Core;
using Mogami.Core.Attributes;
using Mogami.Core.Constructions;
using Mogami.Core.Infrastructure;
using Mogami.Gateway;
using Mogami.Model;
using Mogami.Model.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Mogami.Applus.Manager
{
	public class ThumbnailManager : IThumbnailManager
	{


		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(ThumbnailManager));

		#endregion フィールド

		#region メソッド

		/// <summary>
		/// サムネイル作成
		/// </summary>
		/// <param name="thumbnailhash">既存のサムネイルを、baseImageFilePathで生成しなおしたい場合、
		/// 既存のサムネイル情報を示すキーを指定します。それ以外は、NULLを指定します。</param>
		/// <param name="baseImageFilePath">サムネイル生成元の画像ファイルパス</param>
		/// <returns></returns>
		public string BuildThumbnail(string thumbnailhash, string baseImageFilePath)
		{
			LOG.Debug("サムネイルの作成を開始します");
			string _ThumbnailKey = null;
			Byte[] imageByte = null;
			{
				imageByte = LoadImageBytes(baseImageFilePath);

				if (imageByte == null) throw new ApplicationException(); // 画像ファイルを取得できなかった。


				//　サムネイル種類別にすべてのサムネイルを生成する
				foreach (ThumbnailType thmbType in Enum.GetValues(typeof(ThumbnailType)))
				{
					ThumbnailInfoAttribute[] infos = (ThumbnailInfoAttribute[])thmbType.GetType().GetField(thmbType.ToString()).GetCustomAttributes(typeof(ThumbnailInfoAttribute), false);
					if (infos.Length > 0)
					{
						var @attr = infos[0];
						
						var resizedImage = CreateImage(imageByte, @attr.Width, @attr.Height); // 生成するサムネイル画像の大きさは「300」(TODO?)

						var invoker = new ThumbnailEncodingInvoker(thumbnailhash, resizedImage, thmbType);
						var encodingBackground = new BackgroundWorker();
						encodingBackground.DoWork += invoker.Do;
						encodingBackground.RunWorkerAsync();

						// スレッドの処理が完了するまで待機
						while (encodingBackground.IsBusy)
							//await Task.Delay(10);
							System.Threading.Thread.Sleep(1);

						_ThumbnailKey = invoker.ThumbnailKey;
						thumbnailhash = _ThumbnailKey;
						encodingBackground.DoWork -= invoker.Do;
					}
				}
			}
			LOG.Debug("サムネイルの作成を完了します");
			return _ThumbnailKey;
		}

		public bool RemoveThumbnail(string thumbnailhash)
		{
			bool bResult = false;

			using (var dbc = new ThumbDbContext())
			{
				var repo = new ThumbnailRepository(dbc);
				var thumbs = repo.FindFromKey(thumbnailhash);
				
				foreach (var prop in thumbs)
				{
					repo.Delete(prop);
				}
				bResult = true;
			}

			return bResult;
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
			readonly string _rebuildThumbnailKey;
			readonly ThumbnailType _ThumbnailType;

			private string _ThumbnailKey;
			#endregion フィールド

			#region コンストラクタ

			/// <summary>
			/// コンストラクタ
			/// </summary>
			/// <param name="thumbnailKey">リビルド対象のサムネイルキー</param>
			/// <param name="imageSource"></param>
			public ThumbnailEncodingInvoker(string thumbnailKey, ImageSource imageSource, ThumbnailType thumbnailType)
			{
				this._ImageSource = imageSource;
				_rebuildThumbnailKey = thumbnailKey;
				_ThumbnailType = thumbnailType;
			}

			#endregion コンストラクタ


			#region プロパティ

			public string ThumbnailKey
			{
				get { return _ThumbnailKey; }
			}

			#endregion プロパティ

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
					// バイナリに出力
					using (MemoryStream memoryStream = new MemoryStream())
					{
						PngBitmapEncoder encoder = new PngBitmapEncoder();
						var frame = BitmapFrame.Create((BitmapImage)this._ImageSource);
						encoder.Frames.Add(frame);
						encoder.Save(memoryStream);

						using (var dbc = new ThumbDbContext())
						{
							var repo = new ThumbnailRepository(dbc);

							if (string.IsNullOrEmpty(_rebuildThumbnailKey))
							{
								string key = null;
								while (key == null)
								{
									var tal = RandomAlphameric.RandomAlphanumeric(20);
									var r = repo.FindFromKey(tal);
									if (r.Count == 0) key = tal;
									foreach(var p in r)
									{
										if (p.ThumbnailType != _ThumbnailType) key = tal;
									}

								}

								var thumbnail = new Thumbnail();
								thumbnail.ThumbnailKey = key;
								thumbnail.ThumbnailType = _ThumbnailType;
								thumbnail.BitmapBytes = memoryStream.ToArray();

								repo.Add(thumbnail);

								_ThumbnailKey = key;
							}
							else
							{
								var thumbnail = repo.FindFromKey(_rebuildThumbnailKey);

								// サムネイルタイプのエンティティが存在する場合、trueをセットする。
								bool isThumbnailSave = false;
								foreach (var prop in thumbnail)
								{
									if (prop.ThumbnailType == _ThumbnailType)
									{
										prop.BitmapBytes = memoryStream.ToArray();
										isThumbnailSave = true;
									}
								}

								if (!isThumbnailSave)
								{
									// 指定したサムネイルタイプのエンティティを、
									// 新規作成する。
									var thumbnail_NewThumbnailType = new Thumbnail();
									thumbnail_NewThumbnailType.ThumbnailKey = _rebuildThumbnailKey;
									thumbnail_NewThumbnailType.ThumbnailType = _ThumbnailType;
									thumbnail_NewThumbnailType.BitmapBytes = memoryStream.ToArray();
									repo.Add(thumbnail_NewThumbnailType);
									_ThumbnailKey = _rebuildThumbnailKey;

								}
								else
								{
									_ThumbnailKey = _rebuildThumbnailKey;
								}
							}
							dbc.SaveChanges();
						}
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
