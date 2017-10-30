using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices;

namespace Untech.FinancePlanner.Domain.ViewModels
{
	[DataContract]
	public class MonthlyFinancialReportEntry
	{
		public MonthlyFinancialReportEntry(int taxonKey)
		{
			TaxonKey = taxonKey;
		}

		[DataMember]
		public int TaxonKey { get; private set; }

		[DataMember]
		public Money Actual { get; set; }

		[DataMember]
		public Money ActualTotals { get; set; }

		[DataMember]
		public Money Forecasted { get; set; }

		[DataMember]
		public Money ForecastedTotals { get; set; }

		[DataMember]
		public IReadOnlyCollection<MonthlyFinancialReportEntry> Entries { get; set; }
	}
}