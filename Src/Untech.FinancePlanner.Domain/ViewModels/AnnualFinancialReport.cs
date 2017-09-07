using System.Collections.Generic;

namespace Untech.FinancePlanner.Domain.ViewModels
{
	public class AnnualFinancialReport
	{
		public TaxonTree Entries { get; set; }
		public IReadOnlyCollection<MonthlyFinancialReport> Months { get; set; }
	}
}