using System;
using System.Threading;
using System.Threading.Tasks;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Notifications;
using Untech.FinancePlanner.Domain.Requests;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;

namespace Untech.FinancePlanner.Domain.Services
{
	public class FinancialJournalService :
		ICommandAsyncHandler<CreateFinancialJournalEntry, FinancialJournalEntry>,
		ICommandAsyncHandler<DeleteFinancialJournalEntry, bool>,
		ICommandAsyncHandler<UpdateFinancialJournalEntry, FinancialJournalEntry>
	{
		private readonly IAsyncDataStorage<FinancialJournalEntry> _dataStorage;
		private readonly IDispatcher _dispatcher;
		private readonly IQueueDispatcher _queueDispatcher;

		public FinancialJournalService(IDispatcher dispatcher,
			IQueueDispatcher queueDispatcher,
			IAsyncDataStorage<FinancialJournalEntry> dataStorage)
		{
			_dispatcher = dispatcher;
			_queueDispatcher = queueDispatcher;
			_dataStorage = dataStorage;
		}

		public async Task<FinancialJournalEntry> HandleAsync(CreateFinancialJournalEntry request, CancellationToken cancellationToken)
		{
			var taxon = await _dispatcher.FetchAsync(new TaxonTreeQuery { TaxonKey = request.TaxonKey }, cancellationToken);
			if (!taxon.IsSelectable) throw new InvalidOperationException("Taxon is no selectable");

			var entry = await _dataStorage.CreateAsync(new FinancialJournalEntry(0, request.TaxonKey)
			{
				Remarks = request.Remarks,
				Actual = request.Actual,
				Forecasted = request.Forecasted,
				When = GetWhenFromRequest(request)
			}, cancellationToken);

			_queueDispatcher.Enqueue(new FinancialJournalEntrySaved(entry));

			return entry;
		}

		public async Task<bool> HandleAsync(DeleteFinancialJournalEntry request, CancellationToken cancellationToken)
		{
			var entry = await _dataStorage.FindAsync(request.Key, cancellationToken);
			var result = await _dataStorage.DeleteAsync(entry, cancellationToken);

			if (result)
			{
				_queueDispatcher.Enqueue(new FinancialJournalEntryDeleted(entry));
			}

			return result;
		}

		public async Task<FinancialJournalEntry> HandleAsync(UpdateFinancialJournalEntry request, CancellationToken cancellationToken)
		{
			var entry = await _dataStorage.FindAsync(request.Key, cancellationToken);

			entry.Remarks = request.Remarks;
			entry.Actual = request.Actual;
			entry.Forecasted = request.Forecasted;

			await _dataStorage.UpdateAsync(entry, cancellationToken);

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