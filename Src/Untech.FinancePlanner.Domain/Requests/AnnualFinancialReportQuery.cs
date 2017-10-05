using System;
using Untech.FinancePlanner.Domain.ViewModels;
using Untech.Practices.CQRS;

namespace Untech.FinancePlanner.Domain.Requests
{
	public class AnnualFinancialReportQuery : IQuery<AnnualFinancialReport>
	{
		public int ShiftMonth { get; set; }
	}
}