﻿using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SQLite;
using Untech.FinancePlanner.Data.Cache;
using Untech.Home;
using Untech.Home.Data;

namespace Untech.FinancePlanner.Data
{
	public class FinancialPlannerContext : DataConnection, IDbInitializer
	{
		public FinancialPlannerContext(IConnectionStringFactory connectionStringFactory)
			: base(new SQLiteDataProvider(), connectionStringFactory.GetConnectionString("financial_planner.db"))
		{
		}

		public ITable<CacheEntry> CacheEntries => GetTable<CacheEntry>();

		public ITable<FinancialJournalEntryDao> FinancialJournalEntries => GetTable<FinancialJournalEntryDao>();

		public ITable<TaxonDao> Taxons => GetTable<TaxonDao>();

		public void InitializeDb()
		{
			this.EnsureTableExists<CacheEntry>();
			this.EnsureTableExists<TaxonDao>();
			this.EnsureTableExists<FinancialJournalEntryDao>();
		}
	}
}