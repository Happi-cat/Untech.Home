using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Untech.FinancePlanner.Domain.Storage;

namespace Untech.FinancePlanner.Data.Cache
{
	public class CacheStorage : ICacheStorage
	{
		private readonly Func<DbContext> _contextFactory;

		public CacheStorage(Func<DbContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public void Drop(string key)
		{
			using (var context = _contextFactory())
			{
				var entry = context.Set<CacheEntry>().SingleOrDefault(n => n.Key == key);

				if (entry == null) return;

				context.Set<CacheEntry>().Remove(entry);
				context.SaveChanges();
			}
		}

		public T Get<T>(string key)
		{
			using (var context = _contextFactory())
			{
				var entry = context.Set<CacheEntry>().SingleOrDefault(n => n.Key == key);

				if (entry == null) return default(T);

				return JsonConvert.DeserializeObject<T>(entry.Json);
			}
		}

		public void Set(string key, object value)
		{
			using (var context = _contextFactory())
			{
				var entity = new CacheEntry
				{
					Key = key,
					Json = JsonConvert.SerializeObject(value),
				};

				context.Set<CacheEntry>().Remove(entity);

				if (value != null)
				{
					context.Set<CacheEntry>().Add(entity);
				}

				context.SaveChanges();
			}
		}
	}
}