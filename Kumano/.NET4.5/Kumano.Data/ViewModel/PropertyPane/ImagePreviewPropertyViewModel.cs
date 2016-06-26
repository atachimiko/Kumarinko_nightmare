using Akalib.Wpf.Dock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kumano.Data.ViewModel
{
	/// <summary>
	/// 
	/// </summary>
	public class ImagePreviewPropertyViewModel : Livet.ViewModel, IPropertyPaneItem
	{
		//TODO: 実装してください


		#region SampleText変更通知プロパティ
		private string _SampleText;
		/// <summary>
		/// サンプルテキスト。実装の動作確認で使用する。
		/// コミット前に削除します。
		/// </summary>
		public string SampleText
		{
			// TODO: コミット前に削除します
			get
			{ return _SampleText; }
			set
			{
				if (_SampleText == value)
					return;
				_SampleText = value;
				RaisePropertyChanged();
			}
		}
		#endregion

	}
}
