using System;
using LinqToDB.Mapping;
using Untech.FinancePlanner.Domain.Models;
using Untech.Practices;
using Untech.Practices.DataStorage;
using Untech.Practices.ObjectMapping;

namespace Untech.FinancePlanner.Data
{
	[Table("FinancialJournalEntries")]
	public class FinancialJournalEntryDao : IAggregateRoot
	{
		public static readonly IMap<FinancialJournalEntry, FinancialJournalEntryDao> ToDaoMap =
			new AdHocMap<FinancialJournalEntry, FinancialJournalEntryDao>(t => new FinancialJournalEntryDao(t));

		public static readonly IMap<FinancialJournalEntryDao, FinancialJournalEntry> ToEntityMap =
			new AdHocMap<FinancialJournalEntryDao, FinancialJournalEntry>(ToEntity);

		public FinancialJournalEntryDao()
		{
		}

		public FinancialJournalEntryDao(FinancialJournalEntry entry)
		{
			Key = entry.Key;
			TaxonKey = entry.TaxonKey;
			Remarks = entry.Remarks;
			When = entry.When;
			ActualAmount = entry.Actual.Amount;
			ActualCurrency = entry.Actual.Currency.Id;
			ForecastedAmount = entry.Forecasted.Amount;
			ForecastedCurrency = entry.Forecasted.Currency.Id;
		}

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

		public static FinancialJournalEntry ToEntity(FinancialJournalEntryDao dao)
		{
			return new FinancialJournalEntry(dao.Key, dao.TaxonKey)
			{
				Remarks = dao.Remarks,
				When = dao.When,
				Actual = new Money(dao.ActualAmount, new Currency(dao.ActualCurrency, dao.ActualCurrency)),
				Forecasted = new Money(dao.ForecastedAmount, new Currency(dao.ForecastedCurrency, dao.ForecastedCurrency))
			};
		}
	}
}