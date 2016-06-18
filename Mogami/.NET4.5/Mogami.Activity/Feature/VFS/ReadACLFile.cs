using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Mogami.Core.Spp;
using Mogami.Contrib.Akalib;
using log4net;
using Mogami.Core.Infrastructure;
using Mogami.Core.Definication;
using Mogami.Model;
using System.IO;
using ProtoBuf;

namespace Mogami.Activity.Feature.VFS
{

	public sealed class ReadACLFile : CodeActivity<AclFileStructure>
	{

		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(ReadACLFile));

		#endregion フィールド


		#region プロパティ

		[RequiredArgument]
		public InArgument<ParameterStack> Parameter { get; set; }

		#endregion プロパティ



		#region メソッド

		protected override AclFileStructure Execute(CodeActivityContext context)
		{
			IWorkflowContext workflowContext = context.GetExtension<IWorkflowContext>();
			ParameterStack pstack = context.GetValue<ParameterStack>(this.Parameter);

			var workspace = pstack.GetValue<Workspace>(ActivityParameterStack.WORKSPACE);
			var relative = pstack.GetValue<string>(ActivityParameterStack.WORKSPACE_FILEPATH);

			var virtualFileFullPath = Path.Combine(workspace.WorkspacePath, relative);

			var fileInfo = new FileInfo(virtualFileFullPath);
			if (!fileInfo.Exists)
			{
				LOG.Warn("対象ファイルが、見つかりませんでした。");
				return null;
			}
			if (fileInfo.Extension != ".aclgene")
			{
				LOG.Warn("対象ファイルが、ACLファイルではありません。");
				return null;
			}

			pstack.SetValue(ActivityParameterStack.WORKSPACE_FILEINFO, fileInfo);

			using (var file = File.OpenRead(virtualFileFullPath))
			{
				return Serializer.Deserialize<AclFileStructure>(file);
			}
		}

		#endregion メソッド
	}
}
