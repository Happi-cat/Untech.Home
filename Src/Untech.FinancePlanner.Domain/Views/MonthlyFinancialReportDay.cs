using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices;

namespace Untech.FinancePlanner.Domain.Views
{
	[DataContract]
	public class MonthlyFinancialReportDay
	{
		public MonthlyFinancialReportDay(int day)
		{
			Day = day;
		}

		[DataMember]
		public int Day { get; private set; }

		[DataMember]
		public Money ActualTotals { get; set; }

		[DataMember]
		public Money ForecastedTotals { get; set; }

		[DataMember]
		public IReadOnlyCollection<MonthlyFinancialReportDayEntry> Entries { get; set; }
	}
}