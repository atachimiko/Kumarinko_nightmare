using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Mogami.Contrib.Akalib;
using Mogami.Core.Definication;
using Mogami.Core.Infrastructure;
using Mogami.Model;
using System.IO;

namespace Mogami.Activity.Feature.VFS
{

	public sealed class IsACLFile : CodeActivity<bool>
	{

		#region プロパティ

		[RequiredArgument]
		public InArgument<ParameterStack> Parameter { get; set; }

		#endregion プロパティ


		#region メソッド

		protected override bool Execute(CodeActivityContext context)
		{
			IWorkflowContext workflowContext = context.GetExtension<IWorkflowContext>();
			ParameterStack pstack = context.GetValue<ParameterStack>(this.Parameter);


			var filepath = pstack.GetValue<string>(ActivityParameterStack.WORKSPACE_FILEPATH);
			var workspace = pstack.GetValue<Workspace>(ActivityParameterStack.WORKSPACE);


			string path = Path.Combine(workspace.WorkspacePath, filepath);
			var fi = new FileInfo(path);

			// ACLファイルかどうかの判定には、拡張子で確認します。
			if (fi.Extension == ".aclgene")
				return true;

			return false;
		}

		#endregion メソッド
	}
}
