using Mogami.Model;
using Mogami.Service.Serialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Service.Response
{
	public class RESPONSE_FINDARTIFACT : RESPONSEAPI_BASE
	{


		#region コンストラクタ

		public RESPONSE_FINDARTIFACT()
		{
			this.Artifacts = new List<DataArtifact>();
		}

		#endregion コンストラクタ


		#region プロパティ

		public List<DataArtifact> Artifacts { get; set; }

		#endregion プロパティ

	}
}
