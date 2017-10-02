using System;
using Untech.FinancePlanner.Domain.Models;

namespace Untech.FinancePlanner.Domain.Notifications
{
	public class FinancialJournalEntrySaved : FinancialJournalEntryNotification
	{
		public FinancialJournalEntrySaved(FinancialJournalEntry entry)
			: base(entry)
		{
		}
	}
}