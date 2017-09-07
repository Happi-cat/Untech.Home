using System.Runtime.Serialization;
using Untech.Practices.CQRS;

namespace Untech.FinancePlanner.Domain.Requests
{
	[DataContract]
	public class DeleteFinancialJournalEntry : ICommand<bool>
	{
		public DeleteFinancialJournalEntry(int id)
		{
			Id = id;
		}

		[DataMember]
		public int Id { get; private set; }
	}
}
