﻿using Akalib.Entity;
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
	public sealed class AppDbContext : AtDbContext
	{


		#region フィールド

		public static SQLiteConnectionStringBuilder SDbConnection;
		private static ILog LOG = LogManager.GetLogger(typeof(AppDbContext));

		#endregion フィールド


		#region コンストラクタ

		public AppDbContext()
		: base(new SQLiteConnection(SDbConnection.ConnectionString), true)
		{
		}

		#endregion コンストラクタ


		#region プロパティ

		public DbSet<ApMetadata> ApMetadatas { get; set; }
		public DbSet<Artifact> Artifacts { get; set; }
		public DbSet<FileMappingInfo> FileMappingInfos { get; set; }
		public DbSet<T_Artifact2Category> Relations_Artifact2Category { get; set; }
		public DbSet<Workspace> Workspaces { get; set; }

		#endregion プロパティ


		#region メソッド

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Configure StudentId as PK for StudentAddress
			modelBuilder.Entity<T_Artifact2Category>()
				.HasKey(e => e.Id);
			// Configure StudentId as FK for StudentAddress
			modelBuilder.Entity<Artifact>()
						.HasOptional(s => s.Category) // Mark StudentAddress is optional for Student
						.WithRequired(ad => ad.Artifact); // Create inverse relationship

			// Tag <-> Artifact
			modelBuilder.Entity<Tag>()
				.HasMany(c => c.Artifacts)
				.WithMany(p => p.Tags)
				.Map(m =>
				{
					m.MapLeftKey("TagId");
					m.MapRightKey("ArtifactId");
					m.ToTable("TS_Artifact2Tag");
				});
		}

		protected override System.Data.Entity.Validation.DbEntityValidationResult ValidateEntity(System.Data.Entity.Infrastructure.DbEntityEntry entityEntry, IDictionary<object, object> items)
		{
			if (entityEntry.Entity is Artifact)
			{
				var artifact = entityEntry.Entity as Artifact;

				if (entityEntry.CurrentValues.GetValue<string>("Title") == "")
				{
					var list = new List<System.Data.Entity.Validation.DbValidationError>();
					list.Add(new System.Data.Entity.Validation.DbValidationError("Title", "Title is required"));

					return new System.Data.Entity.Validation.DbEntityValidationResult(entityEntry, list);
				}
			}
			else if (entityEntry.Entity is Category)
			{
				var category = entityEntry.Entity as Category;

				if (entityEntry.CurrentValues.GetValue<string>("Name") == "")
				{
					var list = new List<System.Data.Entity.Validation.DbValidationError>();
					list.Add(new System.Data.Entity.Validation.DbValidationError("Name", "Name is required"));

					return new System.Data.Entity.Validation.DbEntityValidationResult(entityEntry, list);
				}
				if (category.ParentCategory == null)
				{
					var list = new List<System.Data.Entity.Validation.DbValidationError>();
					list.Add(new System.Data.Entity.Validation.DbValidationError("ParentCategory", "ParentCategory is required"));

					return new System.Data.Entity.Validation.DbEntityValidationResult(entityEntry, list);
				}
			}

			return base.ValidateEntity(entityEntry, items);
		}

		#endregion メソッド

	}
}

