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
	/// 
	/// </summary>
	public class FileMappingInfoRepository : GenericRepository<FileMappingInfo>
	{


		#region コンストラクタ

		public FileMappingInfoRepository(DbContext context)
			: base(context)
		{
		}

		#endregion コンストラクタ


		#region メソッド

		/// <summary>
		/// エンティティの読み込み(静的メソッド)
		/// </summary>
		/// <param name="context"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public static FileMappingInfo Load(DbContext context, long id)
		{
			var repo = new FileMappingInfoRepository(context);
			return repo.Load(id);
		}

		public FileMappingInfo Load(long id)
		{
			return _dbset.Where(x => x.Id == id).FirstOrDefault();
		}

		/// <summary>
		/// AclHashが一致するエンティティを取得します。
		/// </summary>
		/// <param name="aclHash"></param>
		/// <returns></returns>
		public FileMappingInfo LoadByAclHash(string aclHash)
		{
			return _dbset.Where(x => x.AclHash == aclHash).FirstOrDefault();
		}

		/// <summary>
		/// MappingFilePathが一致するエンティティを取得します。
		/// </summary>
		/// <remarks>
		/// MappingFilePathは、ディレクトリ空間からの相対パスです。
		/// </remarks>
		/// <param name="path"></param>
		/// <returns></returns>
		public FileMappingInfo LoadByPath(string path)
		{
			return _dbset.Where(x => x.MappingFilePath == path).FirstOrDefault();
		}

		#endregion メソッド

	}
}
