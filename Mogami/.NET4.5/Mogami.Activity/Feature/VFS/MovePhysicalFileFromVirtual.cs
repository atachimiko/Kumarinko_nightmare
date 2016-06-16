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

namespace Mogami.Activity.Feature.VFS
{

	public sealed class MovePhysicalFileFromVirtual : CodeActivity
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

			var cleared = pstack.GetValue<Boolean>(ActivityParameterStack.MOVEPHYSICALFILEFROMVIRTUAL_CLEAREDFLAG);


			if (!cleared) MoveVirtualFile(context);
			else ClearedTemporaryFile(context);
		}

		/// <summary>
		/// 物理ディレクトリ空間のファイル名から、「tmp」を除去します
		/// </summary>
		/// <param name="context"></param>
		void ClearedTemporaryFile(CodeActivityContext context)
		{
			ParameterStack pstack = context.GetValue<ParameterStack>(this.Parameter);
			var workspace = pstack.GetValue<Workspace>(ActivityParameterStack.WORKSPACE);
			var target = pstack.GetValue<FileMappingInfo>(ActivityParameterStack.TARGET);

			// Guard
			Ensure.That(target).IsNotNull();
			Ensure.That(workspace).IsNotNull();


			var clearedFileInfo = new FileInfo(Path.Combine(workspace.PhysicalPath, target.MappingFilePath) + ".tmp");
			if (!File.Exists(clearedFileInfo.FullName)) throw new ApplicationException(clearedFileInfo.FullName + "が見つかりません");

			var extFileName = Path.GetFileNameWithoutExtension(clearedFileInfo.Name);

			clearedFileInfo.MoveTo(Path.Combine(clearedFileInfo.DirectoryName, extFileName));
		}

		/// <summary>
		/// ファイルの移動(転送)を行います
		/// </summary>
		/// <param name="context"></param>
		void MoveVirtualFile(CodeActivityContext context)
		{
			ParameterStack pstack = context.GetValue<ParameterStack>(this.Parameter);
			var workspace = pstack.GetValue<Workspace>(ActivityParameterStack.WORKSPACE);
			var target = pstack.GetValue<FileMappingInfo>(ActivityParameterStack.TARGET);

			// Guard
			Ensure.That(target).IsNotNull();
			Ensure.That(workspace).IsNotNull();

			var fromFilePath = new FileInfo(Path.Combine(workspace.WorkspacePath, target.MappingFilePath));
			var toFilePath = Path.Combine(workspace.PhysicalPath, target.MappingFilePath + ".tmp");

			if (!File.Exists(fromFilePath.FullName))
				new ApplicationException(fromFilePath.FullName + "が見つかりません");

			// 移動先のディレクトリがサブディレクトリを含む場合、
			// 存在しないサブディレクトリを作成します。
			var newFileInfo = new FileInfo(Path.Combine(workspace.PhysicalPath, target.MappingFilePath));
			Directory.CreateDirectory(newFileInfo.Directory.FullName);

			File.Move(fromFilePath.FullName, toFilePath);
		}

		#endregion メソッド
	}
}
