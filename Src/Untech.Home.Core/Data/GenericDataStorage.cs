using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using Untech.Practices.DataStorage;
using Untech.Practices.ObjectMapping;

namespace Untech.Home.Data
{
	public class GenericDataStorage<T> : IDataStorage<T>, IAsyncDataStorage<T>
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

		public virtual async Task<T> FindAsync(int key, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (var context = _contextFactory())
			{
				return await context
						.GetTable<T>()
						.SingleOrDefaultAsync(n => n.Key == key, cancellationToken)
					?? throw new AggregateRootNotFoundException(key);
			}
		}

		public virtual T Create(T entity)
		{
			using (var context = _contextFactory())
			{
				var key = context.InsertWithInt32Identity(entity);

				return context
					.GetTable<T>()
					.Single(n => n.Key == key);
			}
		}

		public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (var context = _contextFactory())
			{
				var key = await context.InsertWithInt32IdentityAsync(entity, cancellationToken);

				return await context
					.GetTable<T>()
					.SingleAsync(n => n.Key == key, cancellationToken);
			}
		}

		public virtual bool Delete(T entity)
		{
			using (var context = _contextFactory())
			{
				return context.Delete(entity) > 0;
			}
		}

		public virtual async Task<bool> DeleteAsync(T entity,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			using (var context = _contextFactory())
			{
				return await context.DeleteAsync(entity, cancellationToken) > 0;
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

		public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
		{
			using (var context = _contextFactory())
			{
				await context.UpdateAsync(entity, cancellationToken);
				return entity;
			}
		}

		protected IDataContext GetContext() => _contextFactory();

		protected ITable<T> GetTable(IDataContext context) => context.GetTable<T>();
	}

	public class GenericDataStorage<T, TDao> : IDataStorage<T>, IAsyncDataStorage<T>
		where T : IAggregateRoot
		where TDao : class, IAggregateRoot
	{
		private readonly GenericDataStorage<TDao> _innerDataStorage;
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

		public virtual async Task<T> FindAsync(int key, CancellationToken cancellationToken = default(CancellationToken))
		{
			var dao = await _innerDataStorage.FindAsync(key, cancellationToken);
			return ToEntity(dao);
		}

		public virtual T Create(T entity)
		{
			return ToEntity(_innerDataStorage.Create(ToDao(entity)));
		}

		public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
		{
			var dao = await _innerDataStorage.CreateAsync(ToDao(entity), cancellationToken);
			return ToEntity(dao);
		}

		public virtual bool Delete(T entity)
		{
			return _innerDataStorage.Delete(ToDao(entity));
		}

		public virtual Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
		{
			return _innerDataStorage.DeleteAsync(ToDao(entity), cancellationToken);
		}

		public virtual T Update(T entity)
		{
			return ToEntity(_innerDataStorage.Update(ToDao(entity)));
		}

		public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default(CancellationToken))
		{
			var dao = await _innerDataStorage.UpdateAsync(ToDao(entity));
			return ToEntity(dao);
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