using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Untech.FinancePlanner.Domain.Storage
{
	public interface IDataStorage<T>
		where T : IAggregateRoot
	{
		T Find(int id);

		IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

		T Create(T entity);

		T Update(T entity);

		bool Delete(T entity);
	}
}