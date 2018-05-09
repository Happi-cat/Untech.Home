using Untech.FinancePlanner.Domain.Views;
using Untech.Practices.CQRS;

namespace Untech.FinancePlanner.Domain.Requests
{
	public class AnnualFinancialReportQuery : IQuery<AnnualFinancialReport>
	{
		public int ShiftMonth { get; set; }
	}
}