using System;
using LinqToDB.Mapping;
using Untech.FinancePlanner.Domain.Models;
using Untech.Practices;

namespace Untech.FinancePlanner.Data
{
	[Table("FinancialJournalEntries")]
	public class FinancialJournalEntryDto
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

		public static FinancialJournalEntry Convert(FinancialJournalEntryDto dto)
		{
			return new FinancialJournalEntry(dto.Key, dto.TaxonKey)
			{
				Remarks = dto.Remarks,
				When = dto.When,
				Actual = new Money(dto.ActualAmount, new Currency(dto.ActualCurrency, dto.ActualCurrency)),
				Forecasted = new Money(dto.ForecastedAmount, new Currency(dto.ForecastedCurrency, dto.ForecastedCurrency))
			};
		}

		public static FinancialJournalEntryDto Convert(FinancialJournalEntry entry)
		{
			return new FinancialJournalEntryDto
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