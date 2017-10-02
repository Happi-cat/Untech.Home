using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Untech.FinancePlanner.Domain.Cache;

namespace Untech.FinancePlanner.Data.Cache
{
	public class CacheManager : ICacheManager
	{
		private readonly DbSet<CacheEntry> _dbSet;

		public CacheManager(DbSet<CacheEntry> dbSet)
		{
			_dbSet = dbSet;
		}

		public void Drop(string key)
		{
			var entry = _dbSet.Remove(new CacheEntry { Key = key });
			entry.Context.SaveChanges();
		}

		public T Get<T>(string key)
		{
			var entry = _dbSet.SingleOrDefault(n => n.Key == key);

			if (entry == null) return default(T);

			return JsonConvert.DeserializeObject<T>(entry.Json);
		}

		public void Set(string key, object value)
		{
			Drop(key);

			if (value == null) return;

			var entry = _dbSet.Add(new CacheEntry
			{
				Key = key,
				Json = JsonConvert.SerializeObject(value),
			});

			entry.Context.SaveChanges();
		}
	}
}