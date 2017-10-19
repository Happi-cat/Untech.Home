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
		public CreateFinancialJournalEntry(int taxonId, int year, int month)
		{
			TaxonId = taxonId;
			Year = year;
			Month = month;
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
		public int Year { get; private set; }

		[DataMember]
		public int Month { get; private set; }
	}
}
