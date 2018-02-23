using System.Runtime.Serialization;
using Untech.Practices;
using Untech.Practices.CQRS;

namespace Untech.FinancePlanner.Domain.Requests.FinancialJournalEntry
{
	[DataContract]
	public class UpdateFinancialJournalEntry : ICommand<Models.FinancialJournalEntry>
	{
		public UpdateFinancialJournalEntry(int key)
		{
			Key = key;
		}

		[DataMember]
		public int Key { get; private set; }

		[DataMember]
		public string Remarks { get; set; }

		[DataMember]
		public Money Actual { get; set; }

		[DataMember]
		public Money Forecasted { get; set; }
	}
}
