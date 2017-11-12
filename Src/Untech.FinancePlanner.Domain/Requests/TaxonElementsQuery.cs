using System.Collections.Generic;
using Untech.FinancePlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.FinancePlanner.Domain.Requests
{
	public class TaxonElementsQuery : IQuery<IEnumerable<Taxon>>
	{
		public TaxonElementsQuery(int taxonKey)
		{
			TaxonKey = taxonKey;
		}

		public int TaxonKey { get; }
	}
}