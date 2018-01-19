using Untech.FinancePlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.FinancePlanner.Domain.Notifications
{
	public abstract class FinancialJournalEntryNotification : INotification
	{
		protected FinancialJournalEntryNotification(FinancialJournalEntry entry)
		{
			Entry = entry;
		}

		public FinancialJournalEntry Entry { get; }
	}
}