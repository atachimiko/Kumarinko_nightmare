using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Core.Spp
{
	[ProtoContract]
	public class AclFileStructure
	{


		#region フィールド

		public const int CURRENT_VERSION = 1;

		#endregion フィールド


		#region コンストラクタ

		public AclFileStructure()
		{
			this.MagicCode = "ACLGENE";
			this.Data = new KeyValuePair<string, string>[0];
		}

		#endregion コンストラクタ


		#region プロパティ

		[ProtoMember(4)]
		public KeyValuePair<string, string>[] Data { get; set; }

		[ProtoMember(3)]
		public DateTime LastUpdate { get; set; }

		[ProtoMember(1)]
		public string MagicCode { get; set; }

		[ProtoMember(2)]
		public Int32 Version { get; set; }

		#endregion プロパティ


		#region メソッド

		/// <summary>
		/// Data配列から、キーに一致する値を取得します
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string FindKeyValue(string key)
		{
			var r = from p in this.Data
					where p.Key == key
					select p;
			var prop = r.FirstOrDefault();
			return prop.Value;
		}

		#endregion メソッド
	}
}
