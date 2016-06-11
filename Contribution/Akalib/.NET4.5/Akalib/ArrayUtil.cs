using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalib
{
	public static class ArrayUtil
	{
		/// <summary>
		/// 配列から指定した位置の要素を削除した新たな要素を作成します
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="originalArray"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static T[] RemoveAt<T>(T[] originalArray, int index)
		{
			T[] newArr = new T[originalArray.Length - 1];
			for (int i = 0; i < originalArray.Length; i++)
			{
				if (i < index) newArr[i] = originalArray[i];
				if (i > index) newArr[i - 1] = originalArray[i];
			}
			return newArr;
		}

		/// <summary>
		/// コレクションから任意の項目の次の項目を取得する
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="current"></param>
		/// <returns></returns>
		public static T Next<T>(ICollection<T> source, T current) where T : class
		{

			int pos = Array.FindIndex<T>(source.ToArray(), (prop) =>
			{
				if (prop == current) return true;
				return false;
			});

			// currentがコレクションに存在しない
			if (pos <= -1)
				return default(T);

			++pos;

			// 要素の最後の場合は、currentをそのまま返します。
			if (source.Count == pos)
				return current;

			return source.ElementAt(pos);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="current"></param>
		/// <returns></returns>
		public static T SwapNext<T>(IList<T> source, T current) where T : class
		{
			int pos = Array.FindIndex<T>(source.ToArray(), (prop) =>
			{
				if (prop == current) return true;
				return false;
			});

			// currentがコレクションに存在しない
			if (pos <= -1)
				return default(T);
			
			++pos;

			// 要素の最後の場合は、currentをそのまま返します。
			if (source.Count == pos)
				return current;

			
			source.Remove(current);
			source.Insert(pos, current);
			return source.ElementAt(pos);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="current"></param>
		/// <returns></returns>
		public static T SwapPrev<T>(IList<T> source, T current) where T : class
		{
			int pos = Array.FindIndex<T>(source.ToArray(), (prop) =>
			{
				if (prop == current) return true;
				return false;
			});

			// currentがコレクションに存在しない
			if (pos <= -1)
				return default(T);

			--pos;

			if (pos <= -1)
				return current;

			source.Remove(current);
			source.Insert(pos, current);
			return source.ElementAt(pos);
		}

		public class Jointer<T>
		{
			List<T> _elements;

			public Jointer(IEnumerable<T> elements)
			{
				_elements = new List<T>();
				Add(elements);
			}

			public void Add(IEnumerable<T> elements)
			{
				foreach (T value in elements)
					_elements.Add(value);
			}

			public IEnumerator<T> GetEnumerator()
			{
				foreach (T value in _elements)
					yield return value;
			}

			public static IEnumerable<T> Join<T>(params IEnumerable<T>[] args)
			{
				return args != null ? args.SelectMany(arg => arg) : Enumerable.Empty<T>();
			}
		}

	}
}
