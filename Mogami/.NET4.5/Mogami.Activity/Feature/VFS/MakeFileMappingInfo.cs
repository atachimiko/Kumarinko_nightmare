using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Mogami.Contrib.Akalib;
using Mogami.Core.Infrastructure;
using Mogami.Model;
using Mogami.Core.Definication;
using EnsureThat;
using System.IO;
using Mogami.Core.Model;

namespace Mogami.Activity.Feature.VFS
{

	public sealed class MakeFileMappingInfo : CodeActivity
	{

		#region プロパティ

		[RequiredArgument]
		public InArgument<string> Aclhash { get; set; }

		[RequiredArgument]
		public InArgument<string> OutputName { get; set; }

		[RequiredArgument]
		public InArgument<ParameterStack> Parameter { get; set; }

		#endregion プロパティ



		#region メソッド

		protected override void Execute(CodeActivityContext context)
		{
			IWorkflowContext workflowContext = context.GetExtension<IWorkflowContext>();
			ParameterStack pstack = context.GetValue<ParameterStack>(this.Parameter);

			var aclhash = context.GetValue<string>(Aclhash);
			var outputname = context.GetValue<string>(OutputName);

			var workspace = pstack.GetValue<Workspace>(ActivityParameterStack.WORKSPACE);
			var fileinfo = pstack.GetValue<FileSystemInfo>(ActivityParameterStack.WORKSPACE_FILEINFO);

			// Guard
			Ensure.That(workspace).IsNotNull();
			Ensure.That(fileinfo).IsNotNull();

			var entity = new FileMappingInfo
			{
				AclHash = aclhash,
				Workspace = workspace,
				MappingFilePath = workspace.TrimWorekspacePath(fileinfo.FullName, false),
			};

			pstack.SetValue(outputname, entity);
		}

		#endregion メソッド
	}
}
