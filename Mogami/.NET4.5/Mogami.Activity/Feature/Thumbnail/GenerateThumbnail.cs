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
using Mogami.Model;
using Mogami.Model.Repository;

namespace Mogami.Activity.Feature.Thumbnail
{

	public sealed class GenerateThumbnail : CodeActivity
	{


		#region フィールド

		private static ILog LOG = LogManager.GetLogger(typeof(GenerateThumbnail));

		#endregion フィールド


		#region プロパティ

		[RequiredArgument]
		public InArgument<ParameterStack> Parameter { get; set; }

		[RequiredArgument]
		public InArgument<string> ReadKey { get; set; }

		#endregion プロパティ

		#region メソッド

		// アクティビティが値を返す場合は、CodeActivity<TResult> から派生して、
		// Execute メソッドから値を返します。
		protected override void Execute(CodeActivityContext context)
		{
			IWorkflowContext workflowContext = context.GetExtension<IWorkflowContext>();
			ParameterStack pstack = context.GetValue<ParameterStack>(this.Parameter);
			var readKey = this.ReadKey.Get(context);

			
			switch (readKey)
			{
				case "Artifact":
					GenerateArtifact(workflowContext, pstack);
					break;
				case "Misc":
					GenerateMisc(workflowContext, pstack);
					break;
				default:
					throw new ApplicationException();
			}
		}

		/// <summary>
		/// Artifactから画像ファイルパスを取得し、サムネイルを生成します。
		/// </summary>
		/// <param name="workflowContext"></param>
		/// <param name="pstack"></param>
		private void GenerateArtifact(IWorkflowContext workflowContext, ParameterStack pstack)
		{
			var artifact = pstack.GetValue<ImageArtifact>(ActivityParameterStack.TARGET);
			if (artifact == null) throw new ArgumentNullException();

			var imageArtifactRepository = new ImageArtifactRepository(workflowContext.DbContext);
			artifact = imageArtifactRepository.Load(artifact.Id);

			var fullpath = System.IO.Path.Combine(artifact.FileMappingInfo.Workspace.PhysicalPath, artifact.FileMappingInfo.MappingFilePath);
			var thumbnailManager = workflowContext.ThumbnailManager;
			if (string.IsNullOrEmpty(artifact.FileMappingInfo.ThumbnailKey))
			{
				var thumbnailKey = thumbnailManager.BuildThumbnail(null, fullpath);
				artifact.FileMappingInfo.ThumbnailKey = thumbnailKey;
			}
			else
			{
				var thumbnailKey = thumbnailManager.BuildThumbnail(artifact.FileMappingInfo.ThumbnailKey, fullpath);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="workflowContext"></param>
		/// <param name="pstack"></param>
		private void GenerateMisc(IWorkflowContext workflowContext, ParameterStack pstack)
		{
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
