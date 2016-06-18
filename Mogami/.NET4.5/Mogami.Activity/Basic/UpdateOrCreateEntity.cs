using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Akalib.Entity;
using Mogami.Core.Infrastructure;
using Mogami.Contrib.Akalib;
using EnsureThat;

namespace Mogami.Activity.Basic
{

	public sealed class UpdateOrCreateEntity : CodeActivity<IEntity<long>>
	{
		#region プロパティ

		/// <summary>
		/// 
		/// </summary>
		public InArgument<IEntity<long>> Entity { get; set; }

		public InArgument<ParameterStack> Parameter { get; set; }

		/// <summary>
		/// Parameterを使用する場合に、対象を指定するキー
		/// </summary>
		public InArgument<string> ParameterKey { get; set; }
		#endregion プロパティ

		#region メソッド

		// アクティビティが値を返す場合は、CodeActivity<TResult> から派生して、 Execute メソッドから値を返します。
		protected override IEntity<long> Execute(CodeActivityContext context)
		{
			IWorkflowContext workflowContext = context.GetExtension<IWorkflowContext>();

			var entity = this.Entity.Get(context);
			if (entity == null)
			{
				ParameterStack pstack = context.GetValue<ParameterStack>(this.Parameter);
				string key = this.ParameterKey.Get(context);
				entity = pstack.GetValue<IEntity<long>>(key);
			}

			// Guard
			Ensure.That(entity).IsNotNull();

			var dbset = workflowContext.DbContext.Set(entity.GetType());
			if (entity.Id == 0) dbset.Add(entity);
			else
			{
				workflowContext.DbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
			}

			return entity;
		}

		#endregion メソッド
	}
}
