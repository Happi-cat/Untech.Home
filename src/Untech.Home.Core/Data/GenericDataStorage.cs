using System;
using System.Linq;
using LinqToDB;
using Untech.Practices.DataStorage;
using Untech.Practices.ObjectMapping;

namespace Untech.Home.Data
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
						.SingleOrDefault(n => n.Key == key)
					?? throw new AggregateRootNotFoundException(key);
			}
		}

		public virtual T Create(T entity)
		{
			using (var context = _contextFactory())
			{
				var key = context.InsertWithInt32Identity(entity);
				return context.GetTable<T>().Single(n => n.Key == key);
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

		protected ITable<T> GetTable(IDataContext context) => context.GetTable<T>();
	}

	public class GenericDataStorage<T, TDao> : IDataStorage<T>
		where T : IAggregateRoot
		where TDao : class, IAggregateRoot
	{
		private readonly IDataStorage<TDao> _innerDataStorage;
		private readonly Func<IDataContext> _contextFactory;
		private readonly IMap<T, TDao> _mapToDao;
		private readonly IMap<TDao, T> _mapToEntity;

		public GenericDataStorage(Func<IDataContext> contextFactory, IMap<T, TDao> mapToDao, IMap<TDao, T> mapToEntity)
		{
			_innerDataStorage = new GenericDataStorage<TDao>(contextFactory);
			_contextFactory = contextFactory;
			_mapToDao = mapToDao;
			_mapToEntity = mapToEntity;
		}

		public virtual T Find(int key)
		{
			return ToEntity(_innerDataStorage.Find(key));
		}

		public virtual T Create(T entity)
		{
			return ToEntity(_innerDataStorage.Create(ToDao(entity)));
		}

		public virtual bool Delete(T entity)
		{
			return _innerDataStorage.Delete(ToDao(entity));
		}

		public virtual T Update(T entity)
		{
			return ToEntity(_innerDataStorage.Update(ToDao(entity)));
		}

		protected IDataContext GetContext() => _contextFactory();

		protected ITable<TDao> GetTable(IDataContext context) => context.GetTable<TDao>();

		protected T ToEntity(TDao dao)
		{
			return _mapToEntity.Map(dao);
		}

		protected TDao ToDao(T entity)
		{
			return _mapToDao.Map(entity);
		}
	}
}