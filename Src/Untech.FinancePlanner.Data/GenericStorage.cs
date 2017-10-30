using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Untech.Practices.DataStorage;

namespace Untech.FinancePlanner.Data
{
	public class GenericStorage<T> : IDataStorage<T>
		where T : class, IAggregateRoot
	{
		private readonly Func<DbContext> _contextFactory;

		public GenericStorage(Func<DbContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public T Find(int key)
		{
			using (var context = _contextFactory())
			{
				return context.Set<T>().SingleOrDefault(n => n.Key == key);
			}
		}

		public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
		{
			using (var context = _contextFactory())
			{
				return context.Set<T>().Where(predicate).ToList();
			}
		}

		public T Create(T entity)
		{

			using (var context = _contextFactory())
			{
				var entry = context.Set<T>().Add(entity);
				context.SaveChanges();
				return entry.Entity;
			}
		}

		public bool Delete(T entity)
		{
			using (var context = _contextFactory())
			{
				var entry = context.Set<T>().Remove(entity);
				return context.SaveChanges() > 0;
			}
		}

		public T Update(T entity)
		{
			using (var context = _contextFactory())
			{
				context.Set<T>().Update(entity);
				context.SaveChanges();
				return entity;
			}
		}
	}
}