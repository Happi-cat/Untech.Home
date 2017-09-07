using Untech.FinancePlanner.Domain.ViewModels;
using Untech.Practices.CQRS;

namespace Untech.FinancePlanner.Domain.Requests
{
	public class AnnualFinancialReportQuery : IQuery<AnnualFinancialReport>
	{
		public TaxonTreeQuery Taxon { get; set; }
	}
}