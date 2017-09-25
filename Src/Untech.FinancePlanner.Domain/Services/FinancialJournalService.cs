using System;
using System.Collections.Generic;
using System.Linq;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Requests;
using Untech.Practices.Collections;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.Repos.Queryable;

namespace Untech.FinancePlanner.Domain.Services
{
	public class FinancialJournalService :
		IQueryHandler<FinancialJournalQuery, IEnumerable<FinancialJournalEntry>>,
		ICommandHandler<CreateFinancialJournalEntry, FinancialJournalEntry>,
		ICommandHandler<DeleteFinancialJournalEntry, bool>,
		ICommandHandler<UpdateFinancialJournalEntry, FinancialJournalEntry>
	{
		private readonly IRepository<FinancialJournalEntry> _repo;
		private readonly IDispatcher _dispatcher;

		public FinancialJournalService(IDispatcher dispatcher, IRepository<FinancialJournalEntry> repo)
		{
			_dispatcher = dispatcher;
			_repo = repo;
		}

		public IEnumerable<FinancialJournalEntry> Handle(FinancialJournalQuery request)
		{
			var from = new DateTime(request.Year, request.Month, 1);
			var to = from.AddMonths(1);

			var rootTaxon = _dispatcher.Fetch(request.Taxon ?? new TaxonTreeQuery());

			var entries = new List<FinancialJournalEntry>();

			foreach (var taxon in rootTaxon.DescendantsAndSelf())
			{
				entries.AddRange(_repo.GetAll()
					.Where(n => n.TaxonId == taxon.Id && n.When >= from && n.When < to));
			}

			return entries;
		}

		public FinancialJournalEntry Handle(CreateFinancialJournalEntry request)
		{
			if (request.TaxonId == BuiltInTaxonId.Root) throw new ArgumentException("request.TaxonId");

			return _repo.Create(new FinancialJournalEntry(0, request.TaxonId)
			{
				Remarks = request.Remarks,
				Actual = request.Actual,
				Forecasted = request.Forecasted,
				When = request.When.Date
			});
		}

		public bool Handle(DeleteFinancialJournalEntry request)
		{
			return _repo.Delete(new FinancialJournalEntry(request.Id, 0));
		}

		public FinancialJournalEntry Handle(UpdateFinancialJournalEntry request)
		{
			var entry = _repo.GetAll()
				.Single(n => n.Id == request.Id);

			entry.Remarks = request.Remarks;
			entry.Actual = request.Actual;
			entry.Forecasted = request.Forecasted;

			_repo.Update(entry);

			return entry;
		}
	}
}