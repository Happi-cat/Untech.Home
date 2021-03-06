using System.Runtime.Serialization;
using Untech.FinancePlanner.Domain.Models;
using Untech.Practices;
using Untech.Practices.CQRS;

namespace Untech.FinancePlanner.Domain.Requests
{
	[DataContract]
	public class CreateFinancialJournalEntry : ICommand<FinancialJournalEntry>
	{
		public CreateFinancialJournalEntry(int taxonKey, int year, int month)
		{
			TaxonKey = taxonKey;
			Year = year;
			Month = month;
		}

		[DataMember]
		public int TaxonKey { get; private set; }

		[DataMember]
		public string Remarks { get; set; }

		[DataMember]
		public Money Actual { get; set; }

		[DataMember]
		public Money Forecasted { get; set; }

		[DataMember]
		public int Year { get; private set; }

		[DataMember]
		public int Month { get; private set; }
	}
}
