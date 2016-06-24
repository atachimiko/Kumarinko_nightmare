using Kumano.Contrib.Infrastructures;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Kumano.Core.Presentation
{
	public class ImageListLazyItem : ItemDataBase
	{


		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(ImageListLazyItem));

		private long _ArtifactId;
		private string _IdText;

		private string _Label;

		/// <summary>
		/// 画像のキャッシュを行ったかどうかのフラグ
		/// </summary>
		/// <remarks>
		/// 読み込みに失敗していても、このフラグをTrueに設定すべきです。
		/// </remarks>
		private bool IsCached = false;

		private BitmapSource _Thumbnail;
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

		#endregion プロパティ


		#region メソッド

		public override void LoadedFromData(ILazyLoadingItem loadedData)
		{
			if (IsCached) return; // 読み込み済みの場合は、再度読み込みは行わない。
			IsCached = true;

			try
			{
				string filePath = string.Format(@"X:\30_Artifact\10_AdsImage\99_Event\C87\(C87) [Melty Pot (Mel)] Happy Style! 2 (ゆゆ式)\{0}", this.Label);
				//string filePath = this.FilePath;
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
					BitmapSource bmp = new WriteableBitmap(decoder.Frames[0]);
					bmp.Freeze();

					this.Thumbnail = bmp;
				}
			}
			catch (Exception exc)
			{
				LOG.WarnFormat("[" + exc.Message + "]\n" + exc.StackTrace);
			}
		}

		#endregion メソッド
	}
}
