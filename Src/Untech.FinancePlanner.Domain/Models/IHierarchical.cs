using System.Collections.Generic;

namespace Untech.FinancePlanner.Domain.Models
{
	public interface IHierarchical<T>
		where T : IHierarchical<T>
	{
		IReadOnlyCollection<T> Elements { get; }
	}
}