using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Untech.FinancePlanner.Domain.Views
{
	[DataContract]
	public class AnnualFinancialReport
	{
		[DataMember]
		public IReadOnlyCollection<TaxonTree> Entries { get; set; }

		[DataMember]
		public IReadOnlyCollection<AnnualFinancialReportMonth> Months { get; set; }
	}
}