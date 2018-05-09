using System;
using System.Collections.Generic;
using System.Linq;
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

		public static MonthlyFinancialReportDay Create(DateTime day,
			IEnumerable<MonthlyFinancialReportDayEntry> entries)
		{
			var report = new MonthlyFinancialReportDay(day.Day)
			{
				Entries = entries
					.ToList()
			};

			report.ActualTotals = report.Entries.Sum(n => n.Actual);
			report.ForecastedTotals = report.Entries.Sum(n => n.Forecasted);

			return report;
		}
	}
}