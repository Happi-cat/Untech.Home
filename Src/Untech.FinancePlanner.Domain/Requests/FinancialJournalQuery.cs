using System;
using System.Collections.Generic;
using Untech.FinancePlanner.Domain.Models;
using Untech.Home;
using Untech.Practices.CQRS;

namespace Untech.FinancePlanner.Domain.Requests
{
	public class FinancialJournalQuery : IQuery<IEnumerable<FinancialJournalEntry>>,
		IHasMonthInfo
	{
		public FinancialJournalQuery(int year, int month)
		{
			Year = year;
			Month = month;
		}

		public FinancialJournalQuery(DateTime thatMonth)
		{
			Year = thatMonth.Year;
			Month = thatMonth.Month;
		}

		public int Year { get; }
		public int Month { get; }

		public TaxonTreeQuery Taxon { get; set; }
	}
}