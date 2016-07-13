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
			var vm = workspace.FindDocumentPane(typeof(NavigationDocumentViewModel)).FirstOrDefault() as NavigationDocumentViewModel;
			var dataTemplate = vm.ActiveContent as ArtifactNavigationListDocumentViewModel;
			if (dataTemplate == null)
			{
				dataTemplate = new ArtifactNavigationListDocumentViewModel();
				vm.ActiveContent = dataTemplate;
			}

			if (confirmMessage.FindByCategoryId != 0L)
				dataTemplate.LoadCategoryId = confirmMessage.FindByCategoryId;
			else if (confirmMessage.FindByTagId != 0L)
				dataTemplate.LoadTagId = confirmMessage.FindByTagId;
		}
	}
}
