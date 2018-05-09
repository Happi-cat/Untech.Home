using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Home;

namespace Untech.FinancePlanner.Domain.Views
{
	[DataContract]
	public class MonthlyFinancialReport : IHasMonthInfo
	{
		public MonthlyFinancialReport(int year, int month)
		{
			Year = year;
			Month = month;
		}

		[DataMember]
		public int Year { get; private set; }

		[DataMember]
		public int Month { get; private set; }

		[DataMember]
		public IReadOnlyCollection<MonthlyFinancialReportDay> Days { get; set; }
	}
}