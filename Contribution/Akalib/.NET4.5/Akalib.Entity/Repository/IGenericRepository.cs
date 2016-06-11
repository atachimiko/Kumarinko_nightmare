using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Akalib.Entity.Repository
{
	/// <summary>
	/// エンティティを取得するリソース
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IGenericRepository<T> where T : class
	{
		DbContext Entities { get; }

		IEnumerable<T> GetAll();

		IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);

		T Add(T entity);

		T Delete(T entity);

		void Edit(T entity);

		void Save();
	}
}