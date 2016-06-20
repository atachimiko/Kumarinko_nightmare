using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Mogami.Core.Infrastructure;
using Mogami.Contrib.Akalib;
using Mogami.Model;
using Mogami.Core.Definication;
using EnsureThat;
using Mogami.Model.Repository;
using Akalib.String;

namespace Mogami.Activity.Feature.Content
{

	public sealed class MakeArtifact : CodeActivity
	{

		#region プロパティ

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

			var target = pstack.GetValue<FileMappingInfo>(ActivityParameterStack.TARGET);
			var outputname = context.GetValue<string>(OutputName);

			// Guard
			Ensure.That(target).IsNotNull();

			// FileMappingInfoがArtifactとの関連が存在する場合、
			// 新規のArtifactは作成できないので、例外を投げる。
			if (target.Id != 0L)
			{
				var r = new ArtifactRepository(workflowContext.DbContext);
				var a = r.LoadByFileMappingInfo(target);
				if (a != null) throw new ApplicationException("すでに作成済みのFileMappingInfoです。");
			}

			// 現Verでは画像のみ、メタ情報を生成できる。
			// (それ以外のファイルは、例外を投げる)
			if (target.Mimetype == "image/png")
			{
				var repo = new ImageArtifactRepository(workflowContext.DbContext);
				var entity = new ImageArtifact
				{
					Title = target.MappingFilePath,
					IdentifyKey = RandomAlphameric.RandomAlphanumeric(10),
					FileMappingInfo = target,
					ImageHeight = -1, // 未実装
					ImageWidth = -1, // 未実装
				};
				repo.Add(entity);

				pstack.SetValue(outputname, entity);
			}
			else
			{
				throw new ApplicationException("処理不能なMIMEタイプです");
			}
		}

		#endregion メソッド
	}
}
