using System;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Notifications;
using Untech.FinancePlanner.Domain.Requests;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;

namespace Untech.FinancePlanner.Domain.Services
{
	public class FinancialJournalService :
		ICommandHandler<CreateFinancialJournalEntry, FinancialJournalEntry>,
		ICommandHandler<DeleteFinancialJournalEntry, bool>,
		ICommandHandler<UpdateFinancialJournalEntry, FinancialJournalEntry>
	{
		private readonly IDataStorage<FinancialJournalEntry> _dataStorage;
		private readonly IDispatcher _dispatcher;
		private readonly IQueueDispatcher _queueDispatcher;

		public FinancialJournalService(IDispatcher dispatcher,
			IQueueDispatcher queueDispatcher,
			IDataStorage<FinancialJournalEntry> dataStorage)
		{
			_dispatcher = dispatcher;
			_queueDispatcher = queueDispatcher;
			_dataStorage = dataStorage;
		}

		public FinancialJournalEntry Handle(CreateFinancialJournalEntry request)
		{
			var taxon = _dispatcher.Fetch(new TaxonTreeQuery { TaxonKey = request.TaxonKey });
			if (!taxon.IsSelectable) throw new InvalidOperationException("Taxon is no selectable");

			var entry = _dataStorage.Create(new FinancialJournalEntry(0, request.TaxonKey)
			{
				Remarks = request.Remarks,
				Actual = request.Actual,
				Forecasted = request.Forecasted,
				When = GetWhenFromRequest(request)
			});

			_queueDispatcher.Enqueue(new FinancialJournalEntrySaved(entry));

			return entry;
		}

		public bool Handle(DeleteFinancialJournalEntry request)
		{
			var entry = _dataStorage.Find(request.Key);
			var result = _dataStorage.Delete(entry);

			if (result)
			{
				_queueDispatcher.Enqueue(new FinancialJournalEntryDeleted(entry));
			}

			return result;
		}

		public FinancialJournalEntry Handle(UpdateFinancialJournalEntry request)
		{
			var entry = _dataStorage.Find(request.Key);

			entry.Remarks = request.Remarks;
			entry.Actual = request.Actual;
			entry.Forecasted = request.Forecasted;

			_dataStorage.Update(entry);

			_queueDispatcher.Enqueue(new FinancialJournalEntrySaved(entry));
			return entry;
		}

		private static DateTime GetWhenFromRequest(CreateFinancialJournalEntry request)
		{
			var when = DateTime.Today;

			return when.Year != request.Year || when.Month != request.Month
				? new DateTime(request.Year, request.Month, 1)
				: when;
		}
	}
}