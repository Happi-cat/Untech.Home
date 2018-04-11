using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Requests;
using Untech.FinancePlanner.Domain.Views;
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
		public Task<AnnualFinancialReport> GetReport(int shiftMonth = 0)
		{
			return _dispatcher
				.FetchAsync(new AnnualFinancialReportQuery { ShiftMonth = shiftMonth }, CancellationToken.None);
		}

		[HttpGet("report/{year}/{month}")]
		public Task<MonthlyFinancialReport> GetMonthlyReport(int year, int month)
		{
			return _dispatcher
				.FetchAsync(new MonthlyFinancialReportQuery(year, month), CancellationToken.None);
		}

		[HttpGet("taxon")]
		public Task<TaxonTree> GetTaxonTree(int deep)
		{
			return _dispatcher
				.FetchAsync(new TaxonTreeQuery { Deep = deep }, CancellationToken.None);
		}

		[HttpGet("taxon/{taxonKey}")]
		public Task<TaxonTree> GetTaxonTree(int taxonKey, int deep = 0)
		{
			return _dispatcher
				.FetchAsync(new TaxonTreeQuery { TaxonKey = taxonKey, Deep = deep }, CancellationToken.None);
		}

		[HttpGet("journal")]
		public Task<IEnumerable<FinancialJournalEntry>> GetJournalEntries(int taxonId = 0, int deep = 0)
		{
			return _dispatcher
				.FetchAsync(new FinancialJournalQuery(DateTime.Today)
				{
					Taxon = new TaxonTreeQuery { TaxonKey = taxonId, Deep = deep }
				}, CancellationToken.None);
		}

		[HttpGet("journal/{year}/{month}")]
		public Task<IEnumerable<FinancialJournalEntry>> GetJournalEntries(int year, int month, int taxonId = 0, int deep = 0)
		{
			return _dispatcher
				.FetchAsync(new FinancialJournalQuery(year, month)
				{
					Taxon = new TaxonTreeQuery { TaxonKey = taxonId, Deep = deep }
				}, CancellationToken.None);
		}

		[HttpPost("journal")]
		public Task<FinancialJournalEntry> CreateJournalEntry([FromBody]CreateFinancialJournalEntry request)
		{
			return _dispatcher.ProcessAsync(request, CancellationToken.None);
		}

		[HttpPut("journal/{key}")]
		public Task<FinancialJournalEntry> UpdateJournalEntry(int key, [FromBody]UpdateFinancialJournalEntry request)
		{
			if (request.Key != key) throw new ArgumentException(nameof(key));

			return _dispatcher.ProcessAsync(request, CancellationToken.None);
		}

		[HttpDelete("journal/{key}")]
		public Task<bool> DeleteJournalEntry(int key)
		{
			return _dispatcher.ProcessAsync(new DeleteFinancialJournalEntry(key), CancellationToken.None);
		}
	}
}