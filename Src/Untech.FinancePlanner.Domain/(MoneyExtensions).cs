using System;
using System.Collections.Generic;
using System.Linq;
using Untech.Practices;

namespace Untech.Practices
{
	public static class MoneyExtensions
	{
		public static Money Sum(this IEnumerable<Money> source)
		{
			var values = source.Where(n => n != null).ToList();

			if (values.Count == 0) return null;

			var currencies = values.Select(n => n.Currency).Distinct().ToList();

			if (currencies.Count > 1) throw new InvalidOperationException("Cannot summarize different currencies without conversion");

			return new Money(values.Sum(n => n.Amount), currencies.Single());
		}

		public static Money Sum<T>(this IEnumerable<T> source, Func<T, Money> selector)
		{
			return source.Select(selector).Sum();
		}

		public static Money Subtract(this Money left, Money right)
		{
			if (left == null) return right;
			if (right == null) return left;

			if (!left.Currency.Equals(right.Currency)) throw new InvalidOperationException("Cannot summarize different currencies without conversion");

			return new Money(left.Amount - right.Amount, left.Currency);
		}
	}
}