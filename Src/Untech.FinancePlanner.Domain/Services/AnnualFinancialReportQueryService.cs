using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.FinancePlanner.Domain.Notifications;
using Untech.FinancePlanner.Domain.Requests;
using Untech.FinancePlanner.Domain.Views;
using Untech.Home;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage.Cache;

namespace Untech.FinancePlanner.Domain.Services
{
	public class AnnualFinancialReportQueryService :
		IQueryAsyncHandler<AnnualFinancialReportQuery, AnnualFinancialReport>,
		INotificationAsyncHandler<FinancialJournalEntrySaved>,
		INotificationAsyncHandler<FinancialJournalEntryDeleted>
	{
		private readonly IQueryDispatcher _dispatcher;
		private readonly IAsyncCacheStorage _cacheStorage;

		public AnnualFinancialReportQueryService(IQueryDispatcher dispatcher, IAsyncCacheStorage cacheStorage)
		{
			_dispatcher = dispatcher;
			_cacheStorage = cacheStorage;
		}

		public async Task<AnnualFinancialReport> HandleAsync(AnnualFinancialReportQuery request, CancellationToken cancellationToken)
		{
			var thisMonth = DateTime.Today.AsMonthDate();
			var rootTaxon = await _dispatcher.FetchAsync(new TaxonTreeQuery { Deep = 3 }, cancellationToken);

			var builder = new MonthReportBuilder(_dispatcher, rootTaxon);
			var months = await Task.WhenAll(Enumerable.Range(-3 + request.ShiftMonth, 12)
				.Select(thisMonth.AddMonths)
				.Select(n => GetReportFromCacheOrBuildAsync(n, builder, cancellationToken)));

			return new AnnualFinancialReport
			{
				Entries = rootTaxon.Elements,
				Months = months.ToList()
			};
		}

		public Task PublishAsync(FinancialJournalEntrySaved notification, CancellationToken cancellationToken)
		{
			var cacheKey = GetMonthlyReportKey(notification.Entry.When);
			return _cacheStorage.DropAsync(cacheKey, cancellationToken: cancellationToken);
		}

		public Task PublishAsync(FinancialJournalEntryDeleted notification, CancellationToken cancellationToken)
		{
			var cacheKey = GetMonthlyReportKey(notification.Entry.When);
			return _cacheStorage.DropAsync(cacheKey, cancellationToken: cancellationToken);
		}

		private static CacheKey GetMonthlyReportKey(DateTime when)
		{
			return new CacheKey("reports", $"annual-financial-report/month/{when.Year}/{when.Month}");
		}

		private async Task<AnnualFinancialReportMonth> GetReportFromCacheOrBuildAsync(DateTime thatMonth, MonthReportBuilder builder, CancellationToken cancellationToken)
		{
			var cacheKey = GetMonthlyReportKey(thatMonth);
			var report = await _cacheStorage.GetAsync<AnnualFinancialReportMonth>(cacheKey, cancellationToken);

			if (report == null)
			{
				report = await builder.GetReportAsync(thatMonth, cancellationToken);

				await _cacheStorage.SetAsync(cacheKey, report, cancellationToken);
			}

			return report;
		}

		private class MonthReportBuilder
		{
			private readonly IQueryDispatcher _dispatcher;

			private readonly TaxonTree _taxon;

			private DateTime _thatMonth;

			public MonthReportBuilder(IQueryDispatcher dispatcher, TaxonTree taxon)
			{
				_dispatcher = dispatcher;
				_taxon = taxon;
			}

			public async Task<AnnualFinancialReportMonth> GetReportAsync(DateTime thatMonth, CancellationToken cancellationToken)
			{
				_thatMonth = thatMonth;

				var entries = await Task.WhenAll(_taxon.GetElements()
					.Select(e => BuildReportEntryAsync(e, cancellationToken)));

				return AnnualFinancialReportMonth.Create(_thatMonth, entries);
			}

			private async Task<AnnualFinancialReportMonthEntry> BuildReportEntryAsync(TaxonTree currentTaxon, CancellationToken cancellationToken)
			{
				var financialJournalEntries = await _dispatcher.FetchAsync(new FinancialJournalQuery(_thatMonth)
				{
					Taxon = new TaxonTreeQuery
					{
						TaxonKey = currentTaxon.Key,
						Deep = currentTaxon.GetElements().Any() ? 0 : -1
					}
				}, cancellationToken);

				var entries = await Task.WhenAll(currentTaxon.GetElements()
					.Select(e => BuildReportEntryAsync(e, cancellationToken)));

				return AnnualFinancialReportMonthEntry.Create(currentTaxon, financialJournalEntries, entries);
			}
		}
	}
}