using System.Runtime.Serialization;

namespace Mogami.Service.Response
{
	[DataContract]
	public abstract class RESPONSEAPI_BASE
	{

		#region プロパティ
		[DataMember]
		public string Message { get; set; }

		[DataMember]
		public bool Success { get; set; }

		#endregion プロパティ
	}
}