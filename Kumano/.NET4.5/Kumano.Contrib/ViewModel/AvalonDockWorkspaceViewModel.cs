using Akalib.Wpf.Dock;
using Akalib.Wpf.Mvvm;
using Livet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.AvalonDock.Layout.Serialization;

namespace Kumano.Contrib.ViewModel
{
	/// <summary>
	/// AvalonDockのレイアウトを保存・リストアするための処理
	/// </summary>
	public abstract class AvalonDockWorkspaceViewModel : WorkspaceViewModelBase
	{


		#region フィールド

		Byte[] m_defaultLayout;

		#endregion フィールド

		#region プロパティ

		/// <summary>
		/// レイアウトファイルのファイルパスを取得します
		/// </summary>
		String LayoutFile
		{
			get
			{
				return System.IO.Path.ChangeExtension(
					Environment.GetCommandLineArgs()[0]
					, ".AvalonDock.config"
					);
			}
		}

		#endregion プロパティ

		#region メソッド

		/// <summary>
		/// デフォルトレイアウトを設定します(アプリケーション起動直後のドッキング状態を設定)
		/// </summary>
		/// <param name="dockManager"></param>
		public void DefaultLayout(DockingManager dockManager)
		{
			LoadLayoutFromBytes(dockManager, m_defaultLayout);
		}

		/// <summary>
		/// レイアウトを読み込みます
		/// </summary>
		/// <param name="dockManager"></param>
		public void LoadLayout(DockingManager dockManager)
		{
			// backup default layout
			using (var ms = new MemoryStream())
			{
				var serializer = new XmlLayoutSerializer(dockManager);
				serializer.Serialize(ms);
				m_defaultLayout = ms.ToArray();
			}

			// load file
			Byte[] bytes;
			try
			{
				bytes = System.IO.File.ReadAllBytes(LayoutFile);
			}
			catch (FileNotFoundException ex)
			{
				return;
			}

			// restore layout
			LoadLayoutFromBytes(dockManager, bytes);
		}

		/// <summary>
		/// レイアウトをシリアライズしてファイルへ出力します
		/// </summary>
		/// <param name="dockManager"></param>
		public void SaveLayout(DockingManager dockManager)
		{
			var serializer = new XmlLayoutSerializer(dockManager);
			var doc = new XmlDocument();
			using (var stream = new MemoryStream())
			{
				serializer.Serialize(stream);
				stream.Position = 0;
				doc.Load(stream);
			}

			ModifySerializedXml(doc);

			using (var stream = new FileStream(LayoutFile, FileMode.Create))
			{
				doc.Save(stream);
			}
		}

		protected abstract void InitializePane();

		/// <summary>
		/// シリアライズされたXMLファイルに直接変更を行う場合、このメソッドをオーバーライドしてください。
		/// </summary>
		/// <param name="doc"></param>
		protected virtual void ModifySerializedXml(System.Xml.XmlDocument doc)
		{

		}

		bool LoadLayoutFromBytes(DockingManager dockManager, Byte[] bytes)
		{
			InitializePane();

			var serializer = new XmlLayoutSerializer(dockManager);

			serializer.LayoutSerializationCallback += MatchLayoutContent;

			try
			{
				using (var stream = new MemoryStream(bytes))
				{
					serializer.Deserialize(stream);
				}

				// ドキュメントはレストアしない
				//RestoreDocumentsFromBytes(bytes);

				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		void MatchLayoutContent(object o, LayoutSerializationCallbackEventArgs e)
		{
			var contentId = e.Model.ContentId;

			if (e.Model is LayoutAnchorable)
			{
				foreach (var tool in this.AnchorContents)
				{
					if (tool.ContentId == contentId)
					{
						e.Content = tool;
						return;
					}
				}

				// Unknown
				//ErrorDialog(new Exception("unknown ContentID: " + contentId));
				e.Cancel = true; // 未実装のペインは表示しない
				return;
			}

			if (e.Model is LayoutDocument)
			{
				// Documentは復帰しない
				// load済みを探す
				e.Cancel = true; // ドキュメントは読み込まない
				return;

				foreach (var document in this.Contents)
				{
					if (document.ContentId == contentId)
					{
						e.Content = document;
						return;
					}
				}

				// Document
				{
					//var document = NewDocument();
					//Documents.Add(document);
					//document.ContentId = contentId;
					//e.Content = document;
				}

				return;
			}

			//ErrorDialog(new Exception("Unknown Model: " + e.Model.GetType()));
			return;
		}

		#endregion メソッド

	}
}
