using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices;

namespace Untech.FinancePlanner.Domain.ViewModels
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
	}
}