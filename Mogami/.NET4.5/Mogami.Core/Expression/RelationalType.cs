using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Core.Expression
{
	public enum RelationalType
	{
		/// <summary>
		/// ＝＝
		/// </summary>
		EQUAL,
		/// <summary>
		/// ！＝
		/// </summary>
		UNEQUAL,
		/// <summary>
		/// ＞
		/// </summary>
		GREATER,
		/// <summary>
		/// ＞＝
		/// </summary>
		GREATER_EQUAL,
		/// <summary>
		/// ＜
		/// </summary>
		LESS,
		/// <summary>
		/// ＜＝
		/// </summary>
		LESS_EQUAL
	}
}
