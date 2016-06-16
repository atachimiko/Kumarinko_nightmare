using EnsureThat;
using Mogami.Contrib.Akalib;
using Mogami.Core.Definication;
using Mogami.Core.Infrastructure;
using Mogami.Core.Spp;
using Mogami.Model;
using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Mogami.Activity.Feature.VFS
{
	/// <summary>
	/// 
	/// </summary>
	public class CreateACLFile : CodeActivity<bool>
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

			var target = pstack.GetValue<FileMappingInfo>(ActivityParameterStack.TARGET);
			var workspace = pstack.GetValue<Workspace>(ActivityParameterStack.WORKSPACE);

			// Guard
			Ensure.That(target).IsNotNull();
			Ensure.That(workspace).IsNotNull();

			var aclfilepath = Path.Combine(workspace.WorkspacePath, target.MappingFilePath);
			aclfilepath += ".aclgene";

			// ACLファイルの作成を行います。
			//
			var data = new AclFileStructure();
			data.Version = AclFileStructure.CURRENT_VERSION;
			data.LastUpdate = DateTime.Now;
			data.Data = new KeyValuePair<string, string>[]{
				new KeyValuePair<string,string>("ACLHASH", target.AclHash)
			};

			using (var file = File.Create(aclfilepath))
			{
				Serializer.Serialize(file, data);
			}

			return true;
		}

		#endregion メソッド
	}
}
