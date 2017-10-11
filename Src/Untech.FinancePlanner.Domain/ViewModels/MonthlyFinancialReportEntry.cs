using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices;

namespace Untech.FinancePlanner.Domain.ViewModels
{
	[DataContract]
	public class MonthlyFinancialReportEntry
	{
		public MonthlyFinancialReportEntry(int taxonId)
		{
			TaxonId = taxonId;
		}

		[DataMember]
		public int TaxonId { get; private set; }

		[DataMember]
		public Money Actual { get; set; }

		[DataMember]
		public Money Forecasted { get; set; }

		[DataMember]
		public IReadOnlyCollection<MonthlyFinancialReportEntry> Entries { get; set; }
	}
}