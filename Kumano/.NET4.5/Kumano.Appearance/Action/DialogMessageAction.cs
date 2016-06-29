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
	public class DialogMessageAction : InteractionMessageAction<FrameworkElement>
	{
		protected override void InvokeAction(Livet.Messaging.InteractionMessage m)
		{
			////このアクションが対応するメッセージに変換します。
			var confirmMessage = m as DialogMessage;
			if (confirmMessage == null)
			{
				return;
			}

			Window dialog = null;

			// ダイアログの表示
			if (dialog != null)
			{
				dialog.DataContext = confirmMessage.ViewModel;
				dialog.Owner = ApplicationContext.MainWindow;
				dialog.ShowDialog();
			}
		}
	}
}
