using Mogami.Service.Serialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Service.Response
{
	public class RESPONSE_LOADCATGEORY : RESPONSEAPI_BASE
	{

		#region コンストラクタ

		public RESPONSE_LOADCATGEORY()
		{
			this.Categories = new List<DataCategory>();
		}

		#endregion コンストラクタ


		#region プロパティ

		public List<DataCategory> Categories { get; set; }

		#endregion プロパティ
	}
}
