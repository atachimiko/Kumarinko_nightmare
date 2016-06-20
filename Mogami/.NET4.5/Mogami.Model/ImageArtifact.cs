using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Model
{
	public class ImageArtifact : Artifact
	{

		#region フィールド

		private int _ImageHeight;
		private int _ImageWidth;

		#endregion フィールド


		#region プロパティ

		public int ImageHeight
		{
			get { return _ImageHeight; }
			set { _ImageHeight = value; }
		}

		public int ImageWidth
		{
			get { return _ImageWidth; }
			set { _ImageWidth = value; }
		}

		#endregion プロパティ
	}
}
