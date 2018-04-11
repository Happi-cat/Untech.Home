using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Requests;
using Untech.Home;
using Untech.Home.Data;
using Untech.Practices.Collections;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;

namespace Untech.FinancePlanner.Data
{
	public class FinancialJournalEntryStorage : GenericDataStorage<FinancialJournalEntry, FinancialJournalEntryDao>,
		IQueryAsyncHandler<FinancialJournalQuery, IEnumerable<FinancialJournalEntry>>
	{
		private readonly IQueryDispatcher _dispatcher;

		public FinancialJournalEntryStorage(IQueryDispatcher dispatcher, Func<FinancialPlannerContext> contextFactory)
			: base(contextFactory, DaoMapper.Instance, DaoMapper.Instance)
		{
			_dispatcher = dispatcher;
		}

		public async Task<IEnumerable<FinancialJournalEntry>> HandleAsync(FinancialJournalQuery request,
			CancellationToken cancellationToken)
		{
			var from = request.AsMonthDate();
			var to = from.AddMonths(1);

			var rootTaxon = _dispatcher.Fetch(request.Taxon ?? new TaxonTreeQuery());

			var entries = new List<FinancialJournalEntry>();

			using (var context = GetContext())
			{
				foreach (var taxon in rootTaxon.DescendantsAndSelf())
				{
					var daos = await GetTable(context)
						.Where(n => n.TaxonKey == taxon.Key && from <= n.When && n.When < to)
						.ToListAsync(cancellationToken);

					entries.AddRange(daos.Select(FinancialJournalEntryDao.ToEntity));
				}
			}

			return entries;
		}
	}
}