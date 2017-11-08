using System;
using System.Linq;
using LinqToDB;
using Newtonsoft.Json;
using Untech.Practices.DataStorage.Cache;

namespace Untech.FinancePlanner.Data.Cache
{
	public class CacheStorage : ICacheStorage
	{
		private readonly Func<IDataContext> _connectionFactory;

		public CacheStorage(Func<IDataContext> connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public void Drop(CacheKey key, bool prefix = false)
		{
			var internalKey = key.ToString();

			using (var context = _connectionFactory())
			{
				var set = context.GetTable<CacheEntry>();
				if (prefix)
				{
					internalKey = internalKey.TrimEnd('/') + '/';
					set.Where(n => n.Key == internalKey || n.Key.StartsWith(internalKey))
						.Delete();
				}
				else
				{
					set.Where(n => n.Key == internalKey).Delete();
				}
			}
		}

		public T Get<T>(CacheKey key)
		{
			var internalKey = key.ToString();

			using (var context = _connectionFactory())
			{
				var entry = context
					.GetTable<CacheEntry>()
					.SingleOrDefault(n => n.Key == internalKey);

				return entry == null
					? default(T)
					: JsonConvert.DeserializeObject<T>(entry.Json);
			}
		}

		public void Set(CacheKey key, object value)
		{
			var internalKey = key.ToString();

			using (var context = _connectionFactory())
			{
				context.GetTable<CacheEntry>()
					.Where(n => n.Key == internalKey)
					.Delete();

				if (value == null)
				{
					return;
				}

				var entity = new CacheEntry
				{
					Key = internalKey,
					Json = JsonConvert.SerializeObject(value),
				};

				context.Insert(entity);
			}
		}
	}
}