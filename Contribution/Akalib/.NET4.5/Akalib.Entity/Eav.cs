using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;

namespace Akalib.Entity
{
	/// <summary>
	/// EAVデータを格納するコレクション
	/// </summary>
	public abstract class EavAttributeVarcharListBase<T, EAVT, EAVT2> : IEnumerable<T>
		where EAVT : IEavType<T>, new()
		where EAVT2 : Eav_Attribute
	{
		protected readonly string _AttributeCode;

		protected IList<EAVT2> _Attributes;

		public EavAttributeVarcharListBase(IList<EAVT2> Attributes, string AttributeCode)
		{
			_Attributes = Attributes;
			_AttributeCode = AttributeCode;
		}
		public int Count
		{
			get
			{
				var source = GetEnumerator();
				ICollection c = source as ICollection;
				if (c != null)
					return c.Count;

				int result = 0;

				while (source.MoveNext())
					result++;

				return result;
			}
		}

		public T this[int key]
		{
			get
			{
				var b = true;
				var r = this.GetEnumerator();
				r.Reset();
				while (key < -1 && b)
				{
					key--;
					b = r.MoveNext();
				}

				if (b)
					return r.Current;

				return default(T);
			}

			set
			{
				dynamic r = Get(key);
				if (r != null) r.Value = value;
			}
		}

		public void Add(T item)
		{
			var myItem = new EAVT();
			myItem.Value = item;

			var a = myItem as EAVT2;
			a.AttributeCode = _AttributeCode;


			this._Attributes.Add(a);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this._Attributes.Where(p => p.AttributeCode == _AttributeCode)
					.Cast<EAVT>()
					.Select((p, a) => p.Value).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		protected void Add(EAVT2 item)
		{
			this._Attributes.Add(item);
		}

		protected Eav_Attribute Get(int index)
		{
			var r = this._Attributes.Where(p => p.AttributeCode == _AttributeCode);
			return r.AsQueryable().ElementAt(index);
		}
	}

	public interface IEavType<T>
	{
		T Value { get; set; }
	}


	public abstract class Eav_Attribute : Entity<long>
	{

		#region AttributeCode変更通知プロパティ
		private string _AttributeCode;

		public string AttributeCode
		{
			get
			{ return _AttributeCode; }
			set
			{
				if (_AttributeCode == value)
					return;
				_AttributeCode = value;
				RaisePropertyChanged();
			}
		}
		#endregion

	}
}
