using Akalib.Entity.Repository;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Model.Repository
{
	public class WorkspaceRepository : GenericRepository<Workspace>
	{

		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(WorkspaceRepository));

		#endregion フィールド


		#region コンストラクタ

		public WorkspaceRepository(DbContext context)
			: base(context)
		{
		}

		#endregion コンストラクタ


		#region メソッド
		/// <summary>
		/// エンティティの読み込み(静的メソッド)
		/// </summary>
		/// <remarks>
		/// エンティティの読み込みをワンライナーで記述できます。
		/// </remarks>
		/// <param name="context"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Workspace Load(DbContext context, long id)
		{
			var repo = new WorkspaceRepository(context);
			return repo.Load(id);
		}
		/// <summary>
		/// 指定のファイルパスにマッチするワークスペースを探します
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public Workspace FindByPathMatch(string matchFilePath)
		{
			var normalize = Path.GetFullPath(matchFilePath);
			string[] stArrayData = normalize.Split(Path.DirectorySeparatorChar);
			var st = new Stack<string>(stArrayData);

			while (st.Count > 0)
			{
				st.Pop();

				bool isFirst = true;
				StringBuilder sb = new StringBuilder();
				foreach (var s in st.Reverse())
				{
					if (!isFirst)
					{
						sb.Append(Path.DirectorySeparatorChar);
					}
					sb.Append(s);
					isFirst = false;
				}

				LOG.DebugFormat("Path:{0}", sb);
				string findPath = sb.ToString();

				var workspace = _dbset.Where(x => x.WorkspacePath == findPath).FirstOrDefault();
				if (workspace != null) return workspace;
			}

			return null;
		}

		public Workspace Load(long id)
		{
			return _dbset.Where(x => x.Id == id).FirstOrDefault();
		}

		#endregion メソッド
	}
}
