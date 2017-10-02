using System;
using System.Runtime.Serialization;
using Untech.FinancePlanner.Domain.Models;
using Untech.Practices;
using Untech.Practices.CQRS;

namespace Untech.FinancePlanner.Domain.Requests
{
	[DataContract]
	public class CreateFinancialJournalEntry : ICommand<FinancialJournalEntry>
	{
		public CreateFinancialJournalEntry(int taxonId, DateTime when)
		{
			TaxonId = taxonId;
			When = when;
		}

		[DataMember]
		public int TaxonId { get; private set; }

		[DataMember]
		public string Remarks { get; set; }

		[DataMember]
		public Money Actual { get; set; }

		[DataMember]
		public Money Forecasted { get; set; }

		[DataMember]
		public DateTime When { get; private set; }
	}
}
