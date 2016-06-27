using Akalib.Entity.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Model.Repository
{
	/// <summary>
	/// Categoryのリポジトリ
	/// </summary>
	public class CategoryRepository : GenericRepository<Category>
	{

		#region コンストラクタ

		public CategoryRepository(DbContext context)
			: base(context)
		{
		}

		#endregion コンストラクタ

		#region メソッド

		/// <summary>
		/// Categoryの読み込み
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Category Load(long id)
		{
			return _dbset.Where(x => x.Id == id).FirstOrDefault();
		}

		#endregion メソッド
	}
}
