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
			workspace.ShowImagePreviewDocument();

			var vm = workspace.FindDocumentPane(typeof(ImagePreviewDocumentViewModel)).FirstOrDefault() as ImagePreviewDocumentViewModel;
			if (vm == null)
			{
				LOG.Error("ImagePreviewを表示できませんでした");
				return;
			}

			vm.PreviewImageInfo = null;

			if (confirmMessage.IsWithActive)
			{
				workspace.ShowDocument(vm);
			}

			LOG.InfoFormat("Message Parameter [LoadImageInfo.BitmapFile]{0}", confirmMessage.LoadImageInfo.BitmapFilePath);

			// NOTE: 画像の表示
			vm.PreviewImageInfo = (confirmMessage.LoadImageInfo);
		}

		#endregion メソッド

	}
}
