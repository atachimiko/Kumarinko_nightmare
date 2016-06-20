using Akalib.Entity.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Model.Repository
{
	public class ImageArtifactRepository : GenericRepository<ImageArtifact>
	{


		#region コンストラクタ

		public ImageArtifactRepository(DbContext context)
			: base(context)
		{

		}

		#endregion コンストラクタ


		#region メソッド

		public ImageArtifact Load(long id)
		{
			return _dbset.Where(x => x.Id == id).FirstOrDefault();
		}

		#endregion メソッド

	}
}
