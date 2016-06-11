using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mogami.Contrib.Akalib
{
	/// <summary>
	/// スタック構造を持つハッシュマップ
	/// </summary>
	public class ParameterStack
	{

		#region フィールド

		/// <summary>
		/// データを格納するハッシュのスタック
		/// </summary>
		readonly private Stack<Dictionary<string, object>> _KeyValueStack = new Stack<Dictionary<string, object>>();

		#endregion フィールド


		#region コンストラクタ

		public ParameterStack()
		{
			this._KeyValueStack.Push(new Dictionary<string, object>());
		}

		#endregion コンストラクタ

		#region メソッド

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">ハッシュキー</param>
		/// <param name="hierarchy">全階層を遡って値を探すフラグ</param>
		/// <returns></returns>
		public bool ContainsKey(string key, bool hierarchy = false)
		{
			bool retobj = false;
			if (!hierarchy)
				return this._KeyValueStack.Peek().ContainsKey(key);
			else
			{
				foreach (var prop in this._KeyValueStack)
				{
					if (this._KeyValueStack.Peek().ContainsKey(key))
					{
						retobj = true;
						break;
					}
				}
			}

			return retobj;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TTYPE"></typeparam>
		/// <param name="key">ハッシュキー</param>
		/// <param name="hierarchy">全階層を遡って値を探すフラグ</param>
		/// <returns></returns>
		public TTYPE GetValue<TTYPE>(string key, bool hierarchy = false)
		{
			return (TTYPE)GetValue(key, hierarchy);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key">ハッシュキー</param>
		/// <param name="hierarchy">全階層を遡って値を探すフラグ</param>
		/// <returns></returns>
		public object GetValue(string key, bool hierarchy = false)
		{
			object retobj = null;
			if (!hierarchy)
				this._KeyValueStack.Peek().TryGetValue(key, out retobj);
			else
			{
				foreach (var prop in this._KeyValueStack)
				{
					if (this._KeyValueStack.Peek().TryGetValue(key, out retobj))
						break;
				}
			}

			return retobj;
		}

		/// <summary>
		/// スタックの先頭から、キーマップを削除します
		/// </summary>
		public void Pop()
		{
			this._KeyValueStack.Pop();
		}

		/// <summary>
		/// スタックを追加します
		/// </summary>
		public void Push()
		{
			this._KeyValueStack.Push(new Dictionary<string, object>());
		}

		/// <summary>
		/// 現在のスタックで、キーマップに値を追加、設定します
		/// </summary>
		/// <param name="key">ハッシュキー</param>
		/// <param name="value">値</param>
		public void SetValue(string key, object value)
		{
			this._KeyValueStack.Peek()[key] = value;
		}

		#endregion メソッド
	}
}
