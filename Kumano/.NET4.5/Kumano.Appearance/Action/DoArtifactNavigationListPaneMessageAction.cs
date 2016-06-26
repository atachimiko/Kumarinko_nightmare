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
	public class DoArtifactNavigationListPaneMessageAction : InteractionMessageAction<FrameworkElement>
	{
		static ILog LOG = LogManager.GetLogger(typeof(DoArtifactNavigationListPaneMessageAction));

		protected override void InvokeAction(Livet.Messaging.InteractionMessage message)
		{
			LOG.Debug("Execute DoImageListPaneMessageAction");
			var confirmMessage = message as DoArtifactNavigationListPaneMessage;
			if (confirmMessage == null) return;

			var workspace = ApplicationContext.Workspace as WorkspaceViewModel;
			workspace.ShowImageListDocument();

			var vm = workspace.FindDocumentPane(typeof(ArtifactNavigationListDocumentViewModel)).FirstOrDefault() as ArtifactNavigationListDocumentViewModel;

			vm.LoadServiceImageList();
		}
	}
}
