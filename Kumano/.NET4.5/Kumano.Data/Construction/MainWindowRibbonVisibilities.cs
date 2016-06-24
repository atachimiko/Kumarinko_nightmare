using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kumano.Data.Construction
{
	public class MainWindowRibbonVisibilities : Livet.NotificationObject
	{

		#region フィールド

		private bool _ListExplorerDocumentContextualMenuVisibilityFlag;

		private bool _PreviewImageDocumentContextualMenuVisibilityFlag;

		#endregion フィールド


		#region コンストラクタ

		public MainWindowRibbonVisibilities()
		{
			this._ListExplorerDocumentContextualMenuVisibilityFlag = false;
			this._PreviewImageDocumentContextualMenuVisibilityFlag = false;
		}

		#endregion コンストラクタ

		#region プロパティ

		/// <summary>
		/// 「画像一覧」拡張リボンメニューの表示・非表示フラグを取得、または設定します。
		/// </summary>
		public bool ListExplorerDocumentContextualMenuVisibilityFlag
		{
			get
			{ return _ListExplorerDocumentContextualMenuVisibilityFlag; }
			set
			{
				if (_ListExplorerDocumentContextualMenuVisibilityFlag == value)
					return;
				_ListExplorerDocumentContextualMenuVisibilityFlag = value;
				RaisePropertyChanged();
			}
		}

		/// <summary>
		/// 「画像プレビュー」拡張リボンメニューの表示・非表示フラグを取得、または設定します。
		/// </summary>
		public bool PreviewImageDocumentContextualMenuVisibilityFlag
		{
			get
			{ return _PreviewImageDocumentContextualMenuVisibilityFlag; }
			set
			{
				if (_PreviewImageDocumentContextualMenuVisibilityFlag == value)
					return;
				_PreviewImageDocumentContextualMenuVisibilityFlag = value;
				RaisePropertyChanged();
			}
		}

		#endregion プロパティ

	}
}
