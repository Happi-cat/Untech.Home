using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using Untech.FinancePlanner.Data.Cache;

namespace Untech.FinancePlanner.Data
{
	public class FinancialPlannerContext : DataConnection
	{
		public FinancialPlannerContext()
			: base(new SQLiteDataProvider(), "Data Source=financial_planner.db")
		{
		}

		public ITable<CacheEntry> CacheEntries => GetTable<CacheEntry>();

		public ITable<FinancialJournalEntryDto> FinancialJournalEntries => GetTable<FinancialJournalEntryDto>();

		public ITable<TaxonDto> Taxons => GetTable<TaxonDto>();
	}
}