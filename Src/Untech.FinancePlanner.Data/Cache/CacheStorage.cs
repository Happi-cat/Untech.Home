using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using Newtonsoft.Json;
using Untech.Practices.DataStorage.Cache;

namespace Untech.FinancePlanner.Data.Cache
{
	public class CacheStorage : ICacheStorage, IAsyncCacheStorage
	{
		private readonly Func<IDataContext> _contextFactory;

		public CacheStorage(Func<FinancialPlannerContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public T Get<T>(CacheKey key)
		{
			var internalKey = key.ToString();

			using (var context = _contextFactory())
			{
				var entry = context
					.GetTable<CacheEntry>()
					.SingleOrDefault(n => n.Key == internalKey);

				return Unwrap<T>(entry);
			}
		}

		public async Task<T> GetAsync<T>(CacheKey key, CancellationToken cancellationToken = default(CancellationToken))
		{
			var internalKey = key.ToString();

			using (var context = _contextFactory())
			{
				var entry = await context
					.GetTable<CacheEntry>()
					.SingleOrDefaultAsync(n => n.Key == internalKey, cancellationToken);

				return Unwrap<T>(entry);
			}
		}

		public void Set(CacheKey key, object value)
		{
			var internalKey = key.ToString();

			using (var context = _contextFactory())
			{
				context.GetTable<CacheEntry>()
					.Where(n => n.Key == internalKey)
					.Delete();

				if (value == null)
				{
					return;
				}

				context.Insert(Wrap(value, internalKey));
			}
		}

		public async Task SetAsync(CacheKey key, object value,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			var internalKey = key.ToString();

			using (var context = _contextFactory())
			{
				await context.GetTable<CacheEntry>()
					.Where(n => n.Key == internalKey)
					.DeleteAsync(cancellationToken);

				if (value == null)
				{
					return;
				}

				await context.InsertAsync(Wrap(value, internalKey), token: cancellationToken);
			}
		}

		public void Drop(CacheKey key, bool prefix = false)
		{
			using (var context = _contextFactory())
			{
				GetEntriesForDrop(context.GetTable<CacheEntry>(), key, prefix)
					.Delete();
			}
		}

		public async Task DropAsync(CacheKey key, bool prefix = false,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			using (var context = _contextFactory())
			{
				GetEntriesForDrop(context.GetTable<CacheEntry>(), key, prefix)
					.DeleteAsync(cancellationToken);
			}
		}

		private IQueryable<CacheEntry> GetEntriesForDrop(ITable<CacheEntry> set, CacheKey key, bool prefix = false)
		{
			var internalKey = key.ToString();

			if (prefix)
			{
				internalKey = internalKey.TrimEnd('/');
				var internalPrefix = internalKey + '/';

				return set.Where(n => n.Key == internalKey || n.Key.StartsWith(internalPrefix));
			}

			return set.Where(n => n.Key == internalKey);
		}

		private static CacheEntry Wrap(object value, string internalKey)
		{
			var entity = new CacheEntry
			{
				Key = internalKey,
				Json = JsonConvert.SerializeObject(value),
			};
			return entity;
		}

		private static T Unwrap<T>(CacheEntry entry)
		{
			return entry == null
				? default(T)
				: JsonConvert.DeserializeObject<T>(entry.Json);
		}
	}
}