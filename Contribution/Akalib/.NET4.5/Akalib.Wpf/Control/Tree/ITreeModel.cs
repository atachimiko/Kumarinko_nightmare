using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalib.Wpf.Control.Tree
{
	/// <summary>
	/// TreeListに設定するデータモデルに使用するインターフェース
	/// </summary>
	public interface ITreeModel
	{
		#region メソッド

		/// <summary>
		/// Get list of children of the specified parent
		/// </summary>
		IEnumerable GetChildren(object parent);

		/// <summary>
		/// returns wheather specified parent has any children or not.
		/// </summary>
		bool HasChildren(object parent);

		#endregion メソッド
	}
}
