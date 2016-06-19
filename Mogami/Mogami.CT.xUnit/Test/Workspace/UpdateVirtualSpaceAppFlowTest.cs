using log4net;
using Mogami.Applus;
using Mogami.Contrib.Akalib;
using Mogami.Core.Definication;
using Mogami.CT.xUnit.Action;
using Mogami.Gateway;
using Mogami.Model.Repository;
using Mogami.Workflow;
using NUnit.Framework;
using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mogami.CT.xUnit.Test.Workspace
{
	/// <summary>
	/// UpdateVirtualSpaceAppFlow試験用のサポートクラス
	/// </summary>
	public static class UpdateVirtualSpaceAppFlowTestSupport
	{

		#region メソッド

		/// <summary>
		/// テスト用ファイルを、物理ディレクトリ空間に配置します
		/// </summary>
		/// <param name="loadWorkspaceId">ワークスペース</param>
		/// <returns>物理ディレクトリ空間でのファイル情報</returns>
		public static FileInfo CopyPhysicalSpace_Kinkaku(long loadWorkspaceId)
		{
			FileInfo physicalSpaceFileInfo;
			Assembly myAssembly = Assembly.GetExecutingAssembly();
			string path = Path.GetDirectoryName(myAssembly.Location);
			var srcFile = new FileInfo(path + "/Assets/Image/Kinkaku.jpg");

			using (var dbc = new AppDbContext())
			{
				var repo = new WorkspaceRepository(dbc);
				var workspace = repo.Load(loadWorkspaceId);

				physicalSpaceFileInfo = new FileInfo(Path.Combine(workspace.PhysicalPath, srcFile.Name));
				srcFile.CopyTo(physicalSpaceFileInfo.FullName, true);
			}

			return physicalSpaceFileInfo;
		}

		/// <summary>
		/// 「Kinkaku.jpg.aclgene」ファイルを、仮想ディレクトリ空間に作成します
		/// </summary>
		/// <returns></returns>
		public static FileInfo CopyVirtualSpace_Kinkaku_aclgene(long loadWorkspaceId)
		{
			FileInfo virtualSpaceFileInfo;
			Assembly myAssembly = Assembly.GetExecutingAssembly();
			string path = Path.GetDirectoryName(myAssembly.Location);
			var srcFile = new FileInfo(path + "/Assets/Acl/Kinkaku.jpg.aclgene");

			using (var dbc = new AppDbContext())
			{
				var repo = new WorkspaceRepository(dbc);
				var workspace = repo.Load(loadWorkspaceId);

				virtualSpaceFileInfo = new FileInfo(Path.Combine(workspace.WorkspacePath, srcFile.Name));
				srcFile.CopyTo(virtualSpaceFileInfo.FullName, true);
			}

			return virtualSpaceFileInfo;
		}

		/// <summary>
		/// 「Kinkaku.jpg.aclgene」ファイルを、仮想ディレクトリ空間に「Subdir」フォルダを作成し、フォルダ内に配置します
		/// </summary>
		/// <param name="loadWorkspaceId"></param>
		/// <returns></returns>
		public static FileInfo CopyVirtualSpaceSubdirectory_Kinkaku_aclgene(long loadWorkspaceId)
		{
			const string subdirName = "Subdir";

			FileInfo virtualSpaceFileInfo;
			Assembly myAssembly = Assembly.GetExecutingAssembly();
			string path = Path.GetDirectoryName(myAssembly.Location);
			var srcFile = new FileInfo(path + "/Assets/Acl/Kinkaku.jpg.aclgene");

			using (var dbc = new AppDbContext())
			{
				var repo = new WorkspaceRepository(dbc);
				var workspace = repo.Load(loadWorkspaceId);

				// Subdirフォルダを作成
				Directory.CreateDirectory(Path.Combine(workspace.WorkspacePath, subdirName));

				virtualSpaceFileInfo = new FileInfo(Path.Combine(
					Path.Combine(workspace.WorkspacePath, subdirName)
					, srcFile.Name));
				srcFile.CopyTo(virtualSpaceFileInfo.FullName, true);
			}

			return virtualSpaceFileInfo;

		}

		/// <summary>
		/// 物理ディレクトリ空間のすべてのファイルを削除します
		/// </summary>
		/// <param name="loadWorkspaceId">ワークスペース</param>
		public static void RemovePhysicalFile(long loadWorkspaceId)
		{
			using (var dbc = new AppDbContext())
			{
				var repo = new WorkspaceRepository(dbc);
				var workspace = repo.Load(loadWorkspaceId);

				var dirInfo = new DirectoryInfo(workspace.PhysicalPath);
				while (dirInfo.Exists)
				{
					Directory.Delete(workspace.PhysicalPath, true);
					dirInfo = new DirectoryInfo(workspace.PhysicalPath);
					Thread.Sleep(100);
				}

				while (!dirInfo.Exists)
				{
					Directory.CreateDirectory(workspace.PhysicalPath);
					dirInfo = new DirectoryInfo(workspace.PhysicalPath);
					Thread.Sleep(100);
				}
			}
		}

		/// <summary>
		/// 仮想ディレクトリ空間のすべてのファイルを削除します
		/// </summary>
		/// <param name="loadWorkspaceId">ワークスペース</param>
		public static void RemoveVirtualFile(long loadWorkspaceId)
		{
			using (var dbc = new AppDbContext())
			{
				var repo = new WorkspaceRepository(dbc);
				var workspace = repo.Load(loadWorkspaceId);

				var dirInfo = new DirectoryInfo(workspace.WorkspacePath);
				while (dirInfo.Exists)
				{
					Directory.Delete(workspace.WorkspacePath, true);
					dirInfo = new DirectoryInfo(workspace.WorkspacePath);
					Thread.Sleep(100);
				}

				while (!dirInfo.Exists)
				{
					Directory.CreateDirectory(workspace.WorkspacePath);
					dirInfo = new DirectoryInfo(workspace.WorkspacePath);
					Thread.Sleep(100);
				}
			}
		}

		#endregion メソッド

	}

	/// <summary>
	/// 仮想ディレクトリ空間の仮想ファイルを、サブディレクトリに移動した場合の動作テスト
	/// </summary>
	[TestFixture]
	[UsingDatabaseAction(ImportInitializeDataFlag = true, ImportSqlFileName = "App.UpdateVirtualSpaceAppFlowTest.txt")]
	public class UpdateVirtualSpaceAppFlowTest_MoveFileName
	{


		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(UpdateVirtualSpaceAppFlowTest_MoveFileName));

		#endregion フィールド

		#region メソッド

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// テスト内容：リネーム後のACLファイルを使用したシステム操作
		/// </remarks>
		[Test]
		public void Run_UpdateVirtualSpaceAppFlowTest_MoveFileName()
		{
			LOG.Debug("execute method run");

			UpdateVirtualSpaceAppFlowTestSupport.RemoveVirtualFile(1L);
			UpdateVirtualSpaceAppFlowTestSupport.RemovePhysicalFile(1L);
			UpdateVirtualSpaceAppFlowTestSupport.CopyPhysicalSpace_Kinkaku(1L);

			var aclfile = UpdateVirtualSpaceAppFlowTestSupport.CopyVirtualSpaceSubdirectory_Kinkaku_aclgene(1L);

			// ...
			using (var dbc = new AppDbContext())
			{
				var workspace = WorkspaceRepository.Load(dbc, 1L);
				var workflow = new WorkflowInvoker(new UpdateVirtualSpaceAppFlow());
				workflow.Extensions.Add(new WorkflowExtention(dbc));

				var pstack = new ParameterStack();
				pstack.SetValue("Event", Mogami.Core.Constructions.UpdateVirtualStatusEventType.UPDATE);
				pstack.SetValue(ActivityParameterStack.WORKSPACE_FILEPATH, @"Subdir\Kinkaku.jpg.aclgene"); // 移動後のファイルパス
				pstack.SetValue(ActivityParameterStack.WORKSPACE, workspace);

				var results = workflow.Invoke(new Dictionary<string, object>
				{
					{"ParameterStack", pstack}
				});

				dbc.SaveChanges();
			}



			using (var dbc = new AppDbContext())
			{
				var filemapinginfoRepository = new FileMappingInfoRepository(dbc);
				var entity = filemapinginfoRepository.Load(1L);

				var workspace = WorkspaceRepository.Load(dbc, 1L);

				// エンティティの値が変わっていることの確認
				Assert.AreEqual(@"Subdir\Kinkaku.jpg", entity.MappingFilePath);

				// 物理ディレクトリ空間での、ファイル名称が変更していることの確認
				var physicalFileInfo = new FileInfo(Path.Combine(workspace.PhysicalPath, @"Subdir\Kinkaku.jpg"));
				Assert.IsTrue(physicalFileInfo.Exists);
			}
		}

		#endregion メソッド

	}

	/// <summary>
	/// 仮想ディレクトリ空間の仮想ファイルを削除した場合の動作テスト
	/// </summary>
	[TestFixture]
	[UsingDatabaseAction(ImportInitializeDataFlag = true, ImportSqlFileName = "App.UpdateVirtualSpaceAppFlowTest.txt")]
	public class UpdateVirtualSpaceAppFlowTest_DeleteFileName
	{
		static ILog LOG = LogManager.GetLogger(typeof(UpdateVirtualSpaceAppFlowTest_DeleteFileName));

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// 
		/// </remarks>
		[Test]
		public void Run_UpdateVirtualSpaceAppFlowTest_DeleteFileName()
		{
			UpdateVirtualSpaceAppFlowTestSupport.RemoveVirtualFile(1L);
			UpdateVirtualSpaceAppFlowTestSupport.RemovePhysicalFile(1L);
			UpdateVirtualSpaceAppFlowTestSupport.CopyPhysicalSpace_Kinkaku(1L);

			var aclfile = UpdateVirtualSpaceAppFlowTestSupport.CopyVirtualSpace_Kinkaku_aclgene(1L);
			aclfile.Delete();


			using (var dbc = new AppDbContext())
			{
				var workspace = WorkspaceRepository.Load(dbc, 1L);
				var workflow = new WorkflowInvoker(new UpdateVirtualSpaceAppFlow());
				workflow.Extensions.Add(new WorkflowExtention(dbc));

				var pstack = new ParameterStack();
				pstack.SetValue("Event", Mogami.Core.Constructions.UpdateVirtualStatusEventType.DELETE);
				pstack.SetValue(ActivityParameterStack.WORKSPACE_FILEPATH, "Kinkaku.jpg.aclgene"); // 削除したファイル
				pstack.SetValue(ActivityParameterStack.WORKSPACE, workspace);
				pstack.SetValue("WF_DeleteAclHash", "Test");

				var results = workflow.Invoke(new Dictionary<string, object>
				{
					{"ParameterStack", pstack}
				});

				dbc.SaveChanges();
			}

			using (var dbc = new AppDbContext())
			{
				var workspace = WorkspaceRepository.Load(dbc, 1L);

				// 物理ファイルが削除されていることを確認する
				var physicalFileInfo = new FileInfo(Path.Combine(workspace.PhysicalPath, "Kinkaku.jpg"));
				Assert.IsFalse(physicalFileInfo.Exists);

				var entity = FileMappingInfoRepository.Load(dbc, 1L);
				Assert.IsNull(entity);
			}
		}
	}

	/// <summary>
	/// 仮想ディレクトリ空間の仮想ファイルをリネームした場合の動作テスト
	/// </summary>
	[TestFixture]
	[UsingDatabaseAction(ImportInitializeDataFlag = true, ImportSqlFileName = "App.UpdateVirtualSpaceAppFlowTest.txt")]
	public class UpdateVirtualSpaceAppFlowTest_RenameFileName
	{


		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(UpdateVirtualSpaceAppFlowTest_RenameFileName));

		#endregion フィールド

		#region メソッド

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// テスト内容：リネーム後のACLファイルを使用したシステム操作
		/// </remarks>
		[Test]
		public void Run_UpdateVirtualSpaceAppFlowTest_RenameFileName()
		{
			LOG.Debug("execute method run");

			UpdateVirtualSpaceAppFlowTestSupport.RemoveVirtualFile(1L);
			UpdateVirtualSpaceAppFlowTestSupport.RemovePhysicalFile(1L);
			UpdateVirtualSpaceAppFlowTestSupport.CopyPhysicalSpace_Kinkaku(1L);

			var aclfile = UpdateVirtualSpaceAppFlowTestSupport.CopyVirtualSpace_Kinkaku_aclgene(1L);
			aclfile.MoveTo(Path.Combine(aclfile.DirectoryName, "Kinkaku2.jpg.aclgene")); //  新しいACLファイル名前にリネームする
			aclfile = new FileInfo(Path.Combine(aclfile.DirectoryName, "Kinkaku2.jpg.aclgene"));

			// ...
			using (var dbc = new AppDbContext())
			{
				var workspace = WorkspaceRepository.Load(dbc, 1L);
				var workflow = new WorkflowInvoker(new UpdateVirtualSpaceAppFlow());
				workflow.Extensions.Add(new WorkflowExtention(dbc));

				var pstack = new ParameterStack();
				pstack.SetValue("Event", Mogami.Core.Constructions.UpdateVirtualStatusEventType.RENAME);
				pstack.SetValue(ActivityParameterStack.WORKSPACE_FILEPATH, "Kinkaku2.jpg.aclgene"); // リネーム後のファイル名
				pstack.SetValue(ActivityParameterStack.WORKSPACE, workspace);

				var results = workflow.Invoke(new Dictionary<string, object>
				{
					{"ParameterStack", pstack}
				});

				dbc.SaveChanges();
			}

			using (var dbc = new AppDbContext())
			{
				var filemapinginfoRepository = new FileMappingInfoRepository(dbc);
				var entity = filemapinginfoRepository.Load(1L);

				var workspace = WorkspaceRepository.Load(dbc, 1L);

				// エンティティの値が変わっていることの確認
				Assert.AreEqual("Kinkaku2.jpg", entity.MappingFilePath);

				// 物理ディレクトリ空間での、ファイル名称が変更していることの確認
				var physicalFileInfo = new FileInfo(Path.Combine(workspace.PhysicalPath, "Kinkaku2.jpg"));
				Assert.IsTrue(physicalFileInfo.Exists);
			}

		}

		#endregion メソッド

	}
}
