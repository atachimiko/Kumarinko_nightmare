using Akalib.Wpf.Dock;
using Kumano.Core;
using Kumano.Data.Infrastructure;
using Kumano.Data.ViewModel;
using Livet.Behaviors.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kumano.View.Action
{
	/// <summary>
	/// 
	/// </summary>
	public class ShowDocumentPaneMessageAction : InteractionMessageAction<FrameworkElement>
	{
		protected override void InvokeAction(Livet.Messaging.InteractionMessage m)
		{
			////このアクションが対応するメッセージに変換します。
			var confirmMessage = m as ShowDocumentPaneMessage;
			if (confirmMessage == null) return;

			var workspace = ApplicationContext.Workspace;
			IDocumentPaneViewModel vm;

			if (confirmMessage.ActiveDocument != null)
			{
				confirmMessage.Response = workspace.ShowDocument(confirmMessage.ActiveDocument);
			}
			else
			{
				switch (confirmMessage.ShowDocumentPaneType)
				{
					case ShowDocumentPaneType.DocumentPaneArtifactList:
						vm = new ArtifactNavigationListDocumentViewModel();
						break;
					case ShowDocumentPaneType.DocumentPanePreviewImage:
						vm = new ImagePreviewDocumentViewModel();
						break;
					default:
						throw new NotSupportedException();
				}

				confirmMessage.Response = workspace.ShowDocument(vm);
			}

		}
	}
}
