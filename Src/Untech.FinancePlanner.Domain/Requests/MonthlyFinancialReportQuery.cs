using System.Runtime.Serialization;
using Untech.FinancePlanner.Domain.ViewModels;
using Untech.Practices.CQRS;

namespace Untech.FinancePlanner.Domain.Requests
{
	[DataContract]
	public class MonthlyFinancialReportQuery : IQuery<MonthlyFinancialReport>
	{
		public MonthlyFinancialReportQuery(int year, int month)
		{
			Year = year;
			Month = month;
		}

		[DataMember]
		public int Year { get; private set; }

		[DataMember]
		public int Month { get; private set; }
	}
}
