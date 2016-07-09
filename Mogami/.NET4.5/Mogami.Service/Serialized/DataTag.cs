using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Service.Serialized
{
	[DataContract]
	public class DataTag
	{

		#region プロパティ

		[DataMember(Order = 0)]
		public long Id { get; set; }

		[DataMember(Order = 2)]
		public bool IsHasChild { get; set; }

		[DataMember(Order = 1)]
		public string Name { get; set; }

		#endregion プロパティ
	}
}
