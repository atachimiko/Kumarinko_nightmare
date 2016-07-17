using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Service.Response
{
	[DataContract]
	public class RESPONSE_LOADDEVICESETTING : RESPONSEAPI_BASE
	{


		#region プロパティ

		[DataMember]
		public string Data { get; set; }

		#endregion プロパティ

	}
}
