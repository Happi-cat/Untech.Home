using System;
using System.Collections.Generic;
using System.Linq;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Notifications;
using Untech.FinancePlanner.Domain.Requests;
using Untech.FinancePlanner.Domain.Views;
using Untech.Home;
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
			var thisMonth = DateTime.Today.AsMonthDate();
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

				var entries = _taxon.GetElements()
					.Select(BuildReportEntry);

				return AnnualFinancialReportMonth.Create(thatMonth, entries);
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

				var subEntries = currentTaxon.GetElements()
					.Select(BuildReportEntry);

				return AnnualFinancialReportMonthEntry.Create(currentTaxon, financialJournalEntries, subEntries);
			}
		}
	}
}