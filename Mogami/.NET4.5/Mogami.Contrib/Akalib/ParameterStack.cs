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
		readonly private Stack<Dictionary<string, ValueObject>> _KeyValueStack = new Stack<Dictionary<string, ValueObject>>();

		#endregion フィールド


		#region コンストラクタ

		public ParameterStack()
		{
			this._KeyValueStack.Push(new Dictionary<string, ValueObject>());
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
			ValueObject retobj = null;
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

			if (retobj != null)
			{
				if (string.IsNullOrEmpty(retobj.AliasKey))
					return retobj.Value;

				return GetValue(retobj.AliasKey, hierarchy);
			}else
			{
				throw new ApplicationException(string.Format("キー{0}が、ValueStackに見つかりません。", key));
			}
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
			this._KeyValueStack.Push(new Dictionary<string, ValueObject>());
		}

		/// <summary>
		/// エイリアスキーを追加します。
		/// </summary>
		/// <param name="key"></param>
		/// <param name="aliasKey"></param>
		public void SetAlias(string key, string aliasKey)
		{
			this._KeyValueStack.Peek()[key].Value = null; // 同名のキーがある場合は、キーマップの値は削除。
			this._KeyValueStack.Peek()[key].AliasKey = aliasKey;
		}

		/// <summary>
		/// 現在のスタックで、キーマップに値を追加、設定します
		/// </summary>
		/// <param name="key">ハッシュキー</param>
		/// <param name="value">値</param>
		public void SetValue(string key, object value)
		{
			if (this._KeyValueStack.Peek().ContainsKey(key))
				this._KeyValueStack.Peek()[key].Value = value;
			else
				this._KeyValueStack.Peek().Add(key, new ValueObject { Value = value });
		}

		#endregion メソッド


		#region 内部クラス

		class ValueObject
		{

			#region フィールド

			internal string AliasKey;
			internal object Value;

			#endregion フィールド
		}

		#endregion 内部クラス
	}
}
