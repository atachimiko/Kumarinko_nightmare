using Akalib.Entity.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Model.Repository
{

	public sealed class ApMetadataRepository : GenericRepository<ApMetadata>
	{

		#region コンストラクタ

		public ApMetadataRepository(DbContext context)
			: base(context)
		{
		}

		#endregion コンストラクタ


		#region メソッド

		public override IEnumerable<ApMetadata> GetAll()
		{
			return _entities.Set<ApMetadata>().AsEnumerable();
		}

		public ApMetadata load(long id)
		{
			return _dbset.Where(x => x.Id == id).FirstOrDefault();
		}

		#endregion メソッド

	}
}
