using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Service.Serialized
{
	/// <summary>
	/// Artifactデータモデルのシリアライズ公開用クラスです。
	/// </summary>
	[DataContract]
	public class DataArtifact
	{
		[DataMember(Order = 0)]
		public long Id { get; set; }

		[DataMember(Order = 1)]
		public string IdentifyKey { get; set; }

		[DataMember(Order = 2)]
		public string Title { get; set; }

		[DataMember(Order = 3)]
		public string ThumbnailKey { get; set; }
	}
}
