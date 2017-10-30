using System;
using System.Runtime.Serialization;
using Untech.Practices;
using Untech.Practices.DataStorage;

namespace Untech.FinancePlanner.Domain.Models
{
	[DataContract]
	public class FinancialJournalEntry : IAggregateRoot
	{
		protected FinancialJournalEntry() { }

		public FinancialJournalEntry(int key, int taxonKey)
		{
			Key = key;
			TaxonKey = taxonKey;
		}

		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public int TaxonKey { get; private set; }

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