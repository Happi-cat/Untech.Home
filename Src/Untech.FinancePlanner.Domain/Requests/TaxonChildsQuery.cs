using System.Collections.Generic;
using Untech.FinancePlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.FinancePlanner.Domain.Requests
{
	public class TaxonChildsQuery : IQuery<IEnumerable<Taxon>>
	{
		public TaxonChildsQuery(int taxonKey)
		{
			TaxonKey = taxonKey;
		}

		public int TaxonKey { get; }
	}
}