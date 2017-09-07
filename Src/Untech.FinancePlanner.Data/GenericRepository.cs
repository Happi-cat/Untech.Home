using System.Linq;
using Microsoft.EntityFrameworkCore;
using Untech.Practices.Repos.Queryable;

namespace Untech.FinancePlanner.Data
{
	public class GenericRepository<T> : IRepository<T>
		where T : class
	{
		private readonly DbSet<T> _dbSet;

		protected GenericRepository(DbSet<T> dbSet)
		{
			_dbSet = dbSet;
		}

		public T Create(T entity)
		{
			var entry = _dbSet.Add(entity);
			entry.Context.SaveChanges();
			return entry.Entity;
		}

		public bool Delete(T entity)
		{
			var entry = _dbSet.Remove(entity);
			return entry.Context.SaveChanges() > 0;
		}

		public IQueryable<T> GetAll()
		{
			return _dbSet;
		}

		public bool Update(T entity)
		{
			var entry = _dbSet.Update(entity);
			return entry.Context.SaveChanges() > 0;
		}
	}
}