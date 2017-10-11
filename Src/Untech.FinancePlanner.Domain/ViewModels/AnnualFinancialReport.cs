using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Untech.FinancePlanner.Domain.ViewModels
{
	[DataContract]
	public class AnnualFinancialReport
	{
		[DataMember]
		public IReadOnlyCollection<TaxonTree> Entries { get; set; }

		[DataMember]
		public IReadOnlyCollection<MonthlyFinancialReport> Months { get; set; }
	}
}