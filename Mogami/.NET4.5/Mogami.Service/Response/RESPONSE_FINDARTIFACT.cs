using Mogami.Model;
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
			this.Artifacts = new List<Artifact>();
		}

		#endregion コンストラクタ


		#region プロパティ

		public List<Artifact> Artifacts { get; set; }

		public List<byte[]> Thumbnails { get; set; }

		#endregion プロパティ

	}
}
