using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using Mogami.Contrib.Akalib;
using Mogami.Core.Infrastructure;
using Mogami.Core.Definication;
using Mogami.Model;
using EnsureThat;
using System.IO;

namespace Mogami.Activity.Feature.VFS
{

	public sealed class MovePhysicalFile : CodeActivity
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
			var filepath = pstack.GetValue<string>(ActivityParameterStack.WORKSPACE_PHYFILEPATH_OLD);
			var workspace = pstack.GetValue<Workspace>(ActivityParameterStack.WORKSPACE);

			// Guard
			Ensure.That(target).IsNotNull();
			Ensure.That(workspace).IsNotNull();
			Ensure.That(filepath).IsNotNull();

			var srcFileInfo = new FileInfo(Path.Combine(workspace.PhysicalPath, filepath));
			if (!File.Exists(srcFileInfo.FullName)) throw new ApplicationException(srcFileInfo.FullName + "が見つかりません");

			var oldFileDirectory = new DirectoryInfo(srcFileInfo.DirectoryName);

			// 移動先のディレクトリがサブディレクトリを含む場合、
			// 存在しないサブディレクトリを作成します。
			var newFileInfo = new FileInfo(Path.Combine(workspace.PhysicalPath, target.MappingFilePath));
			Directory.CreateDirectory(newFileInfo.Directory.FullName);

			srcFileInfo.MoveTo(newFileInfo.FullName);

			// 移動元の物理ディレクトリ空間のフォルダが空の場合、フォルダ自体を削除する
			// ※移動元のファイルがあったディレクトリと、その上位階層にむけて空である場合、フォルダを削除します。
			//   このディレクトリにサブディレクトリがある場合は削除しません。(サブディレクトリ内が空であっても、削除は行わない)
			DirectoryInfo deleteTragetDirectory = oldFileDirectory;
			while (deleteTragetDirectory != null &&
				deleteTragetDirectory.GetFileSystemInfos().Count() == 0)
			{
				deleteTragetDirectory.Delete();
				deleteTragetDirectory = deleteTragetDirectory.Parent;
			}
		}

		#endregion メソッド
	}
}
