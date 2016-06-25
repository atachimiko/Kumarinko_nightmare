using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Core.Expression
{
	public class SearchExpression
	{

		#region コンストラクタ

		public SearchExpression()
		{
			SubConnection = LogicalType.AND;
			SubExpression = new List<SearchExpression>();
		}

		#endregion コンストラクタ

		#region プロパティ

		/// <summary>
		/// 
		/// </summary>
		public ExpKeyValue Left { get; set; }

		/// <summary>
		/// 比較演算子
		/// </summary>
		public RelationalType Relational { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public ExpKeyValue Right { get; set; }
		/// <summary>
		/// Subとの接続時の論理演算子
		/// </summary>
		public LogicalType SubConnection { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public List<SearchExpression> SubExpression { get; set; }

		#endregion プロパティ
	}
}
