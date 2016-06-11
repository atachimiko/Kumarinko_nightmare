using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Akalib.Wpf
{
	public static class DependencyObjectExtensions
	{
		/// <summary>
		/// 指定されたDependencyObjectからビジュアルツリーをさかのぼり、
		/// Genericで指定された型のオブジェクトを検索します。
		/// 
		/// 【CAUTION!!】DataGridColumnを探す事はできません!)<br/>
		/// 検索できなかった時はnullを返します。<br/>
		/// </summary>
		/// <remarks>
		/// DataGridColumnは、VisualTree、LogicalTree両方に属さない特殊なクラスなので、
		/// このメソッドを使ってDataGridやDaraGridRowオブジェクトを取得しようとしても失敗します。<br/>
		/// </remarks>
		/// <typeparam name="T">検索対象となるクラスの型宣言</typeparam>
		/// <param name="depObj">DependencyObjectインスタンス</param>
		/// <returns>検索対象オブジェクト</returns>
		public static T FindAncestor<T>(this DependencyObject depObj) where T : class
		{
			var target = depObj;

			try
			{
				do
				{
					//ビジュアルツリー上の親を探します。
					//T型のクラスにヒットするまでさかのぼり続けます。
					target = System.Windows.Media.VisualTreeHelper.GetParent(target);

				} while (target != null && !(target is T));

				return target as T;
			}
			finally
			{
				target = null;
				depObj = null;
			}
		}

		/// <summary>
		/// 指定されたDependencyObjectからビジュアルツリーを下り、
		/// Genericで指定された型のオブジェクトを検索します。<br/>
		/// 検索できなかった時はnullを返します。<br/>
		/// </summary>
		/// <typeparam name="T">検索対象となるクラスの型宣言</typeparam>
		/// <param name="depObj">依存関係プロパティオブジェクト</param>
		/// <param name="descendant">true:子だけでなく、孫、玄孫も探しに行く</param>
		/// <returns>検索対象オブジェクトが列挙されたシーケンス</returns>
		public static IEnumerable<T> FindChildren<T>(
						this DependencyObject depObj, bool descendant = true) where T : class
		{
			var count = System.Windows.Media.VisualTreeHelper.GetChildrenCount(depObj);

			try
			{
				foreach (var idx in RangeEnumerator(0, count - 1))
				{
					var child = System.Windows.Media.VisualTreeHelper.GetChild(depObj, idx);

					if (child != null)
					{
						if (child is T)
						{
							yield return child as T;
						}

						if (descendant)
						{
							count = System.Windows.Media.VisualTreeHelper.GetChildrenCount(child);
							if (count > 0)
							{
								foreach (var ch in child.FindChildren<T>(descendant))
								{
									yield return ch;
								}
							}
						}
					}
				}
			}
			finally
			{
				depObj = null;
			}
		}

		/// <summary>
		/// 指定された開始インデックス(start)から終了インデックス(end)までの
		/// 数値が列挙されたシーケンス(+方向へ)を取得します<br/>
		/// </summary>
		/// <param name="start">開始インデックス</param>
		/// <param name="end">終了インデックス</param>
		/// <returns>数値シーケンス(+方向)</returns>
		public static IEnumerable<int> RangeEnumerator(int start, int end)
		{
			for (int i = start; i <= end; i++)
			{
				yield return i;
			}
		}
	}
}
