using System;
using System.Collections.Generic;
using System.Linq;
using LinqToDB;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Requests;
using Untech.FinancePlanner.Domain.Requests.FinancialJournalEntry;
using Untech.FinancePlanner.Domain.Requests.Taxon;
using Untech.Home;
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

		public FinancialJournalEntryStorage(IQueryDispatcher dispatcher, Func<FinancialPlannerContext> connectionFactory)
		{
			_contextFactory = connectionFactory;
			_dispatcher = dispatcher;
		}

		public FinancialJournalEntry Find(int key)
		{
			using (var context = _contextFactory())
			{
				var dao = context
						.GetTable<FinancialJournalEntryDao>()
						.SingleOrDefault(n => n.Key == key)
					?? throw new ObjectNotFoundException(typeof(FinancialJournalEntry), key);
				return FinancialJournalEntryDao.ToEntity(dao);
			}
		}

		public FinancialJournalEntry Create(FinancialJournalEntry entity)
		{
			using (var context = _contextFactory())
			{
				var key = context.InsertWithInt32Identity(new FinancialJournalEntryDao(entity));
				return Find(key);
			}
		}

		public bool Delete(FinancialJournalEntry entity)
		{
			using (var context = _contextFactory())
			{
				return context.Delete(new FinancialJournalEntryDao(entity)) > 0;
			}
		}

		public FinancialJournalEntry Update(FinancialJournalEntry entity)
		{
			using (var context = _contextFactory())
			{
				context.Update(new FinancialJournalEntryDao(entity));
				return entity;
			}
		}

		public IEnumerable<FinancialJournalEntry> Handle(FinancialJournalQuery request)
		{
			var from = request.AsMonthDate();
			var to = from.AddMonths(1);

			var rootTaxon = _dispatcher.Fetch(request.Taxon ?? new TaxonTreeQuery());

			var entries = new List<FinancialJournalEntry>();

			using (var context = _contextFactory())
			{
				foreach (var taxon in rootTaxon.DescendantsAndSelf())
				{
					var daos = context.GetTable<FinancialJournalEntryDao>()
						.Where(n => n.TaxonKey == taxon.Key && from <= n.When && n.When < to)
						.ToList();

					entries.AddRange(daos.Select(FinancialJournalEntryDao.ToEntity));
				}
			}

			return entries;
		}
	}
}