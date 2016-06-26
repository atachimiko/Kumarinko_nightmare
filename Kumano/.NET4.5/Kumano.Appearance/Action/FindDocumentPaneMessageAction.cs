using Kumano.Core;
using Kumano.Data.Infrastructure;
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
	public class FindDocumentPaneMessageAction : InteractionMessageAction<FrameworkElement>
	{


		#region メソッド

		protected override void InvokeAction(Livet.Messaging.InteractionMessage m)
		{
			////このアクションが対応するメッセージに変換します。
			var confirmMessage = m as FindDocumentPaneMessage;
			if (confirmMessage == null) return;

			var workspace = ApplicationContext.Workspace;
			confirmMessage.Response = workspace.FindDocumentPane(confirmMessage.DocumentPaneViewModelClazz).ToArray();
		}

		#endregion メソッド

	}
}
