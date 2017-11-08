using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqToDB;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Requests;
using Untech.Practices.Collections;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage;

namespace Untech.FinancePlanner.Data
{
	public class FinancialJournalEntryStorage : IDataStorage<FinancialJournalEntry>,
		IQueryHandler<FinancialJournalQuery, IEnumerable<FinancialJournalEntry>>
	{
		private readonly Func<IDataContext> _contextFactory;
		private readonly IQueryDispatcher _dispatcher;

		public FinancialJournalEntryStorage(IQueryDispatcher dispatcher, Func<IDataContext> connectionFactory)
		{
			_contextFactory = connectionFactory;
			_dispatcher = dispatcher;
		}

		public FinancialJournalEntry Find(int key)
		{
			using (var context = _contextFactory())
			{
				var dto = context.GetTable<FinancialJournalEntryDto>().SingleOrDefault(n => n.Key == key);
				return FinancialJournalEntryDto.Convert(dto);
			}
		}

		public FinancialJournalEntry Create(FinancialJournalEntry entity)
		{
			using (var context = _contextFactory())
			{
				var key = context.Insert(FinancialJournalEntryDto.Convert(entity));
				return Find(key);
			}
		}

		public bool Delete(FinancialJournalEntry entity)
		{
			using (var context = _contextFactory())
			{
				return context.Delete(FinancialJournalEntryDto.Convert(entity)) > 0;
			}
		}

		public FinancialJournalEntry Update(FinancialJournalEntry entity)
		{
			using (var context = _contextFactory())
			{
				context.Update(FinancialJournalEntryDto.Convert(entity));
				return entity;
			}
		}

		public IEnumerable<FinancialJournalEntry> Handle(FinancialJournalQuery request)
		{
			var from = new DateTime(request.Year, request.Month, 1);
			var to = from.AddMonths(1);

			var rootTaxon = _dispatcher.Fetch(request.Taxon ?? new TaxonTreeQuery());

			var entries = new List<FinancialJournalEntry>();

			using (var context = _contextFactory())
			{
				foreach (var taxon in rootTaxon.DescendantsAndSelf())
				{
					var entriesToAdd = context.GetTable<FinancialJournalEntryDto>()
						.Where(n => n.TaxonKey == taxon.Key && n.When >= from && n.When < to)
						.ToList()
						.Select(FinancialJournalEntryDto.Convert);

					entries.AddRange(entriesToAdd);
				}
			}

			return entries;
		}

	}
}