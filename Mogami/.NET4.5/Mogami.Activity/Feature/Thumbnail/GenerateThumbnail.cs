using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Mogami.Contrib.Akalib;
using Mogami.Core.Definication;
using Mogami.Core.Infrastructure;
using log4net;
using EnsureThat;

namespace Mogami.Activity.Feature.Thumbnail
{

	public sealed class GenerateThumbnail : CodeActivity
	{

		#region フィールド

		private static ILog LOG = LogManager.GetLogger(typeof(GenerateThumbnail));

		#endregion フィールド


		#region プロパティ

		public InArgument<ParameterStack> Parameter { get; set; }

		#endregion プロパティ

		#region メソッド

		// アクティビティが値を返す場合は、CodeActivity<TResult> から派生して、
		// Execute メソッドから値を返します。
		protected override void Execute(CodeActivityContext context)
		{
			IWorkflowContext workflowContext = context.GetExtension<IWorkflowContext>();
			ParameterStack pstack = context.GetValue<ParameterStack>(this.Parameter);

			var key = pstack.GetValue<string>(ActivityParameterStack.GENERATETHUMBNAIL_KEY);
			var path = pstack.GetValue<string>(ActivityParameterStack.GENERATETHUMBNAIL_IMAGEPATH);


			// Guard
			Ensure.That(key).IsNotNullOrEmpty();
			Ensure.That(path).IsNotNullOrEmpty();

			var thumbnailManager = workflowContext.ThumbnailManager;
			thumbnailManager.BuildThumbnail(key, path);
		}

		#endregion メソッド
	}
}
