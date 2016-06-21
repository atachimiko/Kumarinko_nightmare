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

namespace Mogami.Activity.Feature.Thumbnail
{

	public sealed class DeleteThumbnail : CodeActivity
	{


		#region プロパティ

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

			switch (readKey)
			{
				case "Artifact":
					DeleteeArtifact(workflowContext, pstack);
					break;
				case "Misc":
					DeleteMisc(workflowContext, pstack);
					break;
				default:
					throw new ApplicationException();
			}
		}

		private void DeleteeArtifact(IWorkflowContext workflowContext, ParameterStack pstack)
		{
			var artifact = pstack.GetValue<ImageArtifact>(ActivityParameterStack.TARGET);
			var thumbnailManager = workflowContext.ThumbnailManager;
			if (artifact == null) throw new ArgumentNullException();

			thumbnailManager.RemoveThumbnail(artifact.ThumbnailKey);
		}

		private void DeleteMisc(IWorkflowContext workflowContext, ParameterStack pstack)
		{
			var key = pstack.GetValue<string>(ActivityParameterStack.GENERATETHUMBNAIL_KEY);

			// Guard
			Ensure.That(key).IsNotNullOrEmpty();

			var thumbnailManager = workflowContext.ThumbnailManager;
			thumbnailManager.RemoveThumbnail(key);
		}

		#endregion メソッド
	}
}
