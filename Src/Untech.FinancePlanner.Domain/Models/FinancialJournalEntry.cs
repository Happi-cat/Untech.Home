using System;
using System.Runtime.Serialization;
using Untech.FinancePlanner.Domain.Storage;
using Untech.Practices;

namespace Untech.FinancePlanner.Domain.Models
{
	[DataContract]
	public class FinancialJournalEntry : IAggregateRoot
	{
		protected FinancialJournalEntry() { }

		public FinancialJournalEntry(int id, int taxonId)
		{
			Id = id;
			TaxonId = taxonId;
		}

		[DataMember]
		public int Id { get; private set; }

		[DataMember]
		public int TaxonId { get; private set; }

		[DataMember]
		public string Remarks { get; set; }

		[DataMember]
		public Money Actual { get; set; }

		[DataMember]
		public Money Forecasted { get; set; }

		[DataMember]
		public DateTime When { get; set; }
	}
}