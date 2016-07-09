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
		/// カテゴリをキーに、Artifactを検索する
		/// </summary>
		/// <param name="category"></param>
		/// <returns></returns>
		public IQueryable<Artifact> FindByCategory(Category category)
		{
			return _dbset.Where(prop => prop.Category.Category.Id == category.Id);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		public IQueryable<Artifact> FindByTag(Tag tag)
		{
			return from u in _dbset
				   from c in u.Tags
				   where c.Id == tag.Id
				   select u;
		}


		/// <summary>
		/// Artifactを読み込みます
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Artifact Load(long id)
		{
			return _dbset.Where(x => x.Id == id).FirstOrDefault();
		}

		public Artifact LoadByFileMappingInfo(FileMappingInfo filemappinginfo)
		{
			return _dbset.Where(prop => prop.FileMappingInfo.Id == filemappinginfo.Id).FirstOrDefault();
		}

		#endregion メソッド
	}
}
