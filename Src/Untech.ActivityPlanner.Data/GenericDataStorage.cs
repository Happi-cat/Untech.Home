using System;
using System.Linq;
using LinqToDB;
using Untech.Practices.DataStorage;

namespace Untech.ActivityPlanner.Data
{
	public class GenericDataStorage<T> : IDataStorage<T>
		where T : class, IAggregateRoot
	{
		private readonly Func<IDataContext> _contextFactory;

		public GenericDataStorage(Func<IDataContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public virtual T Find(int key)
		{
			using (var context = _contextFactory())
			{
				return context
					.GetTable<T>()
					.SingleOrDefault(n => n.Key == key);
			}
		}

		public virtual T Create(T entity)
		{
			using (var context = _contextFactory())
			{
				var key = context.InsertWithInt32Identity(entity);
				return Find(key);
			}
		}

		public virtual bool Delete(T entity)
		{
			using (var context = _contextFactory())
			{
				return context.Delete(entity) > 0;
			}
		}

		public virtual T Update(T entity)
		{
			using (var context = _contextFactory())
			{
				context.Update(entity);
				return entity;
			}
		}

		protected IDataContext GetContext() => _contextFactory();
	}
}