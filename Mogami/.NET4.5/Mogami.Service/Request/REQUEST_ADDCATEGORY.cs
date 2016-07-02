using Mogami.Service.Serialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Service.Request
{
	[DataContract]
	public class REQUEST_ADDCATEGORY
	{

		#region プロパティ

		[DataMember]
		public DataCategory Catgeory { get; set; }

		[DataMember]
		public long? ParentCategoryId { get; set; }

		#endregion プロパティ
	}
}
