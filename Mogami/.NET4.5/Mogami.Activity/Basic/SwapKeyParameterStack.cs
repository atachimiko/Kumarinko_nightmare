using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Mogami.Contrib.Akalib;
using Mogami.Core.Infrastructure;

namespace Mogami.Activity.Basic
{

	public sealed class SwapKeyParameterStack : CodeActivity
	{
		#region プロパティ

		/// <summary>
		/// リネーム後のキー
		/// </summary>
		[RequiredArgument]
		public InArgument<string> DistKey { get; set; }

		/// <summary>
		/// ParameterStack
		/// </summary>
		[RequiredArgument]
		public InArgument<ParameterStack> Parameter { get; set; }

		/// <summary>
		/// リネーム対象のキー
		/// </summary>
		[RequiredArgument]
		public InArgument<string> SrcKey { get; set; }

		#endregion プロパティ


		#region メソッド

		protected override void Execute(CodeActivityContext context)
		{
			IWorkflowContext workflowContext = context.GetExtension<IWorkflowContext>();
			ParameterStack pstack = context.GetValue<ParameterStack>(this.Parameter);
			string distKey = context.GetValue<string>(this.DistKey);
			string srcKey = context.GetValue<string>(this.SrcKey);

			var obj = pstack.GetValue(srcKey, false);
			pstack.SetValue(distKey, obj);

			pstack.ClearKey(srcKey);
		}

		#endregion メソッド
	}
}
