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
		private string _Key;

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

		public string Key
		{
			get { return _Key; }
			set
			{
				if (_Key == value) return;
				_Key = value;
			}
		}

		#endregion プロパティ
	}
}
