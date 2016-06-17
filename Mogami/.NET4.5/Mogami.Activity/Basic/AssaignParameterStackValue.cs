using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Mogami.Contrib.Akalib;

namespace Mogami.Activity.Basic
{

	public sealed class AssaignParameterStackValue : CodeActivity
	{

		#region プロパティ

		[RequiredArgument]
		public InArgument<string> Key { get; set; }

		[RequiredArgument]
		public InArgument<ParameterStack> Parameter { get; set; }
		[RequiredArgument]
		public InArgument<object> Value { get; set; }

		#endregion プロパティ


		#region メソッド

		protected override void Execute(CodeActivityContext context)
		{
			ParameterStack pstack = context.GetValue<ParameterStack>(this.Parameter);
			object value = context.GetValue<object>(this.Value);
			string key = context.GetValue<string>(this.Key);

			pstack.SetValue(key, value);
		}

		#endregion メソッド
	}
}
