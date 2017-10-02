using System;
using Untech.FinancePlanner.Domain.Models;
using Untech.Practices.CQRS;

namespace Untech.FinancePlanner.Domain.Notifications
{
	public abstract class FinancialJournalEntryNotification : INotification
	{
		public FinancialJournalEntryNotification(FinancialJournalEntry entry)
		{
			Id = entry.Id;
			TaxonId = entry.TaxonId;
			When = entry.When;
		}

		public int Id { get; private set; }

		public int TaxonId { get; private set; }

		public DateTime When { get; private set; }
	}
}