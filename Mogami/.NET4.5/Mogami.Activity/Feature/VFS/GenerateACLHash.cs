using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace Mogami.Activity.Feature.VFS
{
	
	public sealed class GenerateACLHash : CodeActivity<string>
	{

		#region メソッド

		protected override string Execute(CodeActivityContext context)
		{
			return Guid.NewGuid().ToString("N");
		}

		#endregion メソッド
	}
}
