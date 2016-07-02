using Kumano.Core;
using Kumano.Data.Infrastructure;
using Kumano.Data.ViewModel;
using Livet.Behaviors.Messaging;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kumano.View.Action
{
	public class DoImagePreviewPaneMessageAction : InteractionMessageAction<FrameworkElement>
	{

		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(DoImagePreviewPaneMessageAction));

		#endregion フィールド


		#region メソッド

		protected override void InvokeAction(Livet.Messaging.InteractionMessage message)
		{
			LOG.Debug("Execute DoImagePreviewPaneMessageAction");
			var confirmMessage = message as DoImagePreviewPaneMessage;
			if (confirmMessage == null) return;

			var workspace = ApplicationContext.Workspace as WorkspaceViewModel;
			

			var vm = workspace.FindDocumentPane(typeof(ImagePreviewDocumentViewModel)).FirstOrDefault() as ImagePreviewDocumentViewModel;
			if (vm == null)
			{
				workspace.ShowImagePreviewDocument();
				vm = workspace.FindDocumentPane(typeof(ImagePreviewDocumentViewModel)).FirstOrDefault() as ImagePreviewDocumentViewModel;
			}

			vm.PreviewImageInfo = null;

			LOG.InfoFormat("Message Parameter [LoadImageInfo.BitmapFile]{0}", confirmMessage.LoadImageInfo.BitmapFilePath);
			
			vm.PreviewImageInfo = (confirmMessage.LoadImageInfo);

			if (confirmMessage.IsWithActive)
			{
				workspace.ShowDocument(vm);
				workspace.ActivePane = vm;
			}
		}

		#endregion メソッド

	}
}
