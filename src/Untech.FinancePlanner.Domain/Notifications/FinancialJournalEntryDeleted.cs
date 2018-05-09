using System;
using Untech.FinancePlanner.Domain.Models;

namespace Untech.FinancePlanner.Domain.Notifications
{
	public class FinancialJournalEntryDeleted : FinancialJournalEntryNotification
	{
		public FinancialJournalEntryDeleted(FinancialJournalEntry entry)
			: base(entry)
		{
		}
	}
}