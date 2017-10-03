using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Requests;
using Untech.FinancePlanner.Domain.ViewModels;
using Untech.Practices.CQRS.Dispatching;

namespace Untech.Home.Web.Controllers
{
	[Route("api/financial-planner")]
	public class FinancialPlannerController : Controller
	{
		private readonly IDispatcher _dispatcher;

		public FinancialPlannerController(IDispatcher dispatcher)
		{
			_dispatcher = dispatcher;
		}

		[HttpGet("report")]
		public AnnualFinancialReport GetReport(int shiftMonth = 0) => _dispatcher
			.Fetch(new AnnualFinancialReportQuery { ShiftMonth = shiftMonth });

		[HttpGet("taxon")]
		public TaxonTree GetTaxonTree(int deep) => _dispatcher
			.Fetch(new TaxonTreeQuery { Deep = deep });

		[HttpGet("taxon/{taxonId}")]
		public TaxonTree GetTaxonTree(int taxonId, int deep = 0) => _dispatcher
			.Fetch(new TaxonTreeQuery { TaxonId = taxonId, Deep = deep });

		[HttpGet("journal")]
		public IEnumerable<FinancialJournalEntry> GetJournalEntries(int taxonId = 0, int deep = 0) => _dispatcher
			.Fetch(new FinancialJournalQuery(DateTime.Today)
			{
				Taxon = new TaxonTreeQuery { TaxonId = taxonId, Deep = deep }
			});

		[HttpGet("journal/{year}/{month}")]
		public IEnumerable<FinancialJournalEntry> GetJournalEntries(int year, int month, int taxonId = 0, int deep = 0) => _dispatcher
			.Fetch(new FinancialJournalQuery(year, month)
			{
				Taxon = new TaxonTreeQuery { TaxonId = taxonId, Deep = deep }
			});

		[HttpPost("journal")]
		public FinancialJournalEntry CreateJournalEntry([FromBody]CreateFinancialJournalEntry request)
		{
			return _dispatcher.Process(request);
		}

		[HttpPut("journal/{id}")]
		public FinancialJournalEntry UpdateJournalEntry(int id, [FromBody]UpdateFinancialJournalEntry request)
		{
			if (request.Id != id) throw new ArgumentException(nameof(id));

			return _dispatcher.Process(request);
		}

		[HttpDelete("journal/{id}")]
		public bool DeleteJournalEntry(int id)
		{
			return _dispatcher.Process(new DeleteFinancialJournalEntry(id));
		}
	}
}