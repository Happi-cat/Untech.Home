using System;
using LinqToDB.Mapping;
using Untech.FinancePlanner.Domain.Models;
using Untech.Practices;

namespace Untech.FinancePlanner.Data
{
	[Table("FinancialJournalEntries")]
	public class FinancialJournalEntryDao
	{
		[Column, PrimaryKey, Identity]
		public int Key { get; set; }

		[Column]
		public int TaxonKey { get; set; }

		[Column]
		public string Remarks { get; set; }

		[Column, NotNull]
		public double ActualAmount { get; set; }

		[Column, NotNull]
		public string ActualCurrency { get; set; }

		[Column, NotNull]
		public double ForecastedAmount { get; set; }

		[Column, NotNull]
		public string ForecastedCurrency { get; set; }

		[Column, NotNull]
		public DateTime When { get; set; }

		public static FinancialJournalEntry Convert(FinancialJournalEntryDao dao)
		{
			return new FinancialJournalEntry(dao.Key, dao.TaxonKey)
			{
				Remarks = dao.Remarks,
				When = dao.When,
				Actual = new Money(dao.ActualAmount, new Currency(dao.ActualCurrency, dao.ActualCurrency)),
				Forecasted = new Money(dao.ForecastedAmount, new Currency(dao.ForecastedCurrency, dao.ForecastedCurrency))
			};
		}

		public static FinancialJournalEntryDao Convert(FinancialJournalEntry entry)
		{
			return new FinancialJournalEntryDao
			{
				Key = entry.Key,
				TaxonKey = entry.TaxonKey,
				Remarks = entry.Remarks,
				When = entry.When,
				ActualAmount = entry.Actual.Amount,
				ActualCurrency = entry.Actual.Currency.Id,
				ForecastedAmount = entry.Forecasted.Amount,
				ForecastedCurrency = entry.Forecasted.Currency.Id
			};
		}
	}
}