using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Core.Infrastructure
{
	public interface IThumbnailManager
	{

		#region メソッド

		/// <summary>
		/// 
		/// </summary>
		/// <param name="thumbnailhash">既存のサムネイルを、baseImageFilePathで生成しなおしたい場合、
		/// 既存のサムネイル情報を示すキーを指定します。それ以外は、NULLを指定します。</param>
		/// <param name="baseImageFilePath">サムネイル生成元の画像ファイルパス</param>
		/// <returns>生成したサムネイルのキーを返します</returns>
		string BuildThumbnail(string thumbnailhash, string baseImageFilePath);
		bool RemoveThumbnail(string thumbnailhash);

		#endregion メソッド
	}
}
