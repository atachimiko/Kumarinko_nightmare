using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalib.Entity
{
	/// <summary>
	/// インスタンスの生成情報を追跡するためのフィールドをエンティティに提供するためのインターフェース
	/// </summary>
	public interface IAuditableEntity
	{
		DateTime CreatedDate { get; set; }

		string CreatedBy { get; set; }

		DateTime UpdatedDate { get; set; }

		string UpdatedBy { get; set; }
	}
}