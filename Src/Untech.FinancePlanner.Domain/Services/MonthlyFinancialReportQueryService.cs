using System;
using System.Collections.Generic;
using System.Linq;
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
		IQueryHandler<MonthlyFinancialReportQuery, MonthlyFinancialReport>,
		INotificationHandler<FinancialJournalEntrySaved>,
		INotificationHandler<FinancialJournalEntryDeleted>
	{
		private readonly IQueryDispatcher _dispatcher;
		private readonly ICacheStorage _cacheStorage;

		public MonthlyFinancialReportQueryService(IQueryDispatcher dispatcher, ICacheStorage cacheStorage)
		{
			_dispatcher = dispatcher;
			_cacheStorage = cacheStorage;
		}

		public MonthlyFinancialReport Handle(MonthlyFinancialReportQuery request)
		{
			var cacheKey = GetKey(request.AsMonthDate());
			var report = _cacheStorage.Get<MonthlyFinancialReport>(cacheKey);

			if (report == null)
			{
				report = BuildReport(request);
				_cacheStorage.Set(cacheKey, report);
			}

			return report;
		}

		public void Publish(FinancialJournalEntrySaved notification)
		{
			_cacheStorage.Drop(GetKey(notification.Entry.When));
		}

		public void Publish(FinancialJournalEntryDeleted notification)
		{
			_cacheStorage.Drop(GetKey(notification.Entry.When));
		}

		private MonthlyFinancialReport BuildReport(MonthlyFinancialReportQuery request)
		{
			var dayBuilder = new DayReportBuilder(GetTaxons());

			var entries = _dispatcher.Fetch(new FinancialJournalQuery(request.Year, request.Month)
			{
				Taxon = new TaxonTreeQuery { Deep = -1 }
			});

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

		private IReadOnlyDictionary<int, TaxonTree> GetTaxons() => _dispatcher
			.Fetch(new TaxonTreeQuery { Deep = -1 })
			.DescendantsAndSelf()
			.ToDictionary(n => n.Key);

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

			private MonthlyFinancialReportDayEntry BuildDayReportEntry(FinancialJournalEntry entry) =>
				new MonthlyFinancialReportDayEntry(GetName(entry.TaxonKey), entry.TaxonKey)
				{
					Remarks = entry.Remarks,
					Actual = entry.Actual,
					Forecasted = entry.Forecasted
				};

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