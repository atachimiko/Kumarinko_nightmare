using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Mogami.Core.Infrastructure;
using Mogami.Core.Definication;
using Mogami.Contrib.Akalib;
using Mogami.Model;
using EnsureThat;
using System.IO;

namespace Mogami.Activity.Feature.VFS
{

	public sealed class RemoveACLFile : CodeActivity
	{

		#region プロパティ

		[RequiredArgument]
		public InArgument<ParameterStack> Parameter { get; set; }

		#endregion プロパティ


		#region メソッド

		protected override void Execute(CodeActivityContext context)
		{
			IWorkflowContext workflowContext = context.GetExtension<IWorkflowContext>();
			ParameterStack pstack = context.GetValue<ParameterStack>(this.Parameter);

			var target = pstack.GetValue<FileMappingInfo>(ActivityParameterStack.TARGET);
			var filepath = pstack.GetValue<string>(ActivityParameterStack.WORKSPACE_FILEPATH);
			var workspace = pstack.GetValue<Workspace>(ActivityParameterStack.WORKSPACE);

			// Guard
			Ensure.That(target).IsNotNull();
			Ensure.That(filepath).IsNotNullOrEmpty();
			Ensure.That(workspace).IsNotNull();

			if (string.IsNullOrEmpty(filepath))
			{

				var aclfilepath = Path.Combine(workspace.WorkspacePath, target.MappingFilePath);
				aclfilepath += Path.DirectorySeparatorChar + ".aclgene";

				try
				{
					File.Delete(aclfilepath);
				}
				catch (Exception)
				{

				}
			}
			else
			{

				try
				{
					File.Delete(filepath);
				}
				catch (Exception)
				{

				}
			}
		}

		#endregion メソッド
	}
}
