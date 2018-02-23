using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.FinancePlanner.Domain.Requests
{
	[DataContract]
	public class DeleteFinancialJournalEntry : ICommand<bool>
	{
		public DeleteFinancialJournalEntry(int key)
		{
			Key = key;
		}

		[DataMember]
		public int Key { get; private set; }
	}
}
