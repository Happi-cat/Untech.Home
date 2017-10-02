using System;
using System.Linq;
using Untech.FinancePlanner.Domain.Cache;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Notifications;
using Untech.FinancePlanner.Domain.Requests;
using Untech.FinancePlanner.Domain.ViewModels;
using Untech.Practices;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;

namespace Untech.FinancePlanner.Domain.Services
{
	public class AnnualFinancialReportQueryService :
		IQueryHandler<AnnualFinancialReportQuery, AnnualFinancialReport>,
		INotificationHandler<FinancialJournalEntrySaved>,
		INotificationHandler<FinancialJournalEntryDeleted>
	{
		private readonly IDispatcher _dispatcher;
		private readonly ICacheManager _cacheManager;

		public AnnualFinancialReportQueryService(IDispatcher dispatcher, ICacheManager cacheManager)
		{
			_dispatcher = dispatcher;
			_cacheManager = cacheManager;
		}

		public AnnualFinancialReport Handle(AnnualFinancialReportQuery request)
		{
			var thisMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
			var rootTaxon = _dispatcher.Fetch(new TaxonTreeQuery { Deep = 2 });

			var builder = new MonthlyReportBuilder(_dispatcher, _cacheManager, rootTaxon);

			return new AnnualFinancialReport
			{
				Entries = rootTaxon,
				Months = Enumerable.Range(-3, 12)
					 .Select(thisMonth.AddMonths)
					 .Select(builder.GetReport)
					 .ToList()
			};
		}

		public void Publish(FinancialJournalEntrySaved notification)
		{
			string cacheKey = GetMonthlyReportKey(notification.When);
			_cacheManager.Drop(cacheKey);
		}

		public void Publish(FinancialJournalEntryDeleted notification)
		{
			string cacheKey = GetMonthlyReportKey(notification.When);
			_cacheManager.Drop(cacheKey);
		}

		private static string GetMonthlyReportKey(DateTime when)
		{
			return $"cache://reports/financial-monthly-report/{when.Year}/{when.Month}";
		}

		private class MonthlyReportBuilder
		{
			private readonly IDispatcher _dispatcher;

			private readonly ICacheManager _cacheManager;

			private readonly TaxonTree _taxon;

			private DateTime _thatMonth;

			public MonthlyReportBuilder(IDispatcher dispatcher, ICacheManager cacheManager, TaxonTree taxon)
			{
				_dispatcher = dispatcher;
				_cacheManager = cacheManager;
				_taxon = taxon;
			}

			public MonthlyFinancialReport GetReport(DateTime thatMonth)
			{
				_thatMonth = thatMonth;

				if (_cacheManager != null)
				{
					var cacheKey = GetMonthlyReportKey(thatMonth);
					var cached = _cacheManager.Get<MonthlyFinancialReport>(cacheKey);

					if (cached == null)
					{
						cached = BuildReport();

						_cacheManager.Set(cacheKey, cached);
					}

					return cached;
				}

				return BuildReport();
			}

			private MonthlyFinancialReport BuildReport()
			{
				var report = new MonthlyFinancialReport(_thatMonth)
				{
					Entries = _taxon.Elements
						.Select(BuildReportEntry)
						.ToList()
				};

				var incomes = report.Entries
					.Where(n => n.TaxonId == BuiltInTaxonId.Income)
					.ToList();
				var expenses = report.Entries
					.Where(n => n.TaxonId == BuiltInTaxonId.Expense)
					.ToList();

				report.ActualBalance = incomes
					.Sum(n => n.Actual)
					.Subtract(expenses
						.Sum(n => n.Actual));

				report.ForecastedBalance = incomes
					.Sum(n => n.Forecasted)
					.Subtract(expenses
						.Sum(n => n.Forecasted));

				return report;
			}

			private MonthlyFinancialReportEntry BuildReportEntry(TaxonTree currentTaxon)
			{
				var entry = new MonthlyFinancialReportEntry(currentTaxon.Id, currentTaxon.Name, currentTaxon.Description)
				{
					Entries = currentTaxon.Elements
						.Select(BuildReportEntry)
						.ToList()
				};

				var financialJournalEntries = _dispatcher.Fetch(new FinancialJournalQuery(_thatMonth)
				{
					Taxon = new TaxonTreeQuery
					{
						TaxonId = currentTaxon.Id,
						Deep = currentTaxon.Elements.Any() ? 0 : -1
					}
				});

				entry.Actual = entry.Entries
					.Select(n => n.Actual)
					.Concat(financialJournalEntries
						.Select(n => n.Actual))
					.Sum();

				entry.Forecasted = entry.Entries
					.Select(n => n.Forecasted)
					.Concat(financialJournalEntries
						.Select(n => n.Forecasted))
					.Sum();

				return entry;
			}
		}
	}
}