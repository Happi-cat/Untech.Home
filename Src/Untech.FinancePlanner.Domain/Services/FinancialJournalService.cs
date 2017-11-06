using System;
using System.Collections.Generic;
using System.Linq;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Notifications;
using Untech.FinancePlanner.Domain.Requests;
using Untech.Practices;
using Untech.Practices.Collections;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;

namespace Untech.FinancePlanner.Domain.Services
{
	public class FinancialJournalService :
		IQueryHandler<FinancialJournalQuery, IEnumerable<FinancialJournalEntry>>,
		ICommandHandler<CreateFinancialJournalEntry, FinancialJournalEntry>,
		ICommandHandler<DeleteFinancialJournalEntry, bool>,
		ICommandHandler<UpdateFinancialJournalEntry, FinancialJournalEntry>
	{
		private readonly IDataStorage<FinancialJournalEntry> _dataStorage;
		private readonly IDispatcher _dispatcher;

		public FinancialJournalService(IDispatcher dispatcher, IDataStorage<FinancialJournalEntry> dataStorage)
		{
			_dispatcher = dispatcher;
			_dataStorage = dataStorage;
		}

		public IEnumerable<FinancialJournalEntry> Handle(FinancialJournalQuery request)
		{
			var from = new DateTime(request.Year, request.Month, 1);
			var to = from.AddMonths(1);

			var rootTaxon = _dispatcher.Fetch(request.Taxon ?? new TaxonTreeQuery());

			var entries = new List<FinancialJournalEntry>();

			foreach (var taxon in rootTaxon.DescendantsAndSelf())
			{
				entries.AddRange(_dataStorage.Find(n => n.TaxonKey == taxon.Key && n.When >= from && n.When < to));
			}

			return entries;
		}

		public FinancialJournalEntry Handle(CreateFinancialJournalEntry request)
		{
			var taxon = _dispatcher.Fetch(new TaxonTreeQuery { TaxonKey = request.TaxonKey });
			if (!taxon.IsSelectable) throw new InvalidOperationException("Taxon is no selectable");

			var when = DateTime.Today;
			if (when.Year != request.Year || when.Month != request.Month)
			{
				when = new DateTime(request.Year, request.Month, 1);
			}

			var entry = _dataStorage.Create(new FinancialJournalEntry(0, request.TaxonKey)
			{
				Remarks = request.Remarks,
				Actual = request.Actual,
				Forecasted = request.Forecasted,
				When = when.Date
			});

			_dispatcher.Publish(new FinancialJournalEntrySaved(entry));

			return entry;
		}

		public bool Handle(DeleteFinancialJournalEntry request)
		{
			var entry = _dataStorage.Find(request.Key);
			var result = _dataStorage.Delete(entry);

			if (result)
			{
				_dispatcher.Publish(new FinancialJournalEntryDeleted(entry));
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

			_dispatcher.Publish(new FinancialJournalEntrySaved(entry));
			return entry;
		}
	}
}