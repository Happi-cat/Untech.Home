using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Untech.Practices.DataStorage.Cache;

namespace Untech.FinancePlanner.Data.Cache
{
	public class CacheStorage : ICacheStorage
	{
		private readonly Func<DbContext> _contextFactory;

		public CacheStorage(Func<DbContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public void Drop(CacheKey key, bool prefix = false)
		{
			var internalKey = key.ToString();

			using (var context = _contextFactory())
			{
				var set = context.Set<CacheEntry>();
				if (prefix)
				{
					internalKey = internalKey.TrimEnd('/') + '/';
					var entries = set.Where(n => n.Key.StartsWith(internalKey)).ToList();
					entries.ForEach(e => set.Remove(e));
				}
				else
				{
					var entry = set.SingleOrDefault(n => n.Key == internalKey);

					if (entry == null) return;

					set.Remove(entry);
				}
				context.SaveChanges();
			}
		}

		public T Get<T>(CacheKey key)
		{
			var internalKey = key.ToString();

			using (var context = _contextFactory())
			{
				var entry = context.Set<CacheEntry>().SingleOrDefault(n => n.Key == internalKey);

				if (entry == null) return default(T);

				return JsonConvert.DeserializeObject<T>(entry.Json);
			}
		}

		public void Set(CacheKey key, object value)
		{
			var internalKey = key.ToString();

			using (var context = _contextFactory())
			{
				var entity = new CacheEntry
				{
					Key = internalKey,
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