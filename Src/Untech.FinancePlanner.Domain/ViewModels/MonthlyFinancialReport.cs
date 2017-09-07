using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices;

namespace Untech.FinancePlanner.Domain.ViewModels
{
	[DataContract]
	public class MonthlyFinancialReport
	{
		public MonthlyFinancialReport(DateTime thatMonth)
		{
			Year = thatMonth.Year;
			Month = thatMonth.Month;
		}

		[DataMember]
		public int Year { get; private set; }

		[DataMember]
		public int Month { get; private set; }

		[DataMember]
		public Money ActualBalance { get; set; }

		[DataMember]
		public Money ForecastedBalance { get; set; }

		[DataMember]
		public IReadOnlyCollection<MonthlyFinancialReportEntry> Entries { get; set; }

		[DataMember]
		public bool IsPast
		{
			get
			{
				var today = DateTime.Today;
				return today.Year < Year || (today.Year == Year && today.Month < Month);
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