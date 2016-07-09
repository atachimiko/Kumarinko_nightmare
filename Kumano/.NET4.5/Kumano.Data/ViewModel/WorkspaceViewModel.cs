using Akalib.Wpf.Dock;
using Akalib.Wpf.Mvvm;
using Kumano.Contrib.ViewModel;
using Kumano.Core;
using Kumano.Core.Infrastructures;
using Kumano.Data.Construction;
using Kumano.Data.Infrastructure;
using Kumano.Data.Service;
using Livet;
using Livet.Messaging.IO;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kumano.Data.ViewModel
{
	public class WorkspaceViewModel : AvalonDockWorkspaceViewModel, IWorkspaceViewModel
	{


		#region フィールド

		static ILog LOG = LogManager.GetLogger(typeof(WorkspaceViewModel));
		private readonly MainWindowRibbonVisibilities _MainWindowRibbonVisibilities = new MainWindowRibbonVisibilities();

		CategoryTreeExplorerPaneViewModel _CategoryTreeExplorerPaneViewModel;

		PropertyPaneViewModel _PropertyPaneViewModel;

		TagTreeExplorerPaneViewModel _TagTreeExplorerPaneViewModel;

		#endregion フィールド

		#region プロパティ

		/// <summary>
		/// 
		/// </summary>
		public CategoryTreeExplorerPaneViewModel CategoryTreeExplorerPaneViewModel
		{
			get { return _CategoryTreeExplorerPaneViewModel; }
		}

		/// <summary>
		/// 拡張メニューの表示・非表示フラグを取得する
		/// </summary>
		public MainWindowRibbonVisibilities MainWindowRibbonVisibilities
		{
			get { return _MainWindowRibbonVisibilities; }
		}

		#endregion プロパティ


		#region メソッド

		public override void Close()
		{
			// 不要なため、処理は行わない
		}

		public IEnumerable<IDocumentPaneViewModel> FindDocumentPane(Type clazz)
		{
			var @r = from u in this.Contents
					 where clazz.IsAssignableFrom(u.GetType())
					 select u;
			return @r;
		}

		/// <summary>
		/// 画像ファイルをシステムに登録します
		/// </summary>
		/// <param name="message"></param>
		public async void ImportRegisterImageFile(OpeningFileSelectionMessage message)
		{
			if (message.Response != null && message.Response.Count() > 0)
			{
				foreach (var filepath in message.Response)
				{
					LOG.DebugFormat("選択したファイル情報 {0}", filepath);
					/*
					using (var proxy = new HalcyonService.AlcedinesApiServiceClient())
					{
						var artifacts = await proxy.AttachFileAsync(filepath, 1L);
						foreach (var artifact in artifacts)
						{
							LOG.InfoFormat("登録したArtifact: ID={0}", artifact.Artifact.Id);
						}
					}
					*/
				}
			}
		}

		public void Login()
		{
			LOG.Info("Login");
			/*
			using (var proxy = new HalcyonService.AlcedinesApiServiceClient())
			{
				proxy.Login("alcedines", "alt0123");

				proxy.Login("alcedines222", "alt0123");
			}
			*/
		}

		public void Logout()
		{
			LOG.Info("Logout");
			/*
			using (var proxy = new HalcyonService.AlcedinesApiServiceClient())
			{
				proxy.Logout();
				proxy.Logout();
			}
			*/
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		public void OnWindowSizeChanged(SizeChangedEventArgs e)
		{
			var s = e.OriginalSource as Window;
			if (s.WindowState == System.Windows.WindowState.Normal)
			{
				double lastHeight = e.NewSize.Height;
				double lastWidth = e.NewSize.Width;

				//LOG.InfoFormat("ウィンドウのサイズが変更しました ({0},{1})", lastWidth, lastHeight);

				ApplicationContext.ApplicationSetting.WindowSize = new Size(lastWidth, lastHeight);
			}
		}
		public void SetPropertyPaneItem(IPropertyPaneItem itemViewModel)
		{
			this._PropertyPaneViewModel.ActiveContent = itemViewModel;
		}

		/// <summary>
		/// 画像リスト画面を表示します
		/// </summary>
		public void ShowImageListDocument()
		{
			LOG.Debug("Execute ShowImageListDocument");

			this.ShowDocument(new ArtifactNavigationListDocumentViewModel());
		}

		/// <summary>
		/// 画像プレビュー画面を表示します
		/// </summary>
		public void ShowImagePreviewDocument()
		{
			LOG.Debug("Execute ShowImageDocument");

			this.ShowDocument(new ImagePreviewDocumentViewModel());
		}

		public void ShowPropertyPane()
		{
			if (!this.AnchorContents.Contains(this._PropertyPaneViewModel))
				this.AnchorContents.Add(this._PropertyPaneViewModel);

			this._PropertyPaneViewModel.IsVisible = true;
		}

		//ImagePreviewDocumentViewModel _pp = null;
		protected override void InitializePane()
		{
			if (this._PropertyPaneViewModel == null)
			{
				this._PropertyPaneViewModel = new PropertyPaneViewModel();
				this.AnchorContents.Add(this._PropertyPaneViewModel);
			}

			if (this._CategoryTreeExplorerPaneViewModel == null)
			{
				this._CategoryTreeExplorerPaneViewModel = new CategoryTreeExplorerPaneViewModel();
				this.AnchorContents.Add(this._CategoryTreeExplorerPaneViewModel);
			}
			
			if (this._TagTreeExplorerPaneViewModel == null)
			{
				this._TagTreeExplorerPaneViewModel = new TagTreeExplorerPaneViewModel();
				this.AnchorContents.Add(this._TagTreeExplorerPaneViewModel);
			}
			
		}

		private void ShowDialog(IDialogViewModel viewmodel)
		{
			var message = new DialogMessage(viewmodel);
			
			this.Messenger.RaiseAsync(message);
		}

		#endregion メソッド

	}
}
