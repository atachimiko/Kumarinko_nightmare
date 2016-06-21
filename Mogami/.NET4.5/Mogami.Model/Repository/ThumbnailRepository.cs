using Akalib.Entity.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Model.Repository
{
	public sealed class ThumbnailRepository : GenericRepository<Thumbnail>
	{


		#region コンストラクタ

		public ThumbnailRepository(DbContext context) : base(context)
		{
		}

		#endregion コンストラクタ


		#region メソッド

		public Thumbnail load(long id)
		{
			return _dbset.Where(x => x.Id == id).FirstOrDefault();
		}

		public IList<Thumbnail> FindFromKey(string key)
		{
			return _dbset.Where(x => x.ThumbnailKey == key).ToArray();
		}

		#endregion メソッド

	}
}
