using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Untech.FinancePlanner.Domain.Models;
using Untech.Practices;

namespace Untech.FinancePlanner.Domain.Views
{
	[DataContract]
	public class AnnualFinancialReportMonthEntry
	{
		public AnnualFinancialReportMonthEntry(int taxonKey)
		{
			TaxonKey = taxonKey;
		}

		[DataMember]
		public int TaxonKey { get; private set; }

		[DataMember]
		public Money Actual { get; set; }

		[DataMember]
		public Money ActualTotals { get; set; }

		[DataMember]
		public Money Forecasted { get; set; }

		[DataMember]
		public Money ForecastedTotals { get; set; }

		[DataMember]
		public IReadOnlyCollection<AnnualFinancialReportMonthEntry> Entries { get; set; }

		public static AnnualFinancialReportMonthEntry Create(TaxonTree currentTaxon,
			IEnumerable<FinancialJournalEntry> financialJournalEntries,
			IEnumerable<AnnualFinancialReportMonthEntry> subEntries)
		{
			if (currentTaxon == null) throw new ArgumentNullException(nameof(currentTaxon));
			if (financialJournalEntries == null) throw new ArgumentNullException(nameof(financialJournalEntries));
			if (subEntries == null) throw new ArgumentNullException(nameof(subEntries));

			var entry = new AnnualFinancialReportMonthEntry(currentTaxon.Key)
			{
				Actual = financialJournalEntries.Sum(n => n.Actual),
				Forecasted = financialJournalEntries.Sum(n => n.Forecasted),
				Entries = subEntries
					.Where(e => e.IsActualOrForecastedPresent())
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

		public bool IsActualOrForecastedPresent()
		{
			return new[] { ActualTotals, ForecastedTotals }
				.Select(n => n?.Amount ?? 0)
				.Any(n => n != 0);
		}
	}
}