using log4net;
using Mogami.Applus.Manager;
using Mogami.CT.xUnit.Action;
using Mogami.Gateway;
using Mogami.Model;
using Mogami.Model.Repository;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.CT.xUnit.Test
{
	[TestFixture]
	[UsingDatabaseAction(ImportInitializeDataFlag = true)]
	public class GenerateThumbnailTest
	{

		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(GenerateThumbnailTest));

		#endregion フィールド


		#region メソッド

		/// <summary>
		/// 
		/// </summary>
		[Test]
		[Category("Thumbnail")]
		public void Test_GenerateThumbnail()
		{
			var imageFile = GetAssetsImageFile("Lighthouse.jpg");
			LOG.Info("サムネイルの生成を開始します");
			var manager = new ThumbnailManager();
			manager.BuildThumbnail("aabbccddeeff00112233", imageFile.FullName);
			LOG.Info("サムネイルの生成を終了します");
		}

		/// <summary>
		/// アセンブリ内のサンプル用画像ファイル情報を取得する
		/// </summary>
		/// <param name="imageName">取得したい画像ファイル名</param>
		/// <returns></returns>
		private FileInfo GetAssetsImageFile(string imageName)
		{
			Assembly myAssembly = Assembly.GetExecutingAssembly();
			string path = Path.GetDirectoryName(myAssembly.Location);
			return new FileInfo(path + "/Assets/Image/" + imageName);
		}

		#endregion メソッド
	}
}
