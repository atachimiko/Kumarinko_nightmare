using Mogami.Applus.Manager;
using Mogami.Core.Infrastructure;
using Mogami.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Applus
{
	public class WorkflowExtention : IWorkflowContext
	{


		#region フィールド

		AppDbContext _appdb;

		#endregion フィールド


		#region コンストラクタ

		public WorkflowExtention(AppDbContext appdb)
		{
			this._appdb = appdb;
		}

		#endregion コンストラクタ


		#region プロパティ

		public AppDbContext AppDbContext { get { return this._appdb; } }

		public System.Data.Entity.DbContext DbContext
		{
			get { return this._appdb; }
		}

		public IThumbnailManager ThumbnailManager { get { return new ThumbnailManager(); } }

		#endregion プロパティ
	}
}
