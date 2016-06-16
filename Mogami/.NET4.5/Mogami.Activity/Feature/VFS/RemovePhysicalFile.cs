using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Mogami.Contrib.Akalib;
using Mogami.Core.Definication;
using Mogami.Model;
using EnsureThat;
using Mogami.Core.Infrastructure;
using System.IO;

namespace Mogami.Activity.Feature.VFS
{

	public sealed class RemovePhysicalFile : CodeActivity
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
			var workspace = pstack.GetValue<Workspace>(ActivityParameterStack.WORKSPACE);

			// Guard
			Ensure.That(target).IsNotNull();
			Ensure.That(workspace).IsNotNull();

			var file = new FileInfo(Path.Combine(workspace.PhysicalPath, target.MappingFilePath));
			if (!file.Exists) throw new ApplicationException();
			file.Delete();
		}

		#endregion メソッド
	}
}
