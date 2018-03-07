using System.Runtime.Serialization;
using Untech.Practices;

namespace Untech.FinancePlanner.Domain.Views
{
	[DataContract]
	public class MonthlyFinancialReportDayEntry
	{
		public MonthlyFinancialReportDayEntry(string name, int taxonKey)
		{
			Name = name;
			TaxonKey = taxonKey;
		}

		[DataMember]
		public int TaxonKey { get; private set; }

		[DataMember]
		public string Name { get; private set; }

		[DataMember]
		public string Remarks { get; set; }

		[DataMember]
		public Money Actual { get; set; }

		[DataMember]
		public Money Forecasted { get; set; }
	}
}