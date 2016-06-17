using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Mogami.Contrib.Akalib;
using Mogami.Core.Infrastructure;

namespace Mogami.Activity.Basic
{

	public sealed class ParameterStackAlias : CodeActivity
	{
		#region プロパティ

		/// <summary>
		/// エイリアス名
		/// </summary>
		[RequiredArgument]
		public InArgument<string> AliasName { get; set; }

		/// <summary>
		/// エイリアスを作成するキーの名称
		/// </summary>
		[RequiredArgument]
		public InArgument<string> Key { get; set; }

		[RequiredArgument]
		public InArgument<ParameterStack> Parameter { get; set; }

		#endregion プロパティ



		#region メソッド

		protected override void Execute(CodeActivityContext context)
		{
			IWorkflowContext workflowContext = context.GetExtension<IWorkflowContext>();
			ParameterStack pstack = context.GetValue<ParameterStack>(this.Parameter);
			string aliasName = context.GetValue<string>(this.AliasName);
			string key = context.GetValue<string>(this.Key);

			pstack.SetAlias(key, aliasName);
		}

		#endregion メソッド
	}
}
