using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Core.Constructions
{
	/// <summary>
	/// カテゴリ区分
	/// </summary>
	[Flags]
	public enum CategoryType
	{
		/// <summary>
		/// システムカテゴリ
		/// </summary>
		SYSTEM,
		/// <summary>
		/// システムカテゴリ
		/// </summary>
		SYSTEM_RECYCLE,
		/// <summary>
		/// 
		/// </summary>
		APPLICATION,

	}
}
