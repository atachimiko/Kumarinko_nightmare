using Akalib.Entity.Repository;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Model.Repository
{
	public class TagRepository : GenericRepository<Tag>
	{

		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(TagRepository));

		#endregion フィールド


		#region コンストラクタ

		public TagRepository(DbContext context)
			: base(context)
		{
		}

		#endregion コンストラクタ


		#region メソッド

		public Tag Load(long id)
		{
			return _dbset.Where(x => x.Id == id).FirstOrDefault();
		}

		#endregion メソッド

	}
}
