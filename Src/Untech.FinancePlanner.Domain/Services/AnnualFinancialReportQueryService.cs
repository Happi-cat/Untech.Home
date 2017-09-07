using System;
using System.Linq;
using Untech.FinancePlanner.Domain.Models;
using Untech.FinancePlanner.Domain.Requests;
using Untech.FinancePlanner.Domain.ViewModels;
using Untech.Practices;
using Untech.Practices.CQRS.Dispatching;
using Untech.Practices.CQRS.Handlers;

namespace Untech.FinancePlanner.Domain.Services
{
	public class AnnualFinancialReportQueryService :
		IQueryHandler<AnnualFinancialReportQuery, AnnualFinancialReport>
	{
		private readonly IDispatcher _dispatcher;

		public AnnualFinancialReportQueryService(IDispatcher dispatcher)
		{
			_dispatcher = dispatcher;
		}

		public AnnualFinancialReport Handle(AnnualFinancialReportQuery request)
		{
			var thisMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
			var rootTaxon = _dispatcher.Fetch(request.Taxon ?? new TaxonTreeQuery { Deep = 2 });

			var builder = new MonthlyReportBuilder(_dispatcher, rootTaxon);

			return new AnnualFinancialReport
			{
				Entries = rootTaxon,
				Months = Enumerable.Range(-3, 12)
					 .Select(thisMonth.AddMonths)
					 .Select(builder.GetReport)
					 .ToList()
			};
		}

		private class MonthlyReportBuilder
		{
			private readonly IDispatcher _dispatcher;
			private readonly TaxonTree _taxon;

			private DateTime _thatMonth;

			public MonthlyReportBuilder(IDispatcher dispatcher, TaxonTree taxon)
			{
				_dispatcher = dispatcher;
				_taxon = taxon;
			}

			public MonthlyFinancialReport GetReport(DateTime thatMonth)
			{
				_thatMonth = thatMonth;

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