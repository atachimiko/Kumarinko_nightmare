using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Mogami.Contrib.Akalib;
using Mogami.Core.Infrastructure;
using Mogami.Model;
using Mogami.Core.Definication;
using System.Data.Entity;
using Mogami.Model.Repository;

namespace Mogami.Activity.Feature.Content
{

	public sealed class ReadArtifact : CodeActivity
	{

		#region プロパティ

		[RequiredArgument]
		public InArgument<string> OutputName { get; set; }

		[RequiredArgument]
		public InArgument<ParameterStack> Parameter { get; set; }
		[RequiredArgument]
		public InArgument<string> ReadKey { get; set; }

		#endregion プロパティ


		#region メソッド

		protected override void Execute(CodeActivityContext context)
		{
			IWorkflowContext workflowContext = context.GetExtension<IWorkflowContext>();
			ParameterStack pstack = context.GetValue<ParameterStack>(this.Parameter);

			var readKey = this.ReadKey.Get(context);
			var outputname = context.GetValue<string>(OutputName);

			switch (readKey)
			{
				case "FileMappingInfo":
					var target = pstack.GetValue<FileMappingInfo>(ActivityParameterStack.TARGET);
					var a = ReadByFileMappingInfo(target,workflowContext.DbContext);
					pstack.SetValue(outputname, a);
					break;
				default:
					throw new ApplicationException("不明なReadKeyです。");
			}

		}

		private Artifact ReadByFileMappingInfo(FileMappingInfo fileMappingInfo,DbContext dbcontext)
		{
			var repo = new ArtifactRepository(dbcontext);
			return repo.LoadByFileMappingInfo(fileMappingInfo);
		}

		#endregion メソッド
	}
}
