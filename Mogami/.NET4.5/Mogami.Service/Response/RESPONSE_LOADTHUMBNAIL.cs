using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Service.Response
{
	public class RESPONSE_LOADTHUMBNAIL : RESPONSEAPI_BASE
	{

		#region プロパティ

		public byte[] ThumbnailBytes { get; set; }

		#endregion プロパティ
	}
}
