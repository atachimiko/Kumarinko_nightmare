using Mogami.Core.Constructions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Model
{
	[Table("alThumbnail")]
	public class Thumbnail : ServiceModel
	{


		#region フィールド

		private byte[] _BitmapBytes;
		private ThumbnailType _ThumbnailType;

		#endregion フィールド


		#region プロパティ

		public byte[] BitmapBytes
		{
			get { return _BitmapBytes; }
			set
			{
				_BitmapBytes = value;
			}
		}

		public ThumbnailType ThumbnailType
		{
			get { return _ThumbnailType; }
			set
			{
				_ThumbnailType = value;
			}
		}

		#endregion プロパティ
	}
}
