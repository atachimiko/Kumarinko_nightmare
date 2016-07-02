using Kumano.Core;
using Kumano.Data.Infrastructure;
using Kumano.Data.ViewModel;
using Kumano.View.Dialog;
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
	public class DialogMessageAction : InteractionMessageAction<FrameworkElement>
	{

		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(DialogMessageAction));

		#endregion フィールド


		#region メソッド

		protected override void InvokeAction(Livet.Messaging.InteractionMessage m)
		{
			////このアクションが対応するメッセージに変換します。
			var confirmMessage = m as DialogMessage;
			if (confirmMessage == null)
			{
				return;
			}

			Window dialog = null;
			if(confirmMessage.ViewModel is EditCategoryDialogViewModel)
			{
				dialog = new EditCategoryDialog();
			}
			else if(confirmMessage.ViewModel is VersionDialogViewModel)
			{
				dialog = new VersionDialog();
			}
			else
			{
				LOG.Warn("サポートしていない形式のダイアログボックスを表示しようとしました。");
			}

			// ダイアログの表示
			if (dialog != null)
			{
				dialog.DataContext = confirmMessage.ViewModel;
				dialog.Owner = ApplicationContext.MainWindow;
				dialog.ShowDialog();
			}
		}

		#endregion メソッド
	}
}
