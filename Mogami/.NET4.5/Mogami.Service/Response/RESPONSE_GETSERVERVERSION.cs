using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Service.Response
{
	[DataContract]
	public sealed class RESPONSE_GETSERVERVERSION : RESPONSEAPI_BASE
	{

		#region プロパティ

		[DataMember]
		public string VersionText { get; set; }

		#endregion プロパティ
	}
}
