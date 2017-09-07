using System.Collections.Generic;
using System.Runtime.Serialization;
using Untech.Practices;

namespace Untech.FinancePlanner.Domain.ViewModels
{
	[DataContract]
	public class MonthlyFinancialReportEntry
	{
		public MonthlyFinancialReportEntry(int taxonId, string name, string description = null)
		{
			TaxonId = taxonId;
			Name = name;
			Description = description;
		}

		[DataMember]
		public int TaxonId { get; private set; }

		[DataMember]
		public string Name { get; private set; }

		[DataMember]
		public string Description { get; private set; }

		[DataMember]
		public Money Actual { get; set; }

		[DataMember]
		public Money Forecasted { get; set; }

		[DataMember]
		public IReadOnlyCollection<MonthlyFinancialReportEntry> Entries { get; set; }
	}
}