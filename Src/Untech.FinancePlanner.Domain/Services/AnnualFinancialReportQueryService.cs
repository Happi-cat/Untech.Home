using System;
using System.Linq;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Notifications;
using Untech.FinancePlanner.Domain.Requests;
using Untech.FinancePlanner.Domain.ViewModels;
using Untech.Practices;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;
using Untech.Practices.DataStorage.Cache;

namespace Untech.FinancePlanner.Domain.Services
{
	public class AnnualFinancialReportQueryService :
		IQueryHandler<AnnualFinancialReportQuery, AnnualFinancialReport>,
		INotificationHandler<FinancialJournalEntrySaved>,
		INotificationHandler<FinancialJournalEntryDeleted>
	{
		private readonly IQueryDispatcher _dispatcher;
		private readonly ICacheStorage _cacheStorage;

		public AnnualFinancialReportQueryService(IQueryDispatcher dispatcher, ICacheStorage cacheStorage)
		{
			_dispatcher = dispatcher;
			_cacheStorage = cacheStorage;
		}

		public AnnualFinancialReport Handle(AnnualFinancialReportQuery request)
		{
			var thisMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
			var rootTaxon = _dispatcher.Fetch(new TaxonTreeQuery { Deep = 3 });

			var builder = new MonthReportBuilder(_dispatcher, rootTaxon);

			return new AnnualFinancialReport
			{
				Entries = rootTaxon.Elements,
				Months = Enumerable.Range(-3 + request.ShiftMonth, 12)
					.Select(thisMonth.AddMonths)
					.Select(n => GetReportFromCacheOrBuild(n, builder))
					.ToList()
			};
		}

		public void Publish(FinancialJournalEntrySaved notification)
		{
			var cacheKey = GetMonthlyReportKey(notification.Entry.When);
			_cacheStorage.Drop(cacheKey);
		}

		public void Publish(FinancialJournalEntryDeleted notification)
		{
			var cacheKey = GetMonthlyReportKey(notification.Entry.When);
			_cacheStorage.Drop(cacheKey);
		}

		private static CacheKey GetMonthlyReportKey(DateTime when)
		{
			return new CacheKey("reports", $"annual-financial-report/month/{when.Year}/{when.Month}");
		}

		private AnnualFinancialReportMonth GetReportFromCacheOrBuild(DateTime thatMonth, MonthReportBuilder builder)
		{
			var cacheKey = GetMonthlyReportKey(thatMonth);
			var report = _cacheStorage.Get<AnnualFinancialReportMonth>(cacheKey);

			if (report == null)
			{
				report = builder.GetReport(thatMonth);

				_cacheStorage.Set(cacheKey, report);
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

			public AnnualFinancialReportMonth GetReport(DateTime thatMonth)
			{
				_thatMonth = thatMonth;

				var report = new AnnualFinancialReportMonth(_thatMonth)
				{
					Entries = _taxon.GetElements()
						.Select(BuildReportEntry)
						.Where(IsNotEmptyActualOrForecasted)
						.ToList()
				};

				var incomes = report.Entries
					.Where(n => n.TaxonKey == BuiltInTaxonId.Income)
					.ToList();
				var expenses = report.Entries
					.Where(n => n.TaxonKey == BuiltInTaxonId.Expense)
					.ToList();

				report.ActualTotals = incomes
					.Sum(n => n.ActualTotals)
					.Subtract(expenses
						.Sum(n => n.ActualTotals));

				report.ForecastedTotals = incomes
					.Sum(n => n.ForecastedTotals)
					.Subtract(expenses
						.Sum(n => n.ForecastedTotals));

				return report;
			}

			private AnnualFinancialReportMonthEntry BuildReportEntry(TaxonTree currentTaxon)
			{
				var financialJournalEntries = _dispatcher.Fetch(new FinancialJournalQuery(_thatMonth)
				{
					Taxon = new TaxonTreeQuery
					{
						TaxonKey = currentTaxon.Key,
						Deep = currentTaxon.GetElements().Any() ? 0 : -1
					}
				});

				var entry = new AnnualFinancialReportMonthEntry(currentTaxon.Key)
				{
					Actual = financialJournalEntries.Sum(n => n.Actual),
					Forecasted = financialJournalEntries.Sum(n => n.Forecasted),
					Entries = currentTaxon.GetElements()
						.Select(BuildReportEntry)
						.Where(IsNotEmptyActualOrForecasted)
						.ToList()
				};

				entry.ActualTotals = entry.Entries
					.Select(n => n.ActualTotals)
					.Concat(new[] { entry.Actual })
					.Sum();

				entry.ForecastedTotals = entry.Entries
					.Select(n => n.ForecastedTotals)
					.Concat(new[] { entry.Forecasted })
					.Sum();

				return entry;
			}
		}

		private static bool IsNotEmptyActualOrForecasted(AnnualFinancialReportMonthEntry entry)
		{
			return new[] { entry.ActualTotals, entry.ForecastedTotals }
				.Select(n => n?.Amount ?? 0)
				.Any(n => n != 0);
		}
	}
}