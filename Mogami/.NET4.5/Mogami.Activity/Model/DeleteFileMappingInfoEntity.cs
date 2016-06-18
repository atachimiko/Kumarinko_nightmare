using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Mogami.Model;
using System.ComponentModel;
using Mogami.Contrib.Akalib;
using Mogami.Core.Infrastructure;
using Mogami.Model.Repository;

namespace Mogami.Activity.Model
{

	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// 削除するエンティティは、Entityまたは、パラメータスタックから取得します。
	/// </remarks>
	public sealed class DeleteFileMappingInfoEntity : CodeActivity
	{


		#region プロパティ

		/// <summary>
		/// 削除対象のエンティティ
		/// </summary>
		public InArgument<FileMappingInfo> Entity { get; set; }

		/// <summary>
		/// ParameterTargetKey
		/// </summary>
		[Category("Employee Control")]
		[OverloadGroup("ParameterStack")]
		public InArgument<ParameterStack> Parameter { get; set; }

		/// <summary>
		/// 削除対象をパラメータスタックから取得するキー
		/// </summary>
		[Category("Employee Control")]
		[OverloadGroup("ParameterStack")]
		public InArgument<string> ParameterTargetKey { get; set; }

		#endregion プロパティ



		#region メソッド

		protected override void Execute(CodeActivityContext context)
		{
			IWorkflowContext workflowContext = context.GetExtension<IWorkflowContext>();
			var repo = new FileMappingInfoRepository(workflowContext.DbContext);

			var entity = this.Entity.Get(context);
			if (entity != null)
			{
				repo.Delete(entity);
			}
			else
			{
				ParameterStack pstack = context.GetValue<ParameterStack>(this.Parameter);
				var targetEntityKey = this.ParameterTargetKey.Get(context);
				entity = pstack.GetValue(targetEntityKey, true) as FileMappingInfo;
				if (entity != null)
					repo.Delete(entity);
			}
		}

		#endregion メソッド
	}
}
