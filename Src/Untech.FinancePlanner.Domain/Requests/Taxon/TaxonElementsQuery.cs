using System.Collections.Generic;
using Untech.Practices.CQRS;

namespace Untech.FinancePlanner.Domain.Requests.Taxon
{
	public class TaxonElementsQuery : IQuery<IEnumerable<Models.Taxon>>
	{
		public TaxonElementsQuery(int taxonKey)
		{
			TaxonKey = taxonKey;
		}

		public int TaxonKey { get; }
	}
}