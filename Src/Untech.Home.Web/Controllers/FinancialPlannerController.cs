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

		[HttpGet("taxon/{taxonKey}")]
		public TaxonTree GetTaxonTree(int taxonKey, int deep = 0) => _dispatcher
			.Fetch(new TaxonTreeQuery { TaxonKey = taxonKey, Deep = deep });

		[HttpGet("journal")]
		public IEnumerable<FinancialJournalEntry> GetJournalEntries(int taxonId = 0, int deep = 0) => _dispatcher
			.Fetch(new FinancialJournalQuery(DateTime.Today)
			{
				Taxon = new TaxonTreeQuery { TaxonKey = taxonId, Deep = deep }
			});

		[HttpGet("journal/{year}/{month}")]
		public IEnumerable<FinancialJournalEntry> GetJournalEntries(int year, int month, int taxonId = 0, int deep = 0) => _dispatcher
			.Fetch(new FinancialJournalQuery(year, month)
			{
				Taxon = new TaxonTreeQuery { TaxonKey = taxonId, Deep = deep }
			});

		[HttpPost("journal")]
		public FinancialJournalEntry CreateJournalEntry([FromBody]CreateFinancialJournalEntry request)
		{
			return _dispatcher.Process(request);
		}

		[HttpPut("journal/{key}")]
		public FinancialJournalEntry UpdateJournalEntry(int key, [FromBody]UpdateFinancialJournalEntry request)
		{
			if (request.Key != key) throw new ArgumentException(nameof(key));

			return _dispatcher.Process(request);
		}

		[HttpDelete("journal/{key}")]
		public bool DeleteJournalEntry(int key)
		{
			return _dispatcher.Process(new DeleteFinancialJournalEntry(key));
		}
	}
}