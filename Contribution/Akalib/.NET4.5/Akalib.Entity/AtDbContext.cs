using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Akalib.Entity
{
	/// <summary>
	///
	/// </summary>
	public abstract class AtDbContext : DbContext
	{
		#region Public Constructors

		public AtDbContext(DbConnection dbconnection, bool contextOwnsConnection)
			: base(dbconnection, contextOwnsConnection)
		{
		}

		#endregion Public Constructors

		#region Public Methods

		public override int SaveChanges()
		{
			var modifiedEntries = ChangeTracker.Entries()
				.Where(x => x.Entity is IAuditableEntity
					&& (x.State == System.Data.Entity.EntityState.Added || x.State == System.Data.Entity.EntityState.Modified));

			foreach (var entry in modifiedEntries)
			{
				// IAuditableEntity
				IAuditableEntity auditableEntity = entry.Entity as IAuditableEntity;
				if (auditableEntity != null) ProcAuditableEntity(entry, auditableEntity);

				// ISaveEntity
				ISaveEntity saveEntity = entry.Entity as ISaveEntity;
				if (saveEntity != null) ProcSaveEntity(saveEntity);
			}

			return base.SaveChanges();
		}

		#endregion Public Methods

		#region Private Methods

		private void ProcAuditableEntity(DbEntityEntry entry, IAuditableEntity auditableEntity)
		{
			string identityName = Thread.CurrentPrincipal.Identity.Name;
			DateTime now = DateTime.Now;

			if (entry.State == System.Data.Entity.EntityState.Added)
			{
				auditableEntity.CreatedBy = identityName;
				auditableEntity.CreatedDate = now;
			}
			else
			{
				base.Entry(auditableEntity).Property(x => x.CreatedBy).IsModified = false;
				base.Entry(auditableEntity).Property(x => x.CreatedDate).IsModified = false;
			}

			auditableEntity.UpdatedBy = identityName;
			auditableEntity.UpdatedDate = now;
		}

		private void ProcSaveEntity(ISaveEntity entity)
		{
			entity.OnSave();
		}

		#endregion Private Methods
	}
}