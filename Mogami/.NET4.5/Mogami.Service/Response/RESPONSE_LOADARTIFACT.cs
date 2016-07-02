using Mogami.Service.Serialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Service.Response
{
	public class RESPONSE_LOADARTIFACT
	{

		#region プロパティ

		public DataArtifact Artifact { get; set; }

		public string FilePath { get; set; }

		#endregion プロパティ
	}
}
