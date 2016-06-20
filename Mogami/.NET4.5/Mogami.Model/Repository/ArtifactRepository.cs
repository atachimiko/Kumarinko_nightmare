using Akalib.Entity.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Model.Repository
{
	public class ArtifactRepository : GenericRepository<Artifact>
	{

		#region コンストラクタ

		public ArtifactRepository(DbContext context)
			: base(context)
		{

		}

		#endregion コンストラクタ

		#region メソッド

		/// <summary>
		/// Artifactを読み込みます
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Artifact Load(long id)
		{
			return _dbset.Where(x => x.Id == id).FirstOrDefault();
		}

		#endregion メソッド
	}
}
