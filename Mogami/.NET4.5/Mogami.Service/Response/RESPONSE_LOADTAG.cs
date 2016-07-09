using Mogami.Service.Serialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Service.Response
{
	public class RESPONSE_LOADTAG : RESPONSEAPI_BASE
	{

		#region コンストラクタ

		public RESPONSE_LOADTAG()
		{
			this.Tags = new List<DataTag>();
		}

		#endregion コンストラクタ


		#region プロパティ

		public List<DataTag> Tags { get; set; }

		#endregion プロパティ
	}
}
