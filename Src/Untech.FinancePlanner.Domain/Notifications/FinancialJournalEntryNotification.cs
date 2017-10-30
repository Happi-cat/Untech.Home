using System;
using Untech.FinancePlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.FinancePlanner.Domain.Notifications
{
	public abstract class FinancialJournalEntryNotification : INotification
	{
		public FinancialJournalEntryNotification(FinancialJournalEntry entry)
		{
			Key = entry.Key;
			TaxonKey = entry.TaxonKey;
			When = entry.When;
		}

		public int Key { get; private set; }

		public int TaxonKey { get; private set; }

		public DateTime When { get; private set; }
	}
}