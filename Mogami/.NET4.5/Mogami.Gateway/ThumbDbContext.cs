using Akalib.Entity;
using log4net;
using Mogami.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Gateway
{
	public sealed class ThumbDbContext : AtDbContext
	{


		#region フィールド

		public static SQLiteConnectionStringBuilder SDbConnection;
		private static ILog LOG = LogManager.GetLogger(typeof(ThumbDbContext));

		#endregion フィールド


		#region コンストラクタ

		public ThumbDbContext()
		: base(new SQLiteConnection(SDbConnection.ConnectionString), true)
		{
		}

		#endregion コンストラクタ


		#region プロパティ

		public DbSet<ApMetadata> ApMetadatas { get; set; }
		public DbSet<Thumbnail> Thumbnails { get; set; }

		#endregion プロパティ

	}
}
