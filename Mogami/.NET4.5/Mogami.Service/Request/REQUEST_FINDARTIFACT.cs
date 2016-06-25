using Mogami.Core.Expression;
using Mogami.Service.Construction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Service.Request
{
	[DataContract]
	public class REQUEST_FINDARTIFACT
	{

		#region プロパティ

		/// <summary>
		/// 戻り値にサムネイルを含めるか
		/// </summary>
		public bool EnableThumbnailBytes { get; set; }

		/// <summary>
		/// 検索キー(Category)
		/// </summary>
		[DataMember]
		public long TargetId { get; set; }

		/// <summary>
		/// 検索条件に使用する対象選択を設定します。
		/// </summary>
		[DataMember]
		public FINDTARGET_SELECTOR TargetType { get; set; }

		#endregion プロパティ
	}
}
