using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Service.Request
{
	[DataContract]
	public class REQUEST_LOADTHUMBNAIL
	{

		#region プロパティ

		[DataMember]
		public string ThumbnailKey { get; set; }

		#endregion プロパティ
	}
}
