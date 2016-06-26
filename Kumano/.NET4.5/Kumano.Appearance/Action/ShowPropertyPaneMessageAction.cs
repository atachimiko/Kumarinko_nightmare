using Akalib.Wpf.Dock;
using Kumano.Core;
using Kumano.Data.Construction;
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
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// プロパティペインに表示するViewModelのインスタンスは使い回しを行います。
	/// </remarks>
	public class ShowPropertyPaneMessageAction : InteractionMessageAction<FrameworkElement>
	{

		#region フィールド

		static ImagePreviewPropertyViewModel _ImagePreviewPropertyViewModel = null;
		static ArtifactNavigationListPropertyViewModel _ImageListPropertyViewModel = null;
		static ILog LOG = LogManager.GetLogger(typeof(ShowPropertyPaneMessageAction));

		#endregion フィールド


		#region メソッド

		protected override void InvokeAction(Livet.Messaging.InteractionMessage m)
		{
			////このアクションが対応するメッセージに変換します。
			var confirmMessage = m as ShowPropertyPaneMessage;
			if (confirmMessage == null) return;

			var workspace = ApplicationContext.Workspace;

			IPropertyPaneItem item;
			switch (confirmMessage.ShowPropertyPaneItemType)
			{
				case ShowPropertyPaneItemType.ImageDetailProperty:
					if (_ImagePreviewPropertyViewModel == null)
						_ImagePreviewPropertyViewModel = new ImagePreviewPropertyViewModel();
					item = _ImagePreviewPropertyViewModel;
					break;
				case ShowPropertyPaneItemType.ImageListProperty:
					if (_ImageListPropertyViewModel == null)
						_ImageListPropertyViewModel = new ArtifactNavigationListPropertyViewModel();
					item = _ImageListPropertyViewModel;
					break;
				case ShowPropertyPaneItemType.Clear:
					item = null;
					break;
				default:
					LOG.WarnFormat("未定義のプロパティペインを表示しようとしました。(Type={0})", confirmMessage.ShowPropertyPaneItemType);
					item = null;
					break;
			}

			if (item != null)
			{
				workspace.SetPropertyPaneItem(item);
				workspace.ShowPropertyPane();
				confirmMessage.Response = item;
			}
			else
			{
				workspace.SetPropertyPaneItem(item);
				confirmMessage.Response = null;
			}
		}

		#endregion メソッド
	}
}
