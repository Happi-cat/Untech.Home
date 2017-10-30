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
		private readonly IDispatcher _dispatcher;
		private readonly ICacheStorage _cacheStorage;

		public AnnualFinancialReportQueryService(IDispatcher dispatcher, ICacheStorage cacheStorage)
		{
			_dispatcher = dispatcher;
			_cacheStorage = cacheStorage;
		}

		public AnnualFinancialReport Handle(AnnualFinancialReportQuery request)
		{
			var thisMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
			var rootTaxon = _dispatcher.Fetch(new TaxonTreeQuery { Deep = 3 });

			var builder = new MonthlyReportBuilder(_dispatcher, _cacheStorage, rootTaxon);

			return new AnnualFinancialReport
			{
				Entries = rootTaxon.Elements,
				Months = Enumerable.Range(-3 + request.ShiftMonth, 12)
					.Select(thisMonth.AddMonths)
					.Select(builder.GetReport)
					.ToList()
			};
		}

		public void Publish(FinancialJournalEntrySaved notification)
		{
			var cacheKey = GetMonthlyReportKey(notification.When);
			_cacheStorage.Drop(cacheKey);
		}

		public void Publish(FinancialJournalEntryDeleted notification)
		{
			var cacheKey = GetMonthlyReportKey(notification.When);
			_cacheStorage.Drop(cacheKey);
		}

		private static CacheKey GetMonthlyReportKey(DateTime when)
		{
			return new CacheKey("reports", $"financial-monthly-report/{when.Year}/{when.Month}");
		}

		private class MonthlyReportBuilder
		{
			private readonly IDispatcher _dispatcher;

			private readonly ICacheStorage _cacheStorage;

			private readonly TaxonTree _taxon;

			private DateTime _thatMonth;

			public MonthlyReportBuilder(IDispatcher dispatcher, ICacheStorage cacheStorage, TaxonTree taxon)
			{
				_dispatcher = dispatcher;
				_cacheStorage = cacheStorage;
				_taxon = taxon;
			}

			public MonthlyFinancialReport GetReport(DateTime thatMonth)
			{
				_thatMonth = thatMonth;

				if (_cacheStorage != null)
				{
					var cacheKey = GetMonthlyReportKey(thatMonth);
					var cached = _cacheStorage.Get<MonthlyFinancialReport>(cacheKey);

					if (cached == null)
					{
						cached = BuildReport();

						_cacheStorage.Set(cacheKey, cached);
					}

					return cached;
				}

				return BuildReport();
			}

			private MonthlyFinancialReport BuildReport()
			{
				var report = new MonthlyFinancialReport(_thatMonth)
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
					.Sum(n => n.Actual)
					.Subtract(expenses
						.Sum(n => n.Actual));

				report.ForecastedTotals = incomes
					.Sum(n => n.Forecasted)
					.Subtract(expenses
						.Sum(n => n.Forecasted));

				return report;
			}

			private MonthlyFinancialReportEntry BuildReportEntry(TaxonTree currentTaxon)
			{
				var entry = new MonthlyFinancialReportEntry(currentTaxon.Key)
				{
					Entries = currentTaxon.GetElements()
						.Select(BuildReportEntry)
						.Where(IsNotEmptyActualOrForecasted)
						.ToList()
				};

				var financialJournalEntries = _dispatcher.Fetch(new FinancialJournalQuery(_thatMonth)
				{
					Taxon = new TaxonTreeQuery
					{
						TaxonKey = currentTaxon.Key,
						Deep = currentTaxon.GetElements().Any() ? 0 : -1
					}
				});

				entry.Actual = financialJournalEntries
						.Sum(n => n.Actual);

				entry.ActualTotals = entry.Entries
					.Select(n => n.ActualTotals)
					.Concat(new[] { entry.Actual })
					.Sum();

				entry.Forecasted = financialJournalEntries
					.Sum(n => n.Forecasted);

				entry.ForecastedTotals = entry.Entries
					.Select(n => n.ForecastedTotals)
					.Concat(new[] { entry.Forecasted })
					.Sum();

				return entry;
			}
		}

		private static bool IsNotEmptyActualOrForecasted(MonthlyFinancialReportEntry entry)
		{
			return new[] { entry.ActualTotals, entry.ForecastedTotals }
				.Select(n => n?.Amount ?? 0)
				.Any(n => n != 0);
		}
	}
}