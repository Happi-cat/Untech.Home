using System;
using System.Collections.Generic;
using System.Linq;

namespace Untech.ActivityPlanner.Domain.Services
{
	internal static class CustomEnumerableExtensions
	{
		public static Dictionary<TKey, TSource[]> ToKeyValues<TSource, TKey>(
			this IEnumerable<TSource> source,
			Func<TSource, TKey> keySelector) => source
			.GroupBy(keySelector)
			.ToDictionary(n => n.Key, n => n.ToArray());

		public static IEnumerable<TValue> GetValues<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue[]> dictionary,
			TKey key) => dictionary.ContainsKey(key)
			? dictionary[key]
			: Enumerable.Empty<TValue>();
	}
}