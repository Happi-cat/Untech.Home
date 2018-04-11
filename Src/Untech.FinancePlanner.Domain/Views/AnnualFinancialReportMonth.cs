using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Untech.FinancePlanner.Domain.Models;
using Untech.Home;
using Untech.Practices;

namespace Untech.FinancePlanner.Domain.Views
{
	[DataContract]
	public class AnnualFinancialReportMonth
	{
		public AnnualFinancialReportMonth(DateTime thatMonth)
		{
			Year = thatMonth.Year;
			Month = thatMonth.Month;
		}

		[DataMember]
		public int Year { get; private set; }

		[DataMember]
		public int Month { get; private set; }

		[DataMember]
		public Money ActualTotals { get; set; }

		[DataMember]
		public Money ForecastedTotals { get; set; }

		[DataMember]
		public IReadOnlyCollection<AnnualFinancialReportMonthEntry> Entries { get; set; }

		[DataMember]
		public bool IsPast
		{
			get
			{
				var today = DateTime.Today;
				return Year < today.Year || (today.Year == Year && Month < today.Month);
			}
		}

		[DataMember]
		public bool IsNow
		{
			get
			{
				var today = DateTime.Today;
				return today.Year == Year && today.Month == Month;
			}
		}

		public static AnnualFinancialReportMonth Create(DateTime month, IEnumerable<AnnualFinancialReportMonthEntry> entries)
		{
			var report = new AnnualFinancialReportMonth(month)
			{
				Entries = entries
					.Where(e => e.IsActualOrForecastedPresent())
					.ToList()
			};

			var incomes = report.Entries
				.Where(n => n.TaxonKey == BuiltInTaxonId.Income)
				.ToList();
			var expenses = report.Entries
				.Where(n => n.TaxonKey == BuiltInTaxonId.Expense)
				.ToList();

			report.ActualTotals = incomes
				.Sum(n => n.ActualTotals)
				.Subtract(expenses
					.Sum(n => n.ActualTotals));

			report.ForecastedTotals = incomes
				.Sum(n => n.ForecastedTotals)
				.Subtract(expenses
					.Sum(n => n.ForecastedTotals));

			return report;
		}
	}
}