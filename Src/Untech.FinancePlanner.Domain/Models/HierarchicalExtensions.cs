using System.Collections.Generic;
using System.Linq;
using Untech.Practices.Linq;

namespace Untech.FinancePlanner.Domain.Models
{
	public static class HierarchicalExtensions
	{
		public static IEnumerable<T> DescendantsAndSelf<T>(this T node)
			where T : IHierarchical<T>
		{
			yield return node;

			foreach (var descendant in node.Elements.EmptyIfNull().SelectMany(DescendantsAndSelf))
			{
				yield return descendant;
			}
		}
	}
}