using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Notifications;
using Untech.FinancePlanner.Domain.Requests;
using Untech.FinancePlanner.Domain.Views;
using Untech.Home;
using Untech.Practices;
using Untech.Practices.Collections;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage.Cache;

namespace Untech.FinancePlanner.Domain.Services
{
	public class MonthlyFinancialReportQueryService :
		IQueryAsyncHandler<MonthlyFinancialReportQuery, MonthlyFinancialReport>,
		INotificationAsyncHandler<FinancialJournalEntrySaved>,
		INotificationAsyncHandler<FinancialJournalEntryDeleted>
	{
		private readonly IQueryDispatcher _dispatcher;
		private readonly IAsyncCacheStorage _cacheStorage;

		public MonthlyFinancialReportQueryService(IQueryDispatcher dispatcher, IAsyncCacheStorage cacheStorage)
		{
			_dispatcher = dispatcher;
			_cacheStorage = cacheStorage;
		}

		public async Task<MonthlyFinancialReport> HandleAsync(MonthlyFinancialReportQuery request, CancellationToken cancellationToken)
		{
			var cacheKey = GetKey(request.AsMonthDate());
			var report = await _cacheStorage.GetAsync<MonthlyFinancialReport>(cacheKey, cancellationToken);

			if (report == null)
			{
				report = await BuildReportAsync(request, cancellationToken);
				await _cacheStorage.SetAsync(cacheKey, report, cancellationToken);
			}

			return report;
		}

		public Task PublishAsync(FinancialJournalEntrySaved notification, CancellationToken cancellationToken)
		{
			return _cacheStorage.DropAsync(GetKey(notification.Entry.When), cancellationToken: cancellationToken);
		}

		public Task PublishAsync(FinancialJournalEntryDeleted notification, CancellationToken cancellationToken)
		{
			return _cacheStorage.DropAsync(GetKey(notification.Entry.When), cancellationToken: cancellationToken);
		}

		private async Task<MonthlyFinancialReport> BuildReportAsync(MonthlyFinancialReportQuery request, CancellationToken cancellationToken)
		{
			var taxons = await GetTaxonsAsync(cancellationToken);
			var dayBuilder = new DayReportBuilder(taxons);

			var entries = await _dispatcher.FetchAsync(new FinancialJournalQuery(request.Year, request.Month)
			{
				Taxon = new TaxonTreeQuery { Deep = -1 }
			}, cancellationToken);

			return new MonthlyFinancialReport(request.Year, request.Month)
			{
				Days = entries
					.GroupBy(n => n.When.Date)
					.Select(dayBuilder.GetReport)
					.OrderBy(n => n.Day)
					.ToList()
			};
		}

		private CacheKey GetKey(DateTime when)
		{
			return new CacheKey("reports", $"monthly-financial-report/{when.Year}/{when.Month}");
		}

		private async Task<IReadOnlyDictionary<int, TaxonTree>> GetTaxonsAsync(CancellationToken cancellationToken)
		{
			var taxonTree = await _dispatcher
				.FetchAsync(new TaxonTreeQuery { Deep = -1 }, cancellationToken);

			return taxonTree
				.DescendantsAndSelf()
				.ToDictionary(n => n.Key);
		}

		private class DayReportBuilder
		{
			private readonly IReadOnlyDictionary<int, TaxonTree> _taxons;

			public DayReportBuilder(IReadOnlyDictionary<int, TaxonTree> taxons)
			{
				_taxons = taxons;
			}

			public MonthlyFinancialReportDay GetReport(IGrouping<DateTime, FinancialJournalEntry> dayEntries)
			{
				var report = new MonthlyFinancialReportDay(dayEntries.Key.Day)
				{
					Entries = dayEntries.Select(BuildDayReportEntry).ToList()
				};

				report.ActualTotals = report.Entries.Sum(n => n.Actual);
				report.ForecastedTotals = report.Entries.Sum(n => n.Forecasted);

				return report;
			}

			private MonthlyFinancialReportDayEntry BuildDayReportEntry(FinancialJournalEntry entry)
			{
				return new MonthlyFinancialReportDayEntry(GetName(entry.TaxonKey), entry.TaxonKey)
				{
					Remarks = entry.Remarks,
					Actual = entry.Actual,
					Forecasted = entry.Forecasted
				};
			}

			private string GetName(int taxonKey)
			{
				var names = new Stack<string>();

				var currentTaxon = _taxons[taxonKey];
				while (!currentTaxon.IsSystemRoot)
				{
					names.Push(currentTaxon.Name);

					currentTaxon = _taxons[currentTaxon.ParentKey];
				}

				return string.Join(" / ", names);
			}
		}
	}
}