using Untech.FinancePlanner.Domain.ViewModels;
using Untech.Practices.CQRS;

namespace Untech.FinancePlanner.Domain.Requests
{
	public class TaxonTreeQuery : IQuery<TaxonTree>
	{
		public int TaxonKey { get; set; }

		public int Deep { get; set; }
	}
}